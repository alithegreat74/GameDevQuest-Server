using System.ComponentModel.DataAnnotations;

namespace GamedevQuest.Models.DTO
{
    public class SignupRequestDto
    {
        [EmailAddress(ErrorMessage = "Invalid Email format")]
        [Required(ErrorMessage = "Email Address is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
    public class SignupResponseDto
    {
        public int Id { get; private set; }
        public string Email { get; private set; }
        public SignupResponseDto(User user)
        {
            Id = user.Id;
            Email = user.Email;
        }
    }
}
