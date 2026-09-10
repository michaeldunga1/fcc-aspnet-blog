public static class Seed {
  public static void Run(AppDb db, bool stageHasHash = false) {
    if (db.Users.Any()) return;
    var hash = BCrypt.Net.BCrypt.HashPassword("password123");
    if (!stageHasHash) hash = "placeholder";
    var ada = new User { Username = "ada", Email = "ada@example.com", DisplayName = "Ada Lovelace", PasswordHash = hash };
    var grace = new User { Username = "grace", Email = "grace@example.com", DisplayName = "Grace Hopper", PasswordHash = hash };
    db.Users.AddRange(ada, grace);
    db.SaveChanges();
    db.Posts.AddRange(
      new Post { Title = "Hello from the newsroom", Content = "First post from the teaching seed data.", AuthorId = ada.Id },
      new Post { Title = "Notes on ownership", Content = "Only the author should edit or delete this post.", AuthorId = ada.Id },
      new Post { Title = "Second author voice", Content = "Grace owns this post; Ada should get 403 on mutate.", AuthorId = grace.Id }
    );
    for (var i = 4; i < 12; i++) {
      db.Posts.Add(new Post { Title = $"Seed story {i}", Content = $"Extra seed content {i} for pagination.", AuthorId = i % 2 == 0 ? ada.Id : grace.Id });
    }
    db.SaveChanges();
  }
}
