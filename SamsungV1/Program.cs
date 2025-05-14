using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SamsungV1.Data;
using Sistema.AccesoDatos.Data.Repository.IRepository;
using Sistema.AccesoDatos.Data.Repository;
using Sistema.Models;
using Sistema.Utilidades;
using Resend;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("ConexionSQL") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();
builder.Services.AddControllersWithViews();



//agreagar contenedor trabajo

builder.Services.AddScoped<IContenedorTrabajo, ContenedorTrabajo>();


builder.Services.AddOptions();
builder.Services.AddHttpClient<ResendClient>();
builder.Services.Configure<ResendClientOptions>(o =>
{
    o.ApiToken = "re_i5XGCMgo_K4KVc3T3H9f4ZRxbbjQtHGXE";
});
builder.Services.AddTransient<IResend, ResendClient>();

var app = builder.Build();

//superuser
using (var scope = app.Services.CreateScope())
{
    await IdentidadInicializadora.CrearRolesYSuperUsuarioAsync(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Inicio}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

string wwwroot = app.Environment.WebRootPath;
Rotativa.AspNetCore.RotativaConfiguration.Setup(wwwroot, "rotativa");



app.Run();
