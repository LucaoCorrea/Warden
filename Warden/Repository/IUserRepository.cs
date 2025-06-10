using Microsoft.AspNetCore.Identity;
using Warden.Models;

namespace Warden.Repository
{
    public interface IUserRepository
    {
        UserModel getByLogin(string login);
        UserModel getByEmailLogin(string email, string login);
        List<UserModel> getAll();
        UserModel getById(int Id);
        UserModel create(UserModel user);
        UserModel update(UserModel user);
        UserModel EditPassword(EditPasswordModel editPasswordModel); 
        
        bool delete(int id);

    }
}
