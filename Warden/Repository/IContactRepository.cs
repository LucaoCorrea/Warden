using Warden.Models;

namespace Warden.Repository
{
    public interface IContactRepository
    {
        ContactModel Add(ContactModel contactModel);

    }
}
