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
            if (_session.GetUserSession() != null)
            {
                return RedirectToAction("Index", "Home");
            }
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
                    UserModel userModel = _userRepository.getByLogin(loginModel.Login);

                    if (userModel != null)
                    {
                        if (userModel.ValidPassowrd(loginModel.Password))
                        {
                            _session.CreateUserSession(userModel);
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
                TempData["MensagemErro"] = $"Ocorreu um erro ao tentar realizar o login: {err.Message}";
                return View("Index");
            }
        }

        [HttpPost]
        public IActionResult SendLinkResetPassword(ResetPasswordModel resetPassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserModel user = _userRepository.getByEmailLogin(resetPassword.Email, resetPassword.Login);

                    if (user != null)
                    {
                        string newPassword = user.GenerateNewPassword();
                        string message = $"Sua nova senha é: {newPassword}";

                        bool emailsend = _email.SendEmail(user.Email, "Sistema de Contatos - Nova Senha", message);

                        if (emailsend)
                        {
                            _userRepository.update(user);
                            TempData["MensagemSucesso"] = $"Enviamos para seu e-mail cadastrado uma nova senha.";
                        }
                        else
                        {
                            TempData["MensagemErro"] = $"Não conseguimos enviar e-mail. Por favor, tente novamente.";
                        }

                        return RedirectToAction("Index", "Login");
                    }

                    TempData["MensagemErro"] = $"Não conseguimos redefinir sua senha. Por favor, verifique os dados informados.";
                }

                return View("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos redefinir sua senha, tente novamante, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }

        }
    }
}
