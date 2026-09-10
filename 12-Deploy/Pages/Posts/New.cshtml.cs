using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace NorthlinePress.Pages.Posts;
public class NewModel : PageModel {
  private readonly AppDb _db;
  public NewModel(AppDb db) { _db = db; }
  public string? Error { get; set; }
  public string TitleValue { get; set; } = "";
  public string ContentValue { get; set; } = "";
  public IActionResult OnGet() => User.Identity?.IsAuthenticated == true ? Page() : RedirectToPage("/Login");
  public IActionResult OnPost(string title, string content) {
    if (User.Identity?.IsAuthenticated != true) return RedirectToPage("/Login");
    if (title.Trim().Length < 3) { Error = "Title too short"; TitleValue = title; ContentValue = content; return Page(); }
    var post = new Post { Title = title.Trim(), Content = content.Trim(), AuthorId = int.Parse(User.FindFirst("uid")!.Value) };
    _db.Posts.Add(post); _db.SaveChanges();
    return RedirectToPage("/Posts/Details", new { id = post.Id });
  }
}
