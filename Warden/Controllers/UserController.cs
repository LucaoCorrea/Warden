using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using Warden.Models;
using Warden.Repository;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Warden.Controllers
{
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
            List<UserModel> users = _userRepository.getAll();
            return View(users);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            UserModel user = _userRepository.getById(id);
            return View(user);
        }

        public ActionResult ConfirmDelete(int id)
        {
            UserModel user = _userRepository.getById(id);
            return View(user);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                bool deleted = _userRepository.delete(id);

                if (deleted) TempData["MensagemSucesso"] = "Usuário apagado com sucesso!"; else TempData["MensagemErro"] = "Ops, não conseguimos apagar seu usuário, tente novamante!";
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos apagar seu usuário, tente novamante, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult ListContactsByUserId(int userId)
        {
            List<ContactModel> contacts = _contactRepository.getAll(userId);
            if (contacts == null || contacts.Count == 0)
            {
                TempData["MensagemErro"] = "Nenhum contato encontrado para este usuário.";
                return RedirectToAction("Index");
            }
            return PartialView("_ContactsList", contacts);
        }

        // EndPoints
        [HttpPost]
        public IActionResult Create(UserModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserModel userCreated = _userRepository.create(user);
                    TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(user);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos cadastrar seu usuário, tente novamante, detalhe do erro: {erro.Message}";
                return View(user);
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

                    user = _userRepository.update(user);
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
