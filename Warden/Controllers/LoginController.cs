using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<LoginController> _logger;

        public LoginController(IUserRepository userRepository, ISessionHelper session, IEmail email, ILogger<LoginController> logger)
        {
            _userRepository = userRepository;
            _session = session;
            _email = email;
            _logger = logger;
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
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel loginModel)
        {
            try
            {
                _logger.LogInformation("Tentativa de login para: {Login}", loginModel.Login);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("ModelState inválido para: {Login}", loginModel.Login);
                    return View("Index", loginModel);
                }

                var userModel = _userRepository.getByLogin(loginModel.Login);

                if (userModel == null)
                {
                    _logger.LogWarning("Usuário não encontrado: {Login}", loginModel.Login);
                }
                else if (!userModel.ValidPassword(loginModel.Password))
                {
                    _logger.LogWarning("Senha incorreta para usuário: {Login}", loginModel.Login);
                }

                if (userModel == null || !userModel.ValidPassword(loginModel.Password))
                {
                    TempData["MensagemErro"] = "Usuário e/ou senha inválido(s). Por favor, tente novamente.";
                    return View("Index", loginModel);
                }

                _session.CreateUserSession(userModel);
                _logger.LogInformation("Login bem-sucedido: {Login}", loginModel.Login);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception err)
            {
                _logger.LogError(err, "Erro ao tentar login para: {Login}", loginModel.Login);
                TempData["MensagemErro"] = $"Ocorreu um erro ao tentar realizar o login: {err.Message}";
                return View("Index", loginModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendLinkResetPassword(ResetPasswordModel resetPassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("Solicitação de reset de senha: Login={Login}, Email={Email}", resetPassword.Login, resetPassword.Email);

                    var user = _userRepository.getByEmailLogin(resetPassword.Email, resetPassword.Login);

                    if (user != null)
                    {
                        string newPassword = user.GenerateNewPassword();
                        string message = $"Sua nova senha é: {newPassword}";

                        bool emailSend = _email.SendEmail(user.Email, "Sistema de Contatos - Nova Senha", message);

                        if (emailSend)
                        {
                            _userRepository.update(user);
                            _logger.LogInformation("Senha redefinida e e-mail enviado com sucesso para: {Email}", user.Email);
                            TempData["MensagemSucesso"] = "Enviamos para seu e-mail cadastrado uma nova senha.";
                        }
                        else
                        {
                            _logger.LogWarning("Falha ao enviar e-mail para: {Email}", user.Email);
                            TempData["MensagemErro"] = "Não conseguimos enviar e-mail. Por favor, tente novamente.";
                        }

                        return RedirectToAction("Index", "Login");
                    }

                    _logger.LogWarning("Usuário não encontrado para redefinição de senha: {Login}", resetPassword.Login);
                    TempData["MensagemErro"] = "Não conseguimos redefinir sua senha. Por favor, verifique os dados informados.";
                }

                return View("ResetPassword", resetPassword);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "Erro ao tentar redefinir senha para: {Login}", resetPassword.Login);
                TempData["MensagemErro"] = $"Ops, não conseguimos redefinir sua senha, tente novamente, detalhe do erro: {err.Message}";
                return RedirectToAction("ResetPassword");
            }
        }
    }
}
