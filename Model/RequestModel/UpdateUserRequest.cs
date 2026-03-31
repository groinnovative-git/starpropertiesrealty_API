namespace Star_Properties.Model.RequestModel
{
    public class UpdateUserRequest
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
