using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Warden.Models;

public class Menu : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        string sessionUser = HttpContext.Session.GetString("UserSession");

        if (string.IsNullOrEmpty(sessionUser)) return null;

        UserModel user = JsonConvert.DeserializeObject<UserModel>(sessionUser);

        return View(user); // vai bater no menu/default.cshtml
    }
}
