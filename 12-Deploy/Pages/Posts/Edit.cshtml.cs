using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace NorthlinePress.Pages.Posts;
public class EditModel : PageModel {
  private readonly AppDb _db;
  public EditModel(AppDb db) { _db = db; }
  public int Id { get; set; }
  public string TitleValue { get; set; } = "";
  public string ContentValue { get; set; } = "";
  public IActionResult OnGet(int id) {
    var post = _db.Posts.Find(id); if (post == null) return NotFound();
    if (User.Identity?.IsAuthenticated != true) return Forbid();
    if (int.Parse(User.FindFirst("uid")!.Value) != post.AuthorId) return Forbid();
    Id = post.Id; TitleValue = post.Title; ContentValue = post.Content; return Page();
  }
  public IActionResult OnPost(int id, string title, string content) {
    var post = _db.Posts.Find(id); if (post == null) return NotFound();
    if (User.Identity?.IsAuthenticated != true || int.Parse(User.FindFirst("uid")!.Value) != post.AuthorId) return Forbid();
    post.Title = title.Trim(); post.Content = content.Trim(); _db.SaveChanges();
    return RedirectToPage("/Posts/Details", new { id });
  }
}
