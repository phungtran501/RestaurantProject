using Newtonsoft.Json;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.UI.Areas.Admin.Models;

namespace RestaurantManagement.UI.wwwroot.admin.Utility
{
    public class SessionHelper
    {
        public static string SessionName = "UserLoginSession";
        private readonly HttpContext _httpContextAccessor;

        public SessionHelper(HttpContext httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public ApplicationUser GetSessionInfoLogin
        {
            get
            {
                ApplicationUser user = new();

                if (_httpContextAccessor.User.Identity.IsAuthenticated)
                {
                    if(_httpContextAccessor.Session.GetString(SessionName) != null)
                    {
                        var session = _httpContextAccessor.Session.GetString(SessionName);

                        var md = JsonConvert.DeserializeObject<ApplicationUser>(session);

                        return md;
                    }
                }

                return user;
            }
        }

        public void CreateSession(ApplicationUser applicationUser)
        {
            if (_httpContextAccessor.Session.GetString(SessionName) == null)
            {
                string mdLogin = JsonConvert.SerializeObject(applicationUser);

                _httpContextAccessor.Session.SetString(SessionName, mdLogin);
            }
        }


    }
}
