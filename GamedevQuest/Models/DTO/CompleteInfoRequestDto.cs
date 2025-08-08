using System.ComponentModel.DataAnnotations;

namespace GamedevQuest.Models.DTO
{
    public class CompleteInfoRequestDto
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; private set; }
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; private set; }
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; private set; }
        public CompleteInfoRequestDto(string firstName, string lastName, string username)
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username;
        }
    }
    public class CompleteInfoResponseDto
    {
        public int Id { get; private set; }
        public string Email { get; private set; }
        public string Username { set; private get; }
        public string FirstName { set; private get; }
        public string LastName { set; private get; }
        public CompleteInfoResponseDto(User user)
        {
            if(user==null)
                throw new ArgumentNullException("user");
            Id = user.Id;
            Email = user.Email;
            Username = user.Username;
            FirstName = user.FirstName;
            LastName = user.LastName;
        }
    }
}
