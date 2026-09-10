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
    if (User.Identity?.IsAuthenticated == true) { var uid = int.Parse(User.FindFirst("uid")!.Value); CanEdit = uid == post.AuthorId; }
    return Page();
  }
  public IActionResult OnPostDelete(int id) {
    var post = _db.Posts.Find(id); if (post == null) return NotFound(); if (User.Identity?.IsAuthenticated != true) return Forbid(); var uid = int.Parse(User.FindFirst("uid")!.Value); if (uid != post.AuthorId) return Forbid(); _db.Posts.Remove(post); _db.SaveChanges(); return RedirectToPage("/Index");
  }
  public record ViewPost { public int Id { get; set; } public string Title { get; set; } = ""; public string Content { get; set; } = ""; public string AuthorName { get; set; } = ""; }
}
