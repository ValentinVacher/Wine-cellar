using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.Swagger;
using System.ComponentModel;
using System.Security.Claims;
using System.Text.Json.Serialization;
using Wine_celar.Repositories;
using Wine_cellar.Contexts;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using Wine_cellar.Repositories;

var builder = WebApplication.CreateBuilder(args);

//Définitiion du CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
                      builder =>
                      {
                          builder.WithOrigins("*");
                          
                      });
});

//Ajout de l'authentification
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();


//Permet d'ignorer les cycles
builder.Services.AddControllers().AddJsonOptions(options => 
options.JsonSerializerOptions.ReferenceHandler =
ReferenceHandler.IgnoreCycles); 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//Donne accées au fichier de commentaires
builder.Services.AddSwaggerGen(options => {
    options.IncludeXmlComments("bin\\Debug\\netcoreapp2.0\\SwaggerDemo.xml", true);
});

//Défini la connexion en base de donnée
builder.Services.AddDbContext<WineContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("WineCellarDbCS"));
    
});

builder.Services.AddScoped<IWineRepository, WineRepository>();
builder.Services.AddScoped<ICellarRepository, CellarRepository>();
builder.Services.AddScoped<IDrawerRepository, DrawerRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAppelationRepository, AppelationRepository>();



var app = builder.Build();

//Forcer les migrations en attentes (évite de faire le update-database)
//using (var serviceScope = app.Services.CreateScope())
//{
//    var services = serviceScope.ServiceProvider;
//    var wikyContext = services.GetRequiredService<WineContext>();
//    //wikyContext.Database.Migrate();

//    wikyContext.Database.EnsureDeleted();
//    wikyContext.Database.EnsureCreated();
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();