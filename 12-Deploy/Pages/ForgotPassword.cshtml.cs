using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
public class ForgotPasswordModel : PageModel {
  private readonly AppDb _db;
  public ForgotPasswordModel(AppDb db) { _db = db; }
  public void OnGet() {}
  public IActionResult OnPost(string email) {
    var user = _db.Users.FirstOrDefault(u => u.Email == email.Trim().ToLowerInvariant());
    if (user != null) {
      user.ResetToken = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N")[..8];
      user.ResetExpires = DateTimeOffset.UtcNow.AddHours(1);
      _db.SaveChanges();
      Console.WriteLine("[mail] " + (Environment.GetEnvironmentVariable("APP_URL") ?? "http://127.0.0.1:5080") + "/ResetPassword?token=" + user.ResetToken);
    }
    TempData["Flash"] = "If that email exists, a reset link was sent.";
    return RedirectToPage("/Login");
  }
}
