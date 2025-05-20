using System.ComponentModel.DataAnnotations;

namespace TFC.AppEventos.Application.DTO
{
    public class AuthDto
    {

        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
