namespace RestaurantManagement.Service.DTOs
{
    public class UserAddressModel
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public string Fullname { get; set; }
        public bool IsActive { get; set; }

        public string UserName { get; set; }
    }
}
