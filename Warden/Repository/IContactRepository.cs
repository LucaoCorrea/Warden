using Warden.Models;

namespace Warden.Repository
{
    public interface IContactRepository
    {
        ContactModel GetById(int id);
        List<ContactModel> getAll(int userId);
        ContactModel Add(ContactModel contactModel);

        ContactModel Update(ContactModel contact);

        bool Delete(int id);

    }
}
