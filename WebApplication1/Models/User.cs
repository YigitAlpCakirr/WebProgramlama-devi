using BCrypt.Net;

namespace WebApplication1.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;  // null olamaz, varsayılan boş değer
        public string PasswordHash { get; set; } = string.Empty;  // null olamaz, varsayılan boş değer
        public bool IsAdmin { get; set; }  // Admin kontrolü için eklenen özellik

        // Parametresiz yapıcı ekliyoruz
        public User() { }

        // Yapıcı metod (constructor) ile null olmayan değerler almak
        public User(string username, string password, bool isAdmin = false) : this()
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));  // Null kontrolü
            SetPassword(password ?? throw new ArgumentNullException(nameof(password)));  // Null kontrolü
            IsAdmin = isAdmin;  // Admin özelliği de ekleniyor
        }

        // Şifreyi hash'lemek için metot
        public void SetPassword(string password)
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);  // Hash'lenmiş şifreyi sakla
        }

        // Şifre doğrulama metodu
        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);  // Veritabanındaki hash ile karşılaştırma yap
        }
    }
}
