﻿using System.ComponentModel.DataAnnotations;
using Warden.Enums;
using Warden.Helper;

namespace Warden.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite o nome do usuário")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Digite o login do usuário")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Digite o e-mail do usuário")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é valido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe o perfil do usuário")]
        public ProfileEnum? Profile { get; set; }

        [Required(ErrorMessage = "Digite a senha do usuário")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual List<ContactModel> Contacts { get; set; } = new List<ContactModel>();

        public bool ValidPassword(string password)
        {
            string passwordHash = password.GenerateHash();
            return Password == passwordHash;
        }

        public void SetPasswordHash()
        {
            Password = Password.GenerateHash();
        }

        public void SetNewPassword(string newPassword)
        {
            Password = newPassword.GenerateHash();
        }

        public string GenerateNewPassword()
        {
            string newPassword = Guid.NewGuid().ToString().Substring(0, 8);
            Password = newPassword.GenerateHash();
            return newPassword;
        }
    }
}
