namespace Star_Properties.Model.RequestModel
{
    public class DeleteUserCredientialsRequest
    {
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
