using Microsoft.EntityFrameworkCore;
using Warden.Data;
using Warden.Models;

namespace Warden.Repository
{
    public class UserRepository : IUserRepository
    {

        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public UserModel create(UserModel user)
        {
           user.CreatedAt = DateTime.Now;
            // passar o password para o hash
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public bool delete(int Id)
        {
            UserModel userDB = getById(Id);

            if (userDB == null)
            {
                throw new Exception("Houve um erro ao excluir o usuário!");
            }

            _context.Users.Remove(userDB);  
            _context.SaveChanges();
            return true;
        }

        public UserModel EditPassword(EditPasswordModel editPasswordModel) //classe para Alterar a senha do usuário
        {
            UserModel userDB = getById(editPasswordModel.Id);

            if (userDB == null)
            {
                throw new Exception("Usuário não encontrado!");
            }

            if(!userDB.ValidPassowrd(editPasswordModel.NowPassword)) throw new Exception("Senha não confere!");

            if(userDB.ValidPassowrd(editPasswordModel.NewPassword)) throw new Exception("A nova senha não pode ser igual a senha atual!");

            userDB.SetNewPassword(editPasswordModel.NewPassword);
            userDB.UpdatedAt = DateTime.Now;
            _context.Users.Update(userDB);
            _context.SaveChanges();

            return userDB;

        }

        public UserModel EditPassword()
        {
            throw new NotImplementedException();
        }

        public List<UserModel> getAll()
        {
            return _context.Users.Include(u => u.Contacts).ToList();
        }

        public UserModel getByEmailLogin(string Email, string Login)
        {
            return _context.Users.FirstOrDefault(u => u.Email.ToUpper() == Email.ToUpper() && u.Login.ToUpper() == Login.ToUpper());
        }

        public UserModel getById(int Id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == Id);
        }

        public UserModel getByLogin(string Login)
        {
            return _context.Users.FirstOrDefault(u => u.Login.ToUpper() == Login.ToUpper());
        }

        public UserModel update(UserModel user)
        {
            UserModel userDB = getById(user.Id);
            if (userDB == null)
            {
                throw new Exception("Houve um erro na atualização do usuário!");
            }

            userDB.Name = user.Name;
            userDB.Login = user.Login;
            userDB.Email = user.Email;
            userDB.Profile = user.Profile;
            userDB.UpdatedAt = DateTime.Now;

            _context.Users.Update(userDB);
            _context.SaveChanges();

            return userDB;
        }
    }
}
