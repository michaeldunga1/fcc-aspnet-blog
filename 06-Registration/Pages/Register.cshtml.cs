using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
public class RegisterModel : PageModel {
  private readonly AppDb _db;
  public RegisterModel(AppDb db) { _db = db; }
  public string? Error { get; set; }
  public void OnGet() {}
  public async Task<IActionResult> OnPostAsync(string username, string email, string password) {
    email = email.Trim().ToLowerInvariant();
    if (password.Length < 8) { Error = "Password must be at least 8 characters"; return Page(); }
    if (_db.Users.Any(u => u.Username == username || u.Email == email)) { Error = "Username or email already taken"; return Page(); }
    var user = new User { Username = username, Email = email, DisplayName = username, PasswordHash = BCrypt.Net.BCrypt.HashPassword(password) };
    _db.Users.Add(user); _db.SaveChanges();

    return RedirectToPage("/Index");
  }
}
