using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
public class ResetPasswordModel : PageModel {
  private readonly AppDb _db;
  public ResetPasswordModel(AppDb db) { _db = db; }
  public string Token { get; set; } = "";
  public string? Error { get; set; }
  public void OnGet(string token) { Token = token; }
  public IActionResult OnPost(string token, string password) {
    Token = token;
    var user = _db.Users.FirstOrDefault(u => u.ResetToken == token);
    if (user == null || user.ResetExpires == null || user.ResetExpires < DateTimeOffset.UtcNow) { Error = "Invalid or expired token"; return Page(); }
    if (password.Length < 8) { Error = "Password too short"; return Page(); }
    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
    user.ResetToken = null; user.ResetExpires = null; _db.SaveChanges();
    TempData["Flash"] = "Password updated. Please log in.";
    return RedirectToPage("/Login");
  }
}
