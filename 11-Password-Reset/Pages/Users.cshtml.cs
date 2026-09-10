using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
public class UsersModel : PageModel {
  private readonly AppDb _db;
  public UsersModel(AppDb db) { _db = db; }
  public User Profile { get; set; } = new();
  public List<Post> Posts { get; set; } = new();
  public bool IsSelf { get; set; }
  public IActionResult OnGet(string username) {
    var profile = _db.Users.FirstOrDefault(u => u.Username == username);
    if (profile == null) return NotFound();
    Profile = profile;
    Posts = _db.Posts.Where(p => p.AuthorId == profile.Id).OrderByDescending(p => p.CreatedAt).ToList();
    if (User.Identity?.IsAuthenticated == true) IsSelf = int.Parse(User.FindFirst("uid")!.Value) == profile.Id;
    return Page();
  }
}
