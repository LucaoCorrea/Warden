using Microsoft.AspNetCore.Mvc;
using Warden.Filters;
using Warden.Helper;
using Warden.Models;
using Warden.Repository;

namespace Warden.Controllers
{
    [PageForLoggedInUser]
    public class ContactController : Controller
    {
        private readonly IContactRepository _contactRepository;
        private readonly ISessionHelper _session;

        public ContactController(IContactRepository contactRepository, ISessionHelper session)
        {
            _contactRepository = contactRepository;
            _session = session;
        }

        public IActionResult Index()
        {
            UserModel userLogged = _session.GetUserSession();
            if (userLogged == null) return RedirectToAction("Login", "Account");

            List<ContactModel> contacts = _contactRepository.getAll(userLogged.Id);
            return View(contacts);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            ContactModel contact = _contactRepository.GetById(id);
            if (contact == null) return NotFound();
            return View(contact);
        }

        public IActionResult Delete(int id)
        {
            ContactModel contact = _contactRepository.GetById(id);
            if (contact == null) return NotFound();
            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ContactModel contactModel)
        {
            var userLogged = _session.GetUserSession();
            if (userLogged == null) return RedirectToAction("Login", "Account");

            contactModel.UserId = userLogged.Id;

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["MensagemErro"] = string.Join(" | ", errors);
                return View(contactModel);
            }

            _contactRepository.Add(contactModel);

            TempData["MensagemSucesso"] = "Contato cadastrado com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(ContactModel contactModel)
        {
            if (!ModelState.IsValid) return View(contactModel);

            UserModel userLogged = _session.GetUserSession();
            if (userLogged == null) return RedirectToAction("Login", "Account");

            contactModel.UserId = userLogged.Id;
            _contactRepository.Update(contactModel);

            TempData["MensagemSucesso"] = "Contato alterado com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            bool deleted = _contactRepository.Delete(id);

            if (deleted)
            {
                TempData["MensagemSucesso"] = "Contato excluído com sucesso!";
            }
            else
            {
                TempData["MensagemErro"] = "Não foi possível excluir o contato.";
            }

            return RedirectToAction("Index");
        }
    }
}