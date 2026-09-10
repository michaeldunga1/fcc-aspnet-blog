var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("/", () => Results.Json(new { message = "Northline Press is live", port = 5080 }));
app.Urls.Add("http://0.0.0.0:5080");
app.Run();
