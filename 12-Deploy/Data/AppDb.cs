using Microsoft.EntityFrameworkCore;
public class AppDb : DbContext {
  public AppDb(DbContextOptions<AppDb> options) : base(options) {}
  public DbSet<User> Users => Set<User>();
  public DbSet<Post> Posts => Set<Post>();
}
public class User {
  public int Id { get; set; }
  public string Username { get; set; } = "";
  public string Email { get; set; } = "";
  public string DisplayName { get; set; } = "";
  public string? Bio { get; set; }
  public string? ImagePath { get; set; }
  public string PasswordHash { get; set; } = "placeholder";
  public string? ResetToken { get; set; }
  public DateTimeOffset? ResetExpires { get; set; }
  public List<Post> Posts { get; set; } = new();
}
public class Post {
  public int Id { get; set; }
  public string Title { get; set; } = "";
  public string Content { get; set; } = "";
  public int AuthorId { get; set; }
  public User Author { get; set; } = null!;
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
