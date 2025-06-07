using Microsoft.AspNetCore.Mvc;
using Warden.Data;
using Warden.Models;
using Warden.Repository;

namespace Warden.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactRepository _contactRepository;
        public ContactController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        

        public IActionResult Index()
        {
            List<ContactModel> contact = _contactRepository.getAll();
            return View(contact);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            ContactModel contact = _contactRepository.GetById(id);
            return View(contact);
        }

        public IActionResult Delete(int id)
        {
            ContactModel contact = _contactRepository.GetById(id);
            return View(contact);
        }

        [HttpPost]
        public IActionResult CreateContact(ContactModel contactModel)
        {
            _contactRepository.Add(contactModel);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditContact(ContactModel contactModel)
        {
            _contactRepository.Update(contactModel);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteContact(int id)
        {
            _contactRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
