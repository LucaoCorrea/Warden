using Microsoft.AspNetCore.Identity;
using Warden.Models;

namespace Warden.Repository
{
    public interface IUserRepository
    {
        UserModel getByLogin(string Login);
        UserModel getByEmailLogin(string Email, string Login);
        List<UserModel> getAll();
        UserModel getById(int Id);
        UserModel create(UserModel user);
        UserModel update(UserModel user);
        UserModel EditPassword(EditPasswordModel editPasswordModel); //passar classe EditPasswordModel
        
        bool delete(int Id);

    }
}
