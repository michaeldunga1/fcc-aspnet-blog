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

    if (!string.IsNullOrWhiteSpace(q)) {
      var like = q.Trim().ToLowerInvariant();
      query = query.Where(p => p.Title.ToLower().Contains(like) || p.Content.ToLower().Contains(like));
    }


    var total = query.Count();
    var pages = Math.Max(1, (int)Math.Ceiling(total / (double)5));
    page = Math.Clamp(page, 1, pages);
    var rows = query.Skip((page - 1) * 5).Take(5).ToList();
    Pagination = new PageInfo { Page = page, Pages = pages, Prev = page > 1 ? page - 1 : null, Next = page < pages ? page + 1 : null };
    QDefined = true; Q = q ?? "";

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
