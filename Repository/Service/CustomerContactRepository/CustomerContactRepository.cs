using Star_Properties.DbConfiguration;
using Star_Properties.Model.EntityModel;
using Star_Properties.Model.RequestModel;
using Star_Properties.Model.ResponseModel;
using Star_Properties.Repository.Interface.ICustomerContactRepository;
using Microsoft.EntityFrameworkCore;

namespace Star_Properties.Repository.Service.CustomerContactRepository
{
    public class CustomerContactRepository : ICustomerContactRepository
    {
        private readonly ApplicationDbContext _context;
        public CustomerContactRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerContactResponse> SubmitContact(CustomerContactRequest request)
        {
            var customer = new CustomerContactMaster
            {
                ContactId = Guid.NewGuid(),
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                CustomerInterest = request.CustomerInterest,
                Message = request.Message,
                PropertyId = request.PropertyId,
                SubmittedDate = DateTime.UtcNow,
                LeadStatus = request.LeadStatus ?? "New"
            };

            _context.CustomerContactMaster.Add(customer);
            await _context.SaveChangesAsync();

            return MapToResponse(customer);
        }

        private CustomerContactResponse MapToResponse(CustomerContactMaster customer)
        {
            return new CustomerContactResponse
            {
                ContactId = customer.ContactId,
                FullName = customer.FullName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                CustomerInterest = customer.CustomerInterest,
                Message = customer.Message,
                PropertyId = customer.PropertyId,
                SubmittedDate = customer.SubmittedDate,
                LeadStatus = customer.LeadStatus,
                ModifiedBy = customer.ModifiedBy,
                ModifiedOn = customer.ModifiedOn
            };
        }

        public async Task<List<CustomerContactResponse>> GetAllContacts()
        {
            return _context.CustomerContactMaster
                .Select(x => new CustomerContactResponse
                {
                    ContactId = x.ContactId,
                    FullName = x.FullName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    CustomerInterest = x.CustomerInterest,
                    Message = x.Message,
                    PropertyId = x.PropertyId,
                    SubmittedDate = x.SubmittedDate,
                    LeadStatus = x.LeadStatus,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedOn = x.ModifiedOn
                }).ToList();
        }

        //public async Task<CustomerContactResponse> UpdateContact(CustomerContactRequest request, Guid userId)
        //{
        //    var customerData = await _context.CustomerContactMaster
        //        .FindAsync(request.ContactId);

        //    if (customerData == null)
        //        throw new Exception("Contact not found");

        //    var audits = new List<CustomerContactAudit>();

        //    // Track changes

        //    if (customerData.FullName != request.FullName)
        //    {
        //        audits.Add(CreateAudit(customerData.ContactId, "FullName", customerData.FullName, request.FullName, userId));
        //        customerData.FullName = request.FullName;
        //    }

        //    if (customerData.Email != request.Email)
        //    {
        //        audits.Add(CreateAudit(customerData.ContactId, "Email", customerData.Email, request.Email, userId));
        //        customerData.Email = request.Email;
        //    }

        //    if (customerData.PhoneNumber != request.PhoneNumber)
        //    {
        //        audits.Add(CreateAudit(customerData.ContactId, "PhoneNumber", customerData.PhoneNumber, request.PhoneNumber, userId));
        //        customerData.PhoneNumber = request.PhoneNumber;
        //    }

        //    if (customerData.LeadStatus != request.LeadStatus)
        //    {
        //        audits.Add(CreateAudit(customerData.ContactId, "LeadStatus", customerData.LeadStatus, request.LeadStatus, userId));
        //        customerData.LeadStatus = request.LeadStatus;
        //    }

        //    // Update audit fields
        //    customerData.ModifiedBy = userId;
        //    customerData.ModifiedOn = DateTime.UtcNow;

        //    // Save audits
        //    if (audits.Any())
        //        await _context.CustomerContactAudit.AddRangeAsync(audits);

