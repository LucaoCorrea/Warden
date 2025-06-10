using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Warden.Models;

public class Menu : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        string sessionUser = HttpContext.Session.GetString("sessionUserLogged");

        if (string.IsNullOrEmpty(sessionUser))
            return Content(string.Empty); 

        UserModel user = JsonConvert.DeserializeObject<UserModel>(sessionUser);

        return View(user); // vai bater no menu/default.cshtml
    }
}
