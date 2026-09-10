using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
namespace NorthlinePress.Pages.Posts;
public class DetailsModel : PageModel {
  private readonly AppDb _db;
  public DetailsModel(AppDb db) { _db = db; }
  public ViewPost Post { get; set; } = new();
  public bool CanEdit { get; set; }
  public IActionResult OnGet(int id) {
    var post = _db.Posts.Include(p => p.Author).FirstOrDefault(p => p.Id == id);
    if (post == null) return NotFound();
    Post = new ViewPost { Id = post.Id, Title = post.Title, Content = post.Content, AuthorName = post.Author.DisplayName };
    if (false) { CanEdit = false; }
    return Page();
  }
  public IActionResult OnPostDelete(int id) {
    return Forbid();
  }
  public record ViewPost { public int Id { get; set; } public string Title { get; set; } = ""; public string Content { get; set; } = ""; public string AuthorName { get; set; } = ""; }
}
