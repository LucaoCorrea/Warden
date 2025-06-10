using Warden.Models;
using Newtonsoft.Json;

namespace Warden.Helper
{
    public class Session : ISessionHelper
    {

        private readonly IHttpContextAccessor _httpContext;

        public Session(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public void CreateUserSession(UserModel userModel)
        {
            string value = JsonConvert.SerializeObject(userModel); 
            _httpContext.HttpContext.Session.SetString("UserSession", value);
        }

        public UserModel GetUserSession()
        {
           string userSession = _httpContext.HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userSession)) return null;

            return JsonConvert.DeserializeObject<UserModel>(userSession);
        }

        public void RemoveUserSession()
        {
            _httpContext.HttpContext.Session.Remove("UserSession");
        }
    }
}
