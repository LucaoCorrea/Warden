using Microsoft.AspNetCore.Mvc;
using Warden.Helper;
using Warden.Models;
using Warden.Repository;

namespace Warden.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessionHelper _session;
        private readonly IEmail _email;

        public LoginController(IUserRepository userRepository, ISessionHelper session, IEmail email)
        {
            _userRepository = userRepository;
            _session = session;
            _email = email;
        }

        public ActionResult Index()
        {
            if (_session.GetUserSession() != null) return RedirectToAction("Index", "Home");

            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        public IActionResult Logout()
        {
            _session.RemoveUserSession();
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserModel user = _userRepository.GetByLogin(loginModel.Login);
                    if (user != null)
                    {
                        if (user.ValidPassword(loginModel.Password))
                        {
                            _session.CreateUserSession(user);
                            return RedirectToAction("Index", "Home");
                        }
                        TempData["MensagemErro"] = $"Senha do usuário é inválida, tente novamente.";
                    }

                    TempData["MensagemErro"] = $"Usuário e/ou senha inválido(s). Por favor, tente novamente.";
                }
                return View("Index");
            }
            catch (Exception err)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos realizar seu login, tente novamante, detalhe do erro: {err.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult SendLinkResetPassword(ResetPasswordModel resetPassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserModel user = _userRepository.GetByEmailLogin(resetPassword.Email, resetPassword.Login);

                    if (user != null)
                    {
                        string newPassword = user.GenerateNewPassword();
                        string message = $"Olá {user.Name}, sua nova senha é: {newPassword}";

                        bool emailSent = _email.SendEmail(user.Email, "Redefinição de senha", message);

                        if (emailSent)
                        {
                            _userRepository.Update(user);
                            TempData["MensagemSucesso"] = $"E-mail enviado com sucesso para {user.Email} com a nova senha.";
                        }
                        else
                        {
                            TempData["MensagemErro"] = $"Não foi possível enviar o e-mail para {user.Email}. Tente novamente mais tarde.";
                        }
                        return RedirectToAction("Index", "Login");
                    }
                    TempData["MensagemErro"] = $"Não conseguimos redefinir sua senha. Por favor, verifique os dados informados.";
                }

                return View("Index");
            }
            catch (Exception err)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos redefinir sua senha, tente novamente, detalhe do erro: {err.Message}";
                return RedirectToAction("ResetPassword");
            }
        }
    }
}
