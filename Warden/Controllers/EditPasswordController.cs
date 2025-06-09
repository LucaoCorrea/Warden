using Microsoft.AspNetCore.Mvc;
using Warden.Helper;
using Warden.Models;
using Warden.Repository;

namespace Warden.Controllers
{
    public class EditPasswordController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessionHelper _session;

        public EditPasswordController(IUserRepository userRepository, ISessionHelper session)
        {
            _userRepository = userRepository;
            _session = session;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(EditPasswordModel editPasswordModel)
        {
            try
            {
                UserModel user = _session.GetUserSession();
                editPasswordModel.Id = user.Id;

                if (ModelState.IsValid)
                {
                    _userRepository.EditPassword(editPasswordModel);
                    TempData["MensagemSucesso"] = "Senha alterada com sucesso!";
                    return RedirectToAction("Index", "Home");
                }
                return View("Index", editPasswordModel);
            }
            catch (Exception err)
            {
                TempData["MensagemErro"] = $"Ocorreu um erro ao tentar alterar a senha: {err.Message}";
                return View("Index", editPasswordModel);
            }

        }

    }
}
