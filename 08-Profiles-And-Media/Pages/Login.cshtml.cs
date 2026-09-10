using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
public class LoginModel : PageModel {
  private readonly AppDb _db;
  public LoginModel(AppDb db) { _db = db; }
  public string? Error { get; set; }
  public void OnGet() {}
  public async Task<IActionResult> OnPostAsync(string email, string password) {
    email = email.Trim().ToLowerInvariant();
    var user = _db.Users.FirstOrDefault(u => u.Email == email);
    if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) { Error = "Invalid email or password"; return Page(); }
    var claims = new List<System.Security.Claims.Claim> {
      new("uid", user.Id.ToString()),
      new(System.Security.Claims.ClaimTypes.Name, user.DisplayName),
      new(System.Security.Claims.ClaimTypes.Email, user.Email)
    };
    await HttpContext.SignInAsync("Cookies", new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity(claims, "Cookies")));
    return RedirectToPage("/Index");
  }
}
