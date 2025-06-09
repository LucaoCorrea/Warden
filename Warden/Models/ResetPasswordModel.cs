using System.ComponentModel.DataAnnotations;

namespace Warden.Models
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Digite o login")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Digite o e-mail")]
        public string Email { get; set; }
    }
}
