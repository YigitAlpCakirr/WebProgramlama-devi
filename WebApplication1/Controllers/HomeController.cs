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

    // Giri� sayfas� (GET)
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
            // E�er kullan�c� bulunmazsa veya �ifre yanl��sa, hata mesaj� d�ner
            return Unauthorized(new { success = false, message = "Ge�ersiz kullan�c� ad� veya �ifre!" });
        }

        // Kullan�c� do�ruland�, admin kontrol� ekleyin
        if (loginDto.Username == "g231210387@sakarya.edu.tr" && loginDto.Password == "sau")
        {
            // Admin giri�ini ba�ar�yla kontrol ettik
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, "Admin")
        };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Kullan�c�y� oturum a��yoruz
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Admin sayfas�na y�nlendiriyoruz
            return RedirectToAction("Admin", "Home");  // Y�nlendirme yap�lmas� gereken sayfa
        }
        else
        {
            // Normal kullan�c�, admin paneline y�nlendirme
            return RedirectToAction("Index", "Home");  // Ana sayfaya y�nlendir
        }
    }


    // Admin sayfas�
    [Authorize(Roles = "Admin")]  // Yaln�zca Admin rol�ne sahip kullan�c�lar eri�ebilir
    public IActionResult Admin()
    {
        return View();
    }

    // Eri�im engellendi�inde g�sterilecek sayfa
    public IActionResult AccessDenied()
    {
        return View("AccessDenied"); // Eri�im Reddedildi g�r�n�m�n� d�nd�r
    }

    // ��k�� i�lemi
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        // Kullan�c�y� ��kart�yoruz
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // Ana sayfaya y�nlendiriyoruz
        return RedirectToAction("Index", "Home");
    }
}
