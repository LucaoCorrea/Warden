using Warden.Models;

namespace Warden.Repository
{
    public interface IContactRepository
    {

        List<ContactModel> getAll();
        ContactModel Add(ContactModel contactModel);

    }
}
