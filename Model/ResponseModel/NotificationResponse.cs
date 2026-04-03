namespace Star_Properties.Model.ResponseModel
{
    public class NotificationResponse
    {
        public Guid NotificationId { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
