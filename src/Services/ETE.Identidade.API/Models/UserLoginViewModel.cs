using System.ComponentModel.DataAnnotations;

namespace ETE.Identidade.API.Models
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Password { get; set; }
    }
}
