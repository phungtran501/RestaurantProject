namespace RestaurantManagement.Domain.Model
{
    public class EmailRequest
    {
        public string EmailCustomer { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string[] AttachmentFilePaths { get; set; } = Array.Empty<string>();
    }
}