        //    await _context.SaveChangesAsync();

        //    return MapToResponse(customerData);
        //}

        public async Task<CustomerContactUpdateResponse> UpdateContact(CustomerContactRequest request, Guid userId)
        {
            var customerData = await _context.CustomerContactMaster
                .FindAsync(request.ContactId);

            if (customerData == null)
                throw new Exception("Contact not found");

            var audits = new List<CustomerContactAudit>();

            // Track changes
            if (customerData.FullName != request.FullName)
            {
                audits.Add(CreateAudit(customerData.ContactId, "FullName", customerData.FullName, request.FullName, userId));
                customerData.FullName = request.FullName;
            }

            if (customerData.Email != request.Email)
            {
                audits.Add(CreateAudit(customerData.ContactId, "Email", customerData.Email, request.Email, userId));
                customerData.Email = request.Email;
            }

            if (customerData.PhoneNumber != request.PhoneNumber)
            {
                audits.Add(CreateAudit(customerData.ContactId, "PhoneNumber", customerData.PhoneNumber, request.PhoneNumber, userId));
                customerData.PhoneNumber = request.PhoneNumber;
            }

            if (customerData.LeadStatus != request.LeadStatus)
            {
                audits.Add(CreateAudit(customerData.ContactId, "LeadStatus", customerData.LeadStatus, request.LeadStatus, userId));
                customerData.LeadStatus = request.LeadStatus;
            }

            // Update audit fields
            customerData.ModifiedBy = userId;
            customerData.ModifiedOn = DateTime.UtcNow;

            // Save audits
            if (audits.Any())
                await _context.CustomerContactAudit.AddRangeAsync(audits);

            await _context.SaveChangesAsync();

            // FETCH UPDATED AUDIT LIST
            var auditList = await _context.CustomerContactAudit
                .Where(x => x.ContactId == customerData.ContactId)
                .OrderByDescending(x => x.ModifiedOn)
                .Join(
                    _context.UserMaster,
                    audit => audit.ModifiedBy,
                    user => user.UserId,
                    (audit, user) => new CustomerContactAuditResponse
                    {
                        Description = audit.FieldName + " changed from '" + audit.OldValue + "' to '" + audit.NewValue +
                            "' by " + user.Name +
                            " on " + audit.ModifiedOn.ToString("dd-MMM-yyyy hh:mm tt"),

                        Name = user.Name,

                        ModifiedOn = audit.ModifiedOn
                    }
                )
                .ToListAsync();

            return new CustomerContactUpdateResponse
            {
                Contact = MapToResponse(customerData),
                AuditHistory = auditList
            };
        }

        private CustomerContactAudit CreateAudit(Guid contactId, string field, string oldValue, string newValue, Guid userId)
        {
            return new CustomerContactAudit
            {
                AuditId = Guid.NewGuid(),
                ContactId = contactId,
                FieldName = field,
                OldValue = oldValue,
                NewValue = newValue,
                ModifiedBy = userId,
                ModifiedOn = DateTime.UtcNow
            };
        }

        public async Task<List<CustomerContactAuditResponse>> GetContactAuditDetails(Guid contactId)
        {
            var auditList = await _context.CustomerContactAudit
                .Where(x => x.ContactId == contactId)
                .OrderByDescending(x => x.ModifiedOn)
                .Join(
                    _context.UserMaster,
                    audit => audit.ModifiedBy,
                    user => user.UserId,
                    (audit, user) => new CustomerContactAuditResponse
                    {
                        Description = audit.FieldName + " changed from '" + audit.OldValue + "' to '" + audit.NewValue +
                                      "' by " + user.Name +
                                      " on " + audit.ModifiedOn.ToString("dd-MMM-yyyy hh:mm tt"),

                        Name = user.Name,  

                        ModifiedOn = audit.ModifiedOn
                    }
                )
                .ToListAsync();

            return auditList;
        }
    }
}
