using System.ComponentModel.DataAnnotations;

namespace GamedevQuest.Models.DTO
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username {  get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
    public class LoginResponseDto
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
