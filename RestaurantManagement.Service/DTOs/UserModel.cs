namespace RestaurantManagement.Service.DTOs
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string Fullname { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystem { get; set; }
        public string RoleName { get; set; }
    }
}
