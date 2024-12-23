namespace WebApplication1.Models
{
    public class UserLoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public UserLoginDto(string username, string password)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }
    }

}
