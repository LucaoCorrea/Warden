using Microsoft.AspNetCore.Mvc;
using Warden.Filters;
using Warden.Models;
using Warden.Repository;

namespace Warden.Controllers
{
    [PageForAdmin]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IContactRepository _contactRepository;

        public UserController(IUserRepository userRepository, IContactRepository contactRepository)
        {
            _userRepository = userRepository;
            _contactRepository = contactRepository;
        }

        public ActionResult Index()
        {
            List<UserModel> users = _userRepository.GetAll();
            return View(users);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            UserModel user = _userRepository.GetById(id);
            return View(user);
        }

        public ActionResult ConfirmDelete(int id)
        {
            UserModel user = _userRepository.GetById(id);
            return View(user);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                bool deleted = _userRepository.Delete(id);

                if (deleted) TempData["MensagemSucesso"] = "Usuário apagado com sucesso!"; else TempData["MensagemErro"] = "Ops, não conseguimos apagar seu usuário, tente novamante!";
                return RedirectToAction("Index");
            }
            catch (Exception err)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos apagar seu usuário, tente novamante, detalhe do erro: {err.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult ListContactsByUserId(int id)
        {
            List<ContactModel> contacts = _contactRepository.getAll(id);
            return PartialView("_ContactsUser", contacts);
        }

        [HttpPost]
        public IActionResult Create(UserModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingUsers = _userRepository.GetAll();

                    void AddErrorIfExists(string field, Func<UserModel, bool> predicate, string errorMsg)
                    {
                        if (existingUsers.Any(predicate))
                            ModelState.AddModelError(field, errorMsg);
                    }

                    AddErrorIfExists("Login", u => u.Login == user.Login, "Este login já está em uso.");
                    AddErrorIfExists("Email", u => u.Email == user.Email, "Este e-mail já está em uso.");
                    AddErrorIfExists("Name", u => u.Name == user.Name, "Este nome já está em uso.");

                    if (!ModelState.IsValid)
                        return View(user);

                    _userRepository.Create(user);
                    TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso!";
                    return RedirectToAction("Index");
                }

                return View(user);
            }
            catch (Exception err)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos cadastrar seu usuário. Detalhes do erro: {err.Message}";
                return RedirectToAction("Index");
            }
        }



        [HttpPost]
        public IActionResult Edit(UserNoPasswordModel userNoPassword)
        {
            try
            {
                UserModel user = null;

                if (ModelState.IsValid)
                {
                    user = new UserModel()
                    {
                        Id = userNoPassword.Id,
                        Name = userNoPassword.Name,
                        Login = userNoPassword.Login,
                        Email = userNoPassword.Email,
                        Profile = userNoPassword.Profile,
                    };

                    user = _userRepository.Update(user);
                    TempData["MensagemSucesso"] = "Usuário atualizado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(user);
            }
            catch (Exception err)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos atualizar seu usuário, tente novamante, detalhe do erro: {err.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
