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
            try
            {
                if (ModelState.IsValid)
                {
                    _contactRepository.Add(contactModel);
                    TempData["MensagemSucesso"] = "Contato Criado com Sucesso.";
                    return RedirectToAction("Index");
                }

                return View(contactModel);
            }
            catch (System.Exception err) 
            {
                TempData["MensagemErro"] = $"Erro ao criar Contato. {err.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult EditContact(ContactModel contactModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _contactRepository.Update(contactModel);
                    TempData["MensagemSucesso"] = "Contato Atualizado com Sucesso.";
                    return RedirectToAction("Index");
                }

                return View("Edit", contactModel);
            }
            catch (System.Exception err)
            {
                TempData["MensagemErro"] = $"Erro ao Atualizar Contato. {err.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult DeleteContact(int id)
        {
            try
            {
               bool deleted = _contactRepository.Delete(id);

                if (deleted)
                {
                    TempData["MensagemSucesso"] = "Contato Excluido com Sucesso.";
                }
                else
                {
                    TempData["MensagemErro"] = "Não Conseguimos Excluir o Contato.";
                }
                    return RedirectToAction("Index");
            }
            catch (System.Exception err)
            {
                TempData["MensagemErro"] = $"Não Conseguimos Excluir o Contato {err.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
