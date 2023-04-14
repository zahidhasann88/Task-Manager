using System.ComponentModel.DataAnnotations;

namespace TaskManager.DTOs
{
    public class UserCreateDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }

    public class UserAuthenticateDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }
        public string Password { get; set; }

        public bool IsDeleted { get; set; }
    }

    public class DeleteDto
    {
        public int Id { get; set; }
    }

    public class GetByIdDto
    {
        public int Id { get; set; }
    }


}
