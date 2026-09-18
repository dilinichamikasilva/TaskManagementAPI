using System.ComponentModel.DataAnnotations;

namespace DemoApi.Dtos.Users
{
    public class UpdateUserDto
    {
        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(256)]
        public string Email { get; set; } = string.Empty;
    }
}
