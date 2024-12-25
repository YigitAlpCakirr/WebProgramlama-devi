using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WebApplication1.Data;
using WebApplication1.Models;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // Giriþ sayfasý (GET)
    [HttpGet]
    public IActionResult Login()
    {
        return View("~/Views/Home/Login.cshtml");
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserLoginDto loginDto)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == loginDto.Username);

        if (user == null || !user.VerifyPassword(loginDto.Password))
        {
            // Eðer kullanýcý bulunmazsa veya þifre yanlýþsa, hata mesajý döner
            return Unauthorized(new { success = false, message = "Geçersiz kullanýcý adý veya þifre!" });
        }

        // Kullanýcý doðrulandý, admin kontrolü ekleyin
        if (loginDto.Username == "g231210387@sakarya.edu.tr" && loginDto.Password == "sau")
        {
            // Admin giriþini baþarýyla kontrol ettik
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, "Admin")
        };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Kullanýcýyý oturum açýyoruz
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Admin sayfasýna yönlendiriyoruz
            return RedirectToAction("Admin", "Home");  // Yönlendirme yapýlmasý gereken sayfa
        }
        else
        {
            // Normal kullanýcý, admin paneline yönlendirme
            return RedirectToAction("Index", "Home");  // Ana sayfaya yönlendir
        }
    }


    // Admin sayfasý
    [Authorize(Roles = "Admin")]  // Yalnýzca Admin rolüne sahip kullanýcýlar eriþebilir
    public IActionResult Admin()
    {
        return View();
    }

    // Eriþim engellendiðinde gösterilecek sayfa
    public IActionResult AccessDenied()
    {
        return View("AccessDenied"); // Eriþim Reddedildi görünümünü döndür
    }

    // Çýkýþ iþlemi
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        // Kullanýcýyý çýkartýyoruz
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // Ana sayfaya yönlendiriyoruz
        return RedirectToAction("Index", "Home");
    }
}
