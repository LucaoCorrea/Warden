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

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateContact(ContactModel contactModel)
        {
            _contactRepository.Add(contactModel);
            return RedirectToAction("Index");
        }
    }
}
