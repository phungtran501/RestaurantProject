namespace RestaurantManagement.UI.Areas.Admin.Models
{
    public class UserAddressViewModel
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public string Fullname { get; set; }
        public bool IsActive { get; set; }

        public string UserId { get; set; }
    }
}
