var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("/", () => Results.Text("Northline Press home"));
app.MapGet("/about", () => Results.Text("About Northline Press"));
app.MapGet("/health", () => Results.Json(new { ok = true }));
app.Urls.Add("http://0.0.0.0:5080");
app.Run();
