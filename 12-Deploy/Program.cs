using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDb>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("Default") ??
    "Host=127.0.0.1;Port=5436;Database=northline_12;Username=northline;Password=northline"));
builder.Services.AddAuthentication("Cookies").AddCookie("Cookies", o => {
    o.LoginPath = "/Login"; o.AccessDeniedPath = "/Login";
    o.Cookie.HttpOnly = true; o.Cookie.SameSite = SameSiteMode.Lax;
    o.Cookie.SecurePolicy = builder.Configuration.GetValue("SESSION_HTTPS_ONLY", false) ? CookieSecurePolicy.Always : CookieSecurePolicy.SameAsRequest;
});
builder.Services.AddAuthorization();
var app = builder.Build();
using (var scope = app.Services.CreateScope()) {
    var dbx = scope.ServiceProvider.GetRequiredService<AppDb>();
    dbx.Database.EnsureCreated();
    Seed.Run(dbx, stageHasHash: true);
}
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
var uploads = Path.Combine(app.Environment.ContentRootPath, "uploads");
Directory.CreateDirectory(uploads);
app.UseStaticFiles(new StaticFileOptions { FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(uploads), RequestPath = "/uploads" });
app.MapRazorPages();
app.Urls.Add("http://0.0.0.0:5080");
app.Run();
