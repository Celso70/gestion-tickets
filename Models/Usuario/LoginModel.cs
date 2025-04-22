using System.ComponentModel.DataAnnotations;//para los atributos de configuracion de modelos

namespace GestionTickets.Models.Usuario
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty; //empty para evitar nulls

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }

    public class RefreshTokenRequest 
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }

    public class LogoutRequest
    {
        [Required]
        public string Email { get; set; }
    }
}