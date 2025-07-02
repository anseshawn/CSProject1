using Project1.Contexts.DbConnection;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

ConfigurationManager configuration = builder.Configuration;

AspClassDbConnection.SetConnectionString(configuration);

builder.Services.AddRazorPages();
builder.Services.AddMvc();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

var app = builder.Build();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.MapGet("/", () => "Hello World!");
app.MapControllerRoute(
    name: "default",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);
app.UseSession();
app.MapRazorPages();
app.Run();
