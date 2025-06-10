using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Warden.Models
{
    public class ContactModel
    {

        // Propriedades do modelo de contato e validações
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite o Nome do Contato.")] // este campo é obrigatório
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 100 caracteres.")] 
        public string Name { get; set; }

        [Required(ErrorMessage = "Digite o E-mail do Contato.")] 
        [EmailAddress(ErrorMessage = "E-mail inválido.")] 
        public string Email { get; set; }

        [Phone(ErrorMessage = "Número de telefone inválido.")] 
        public string Phone { get; set; }

        [ValidateNever]
        public UserModel User { get; internal set; }

        public int? UserId { get; set; }
    }
}

