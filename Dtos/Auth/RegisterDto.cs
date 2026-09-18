using System.ComponentModel.DataAnnotations;

namespace DemoApi.Dtos.Auth
{
    public class RegisterDto
    {
        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(8), MaxLength(100)]
        public string Password { get; set; } = string.Empty;
    }
}
