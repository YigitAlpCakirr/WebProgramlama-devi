using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Kullanıcı kaydı
        [HttpPost("signup")]
        public IActionResult Register([FromBody] UserRegistrationDto userDto)
        {
            if (userDto.Username == null || userDto.Password == null)
            {
                return BadRequest(new { success = false, message = "Kullanıcı adı ve şifre boş olamaz!" });
            }

            if (_context.Users.Any(u => u.Username == userDto.Username))
            {
                return BadRequest(new { success = false, message = "Kullanıcı adı zaten var!" });
            }

            var newUser = new User(userDto.Username, userDto.Password);
            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok(new { success = true, message = "Kullanıcı başarıyla kaydedildi!" });
        }

        // Kullanıcı girişi
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto loginDto)
        {
            if (loginDto.Username == null || loginDto.Password == null)
            {
                return BadRequest(new { success = false, message = "Kullanıcı adı ve şifre boş olamaz!" });
            }

            var user = _context.Users.FirstOrDefault(u => u.Username == loginDto.Username);

            if (user == null || !user.VerifyPassword(loginDto.Password))
            {
                return Unauthorized(new { success = false, message = "Geçersiz kullanıcı adı veya şifre!" });
            }

            return Ok(new { success = true, message = "Giriş başarılı!" });
        }
    }
}
