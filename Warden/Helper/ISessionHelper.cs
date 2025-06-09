using Warden.Models;

namespace Warden.Helper
{

    // foi necessario mudar o nome da interface para ISessionHelper,
    // pois o nome SessionHelper conflita com o nome da classe SessionHelper que implementa a interface.
    public interface ISessionHelper
    {
        void CreateUserSession(UserModel userModel);
        void RemoveUserSession();
        UserModel GetUserSession();
    }
}
