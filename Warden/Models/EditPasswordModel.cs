using System.ComponentModel.DataAnnotations;

namespace Warden.Models
{
    public class EditPasswordModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite a senha atual do usuário")]
        public string NowPassword { get; set; }

        [Required(ErrorMessage = "Digite a nova senha do usuário")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirme a nova senha do usuário")]
        [Compare("NovaSenha", ErrorMessage = "Senha não confere com a nova senha")]
        public string ConfirmNewPassword { get; set; }

    }
}
