using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
public class IndexModel : PageModel {
  private readonly AppDb _db;
  public IndexModel(AppDb db) { _db = db; }
  public List<Card> Posts { get; set; } = new();
  public PageInfo? Pagination { get; set; }
  public bool QDefined { get; set; }
  public string Q { get; set; } = "";
  public void OnGet(int page = 1, string? q = null) {
    var query = _db.Posts.Include(p => p.Author).OrderByDescending(p => p.CreatedAt).AsQueryable();


    var rows = query.Take(50).ToList();

    Posts = rows.Select(p => new Card {
      Id = p.Id,
      Title = p.Title,
      AuthorName = p.Author.DisplayName,
      Excerpt = p.Content.Length > 140 ? p.Content[..140] + "…" : p.Content
    }).ToList();
  }
  public record Card { public int? Id { get; set; } public string Title { get; set; } = ""; public string AuthorName { get; set; } = ""; public string Excerpt { get; set; } = ""; }
  public class PageInfo { public int Page { get; set; } public int Pages { get; set; } public int? Prev { get; set; } public int? Next { get; set; } }
}
