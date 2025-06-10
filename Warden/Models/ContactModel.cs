using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

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
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Telefone deve ter entre 8 e 20 caracteres.")] 
        public string Phone { get; set; }
        public UserModel User { get; internal set; }

        public int? UserId { get; set; }
    }
}

