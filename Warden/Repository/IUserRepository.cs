using Microsoft.AspNetCore.Identity;
using Warden.Models;

namespace Warden.Repository
{
    public interface IUserRepository
    {
        UserModel GetByLogin(string login);
        UserModel GetByEmailLogin(string email, string login);
        List<UserModel> GetAll();
        UserModel GetById(int Id);
        UserModel Create(UserModel user);
        UserModel Update(UserModel user);
        UserModel EditPassword(EditPasswordModel editPasswordModel); 
        
        bool Delete(int id);

    }
}
