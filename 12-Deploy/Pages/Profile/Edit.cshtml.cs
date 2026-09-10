using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace NorthlinePress.Pages.ProfileNS;
public class EditModel : PageModel {
  private readonly AppDb _db;
  private readonly IWebHostEnvironment _env;
  public EditModel(AppDb db, IWebHostEnvironment env) { _db = db; _env = env; }
  public User Profile { get; set; } = new();
  public IActionResult OnGet() {
    if (User.Identity?.IsAuthenticated != true) return RedirectToPage("/Login");
    Profile = _db.Users.Find(int.Parse(User.FindFirst("uid")!.Value))!;
    return Page();
  }
  public async Task<IActionResult> OnPostAsync(string displayName, string? bio, IFormFile? image) {
    if (User.Identity?.IsAuthenticated != true) return RedirectToPage("/Login");
    var me = _db.Users.Find(int.Parse(User.FindFirst("uid")!.Value))!;
    me.DisplayName = displayName.Trim(); me.Bio = bio?.Trim();
    if (image != null && image.Length > 0) {
      if (image.ContentType is not ("image/jpeg" or "image/png" or "image/webp")) return BadRequest("Unsupported image type");
      if (image.Length > 2 * 1024 * 1024) return BadRequest("Image too large");
      var ext = image.ContentType switch { "image/png" => ".png", "image/webp" => ".webp", _ => ".jpg" };
      var name = $"{me.Id}-{Guid.NewGuid():N}"[..24] + ext;
      var dir = Path.Combine(_env.ContentRootPath, "uploads"); Directory.CreateDirectory(dir);
      await using var fs = System.IO.File.Create(Path.Combine(dir, name));
      await image.CopyToAsync(fs);
      me.ImagePath = "/uploads/" + name;
    }
    _db.SaveChanges();
    return Redirect($"/Users?username={me.Username}");
  }
}
