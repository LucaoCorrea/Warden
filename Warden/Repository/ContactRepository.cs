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

        public ContactModel Add(ContactModel contactModel)
        {
            _dbContext.Contacts.Add(contactModel);
            _dbContext.SaveChanges();
            return contactModel;
        }
    }
}
