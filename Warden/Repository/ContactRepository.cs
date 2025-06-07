using Warden.Data;
using Warden.Models;

namespace Warden.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly AppDbContext _dbContext;
        public ContactRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<ContactModel> getAll()
        {
            return _dbContext.Contacts.ToList();
        }

        public ContactModel Add(ContactModel contactModel)
        {
            _dbContext.Contacts.Add(contactModel);
            _dbContext.SaveChanges();
            return contactModel;
        }

        public ContactModel GetById(int id)
        {
            return _dbContext.Contacts.FirstOrDefault(x => x.Id == id);
        }

        public ContactModel Update(ContactModel contact)
        {
            ContactModel contactDB = GetById(contact.Id);

            if (contactDB == null) throw new InvalidOperationException("Erro em atualizar o Contato."); // Não usei SystemException porque é genérica e interna do .NET. Usei InvalidOperationException para clareza.

            contactDB.Name = contact.Name;
            contactDB.Email = contact.Email;
            contactDB.Phone = contact.Phone;

            _dbContext.Contacts.Update(contactDB);
            _dbContext.SaveChanges();   

            return contactDB;
        }

        public bool Delete(int id)
        {
            ContactModel contactDB = GetById(id);

            if (contactDB == null) throw new InvalidOperationException("Erro na exclusão do Contato.");

            _dbContext.Contacts.Remove(contactDB);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
