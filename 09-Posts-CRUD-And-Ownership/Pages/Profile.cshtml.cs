using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
public class ProfileModel : PageModel {
  private readonly AppDb _db;
  public ProfileModel(AppDb db) { _db = db; }
  public User Profile { get; set; } = new();
  public List<Post> Posts { get; set; } = new();
  public bool IsSelf { get; set; }
  public IActionResult OnGet() {
    if (User.Identity?.IsAuthenticated != true) return RedirectToPage("/Login");
    var uid = int.Parse(User.FindFirst("uid")!.Value);
    return Redirect($"/Users?username={_db.Users.Find(uid)!.Username}");
  }
}
