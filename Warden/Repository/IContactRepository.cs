using Warden.Models;

namespace Warden.Repository
{
    public interface IContactRepository
    {
        ContactModel GetById(int id);
        List<ContactModel> getAll(int userId);
        ContactModel Add(ContactModel contactModel);

        ContactModel Update(ContactModel contactModel);

        bool Delete(int id);

    }
}
