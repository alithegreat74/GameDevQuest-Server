using System.ComponentModel.DataAnnotations;

namespace GamedevQuest.Models.DTO
{
    public class SignupRequestDto
    {
        [EmailAddress(ErrorMessage = "Invalid Email format")]
        [Required(ErrorMessage = "Email Address is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
    public class SignupResponseDto
    {
        public int Id { get; private set; }
        public string Token { get; private set; }
        public string Email { get; private set; }
        public string Username { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public SignupResponseDto(User user, string token)
        {
            Id = user.Id;
            Email = user.Email;
            Username = user.Username;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Token = token;
        }
    }
}
