using System.ComponentModel.DataAnnotations;

namespace GamedevQuest.Models.DTO
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username {  get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
    public class LoginResponseDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
