using RestaurantManagement.Domain.Model;

namespace RestaurantManagement.Domain.Helper
{
    public interface IEmailHelper
    {
        Task SendEmail(EmailRequest emailRequest);
    }
}