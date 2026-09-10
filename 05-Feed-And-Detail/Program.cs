using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDb>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("Default") ??
    "Host=127.0.0.1;Port=5436;Database=northline_05;Username=northline;Password=northline"));
var app = builder.Build();
using (var scope = app.Services.CreateScope()) {
    var dbx = scope.ServiceProvider.GetRequiredService<AppDb>();
    dbx.Database.EnsureCreated();
    Seed.Run(dbx);
}
app.UseStaticFiles();
app.MapRazorPages();
app.Urls.Add("http://0.0.0.0:5080");
app.Run();
