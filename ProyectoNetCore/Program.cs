using Microsoft.EntityFrameworkCore;
using ProyectoNetCore.Data;
using ProyectoNetCore.Helpers;
using ProyectoNetCore.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});


builder.Services.AddSession();
builder.Services.AddAntiforgery();

string connectionString = builder.Configuration.GetConnectionString("SqlProyecto");
builder.Services.AddTransient<RepositoryAcciones>();
builder.Services.AddTransient<RepositoryUsuarios>();
builder.Services.AddTransient<RepositoryCompras>();
builder.Services.AddTransient <HelperAccion>();
builder.Services.AddTransient <HelperCryptography>();
builder.Services.AddTransient <HelperTools>();
builder.Services.AddTransient <HelperPathProvider>();
builder.Services.AddTransient <HelperJsonSession>();
builder.Services.AddTransient <HelperPathProvider>();
builder.Services.AddDbContext<AccionesContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<UsuariosContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ComprasContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
