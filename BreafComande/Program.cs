using Microsoft.EntityFrameworkCore;
using BreafComande.ClassFolder;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using BreafComande.DbFolder;
using BreafComande.RouteFolder;

var builder = WebApplication.CreateBuilder(args);

// Configuration de DbContext avec SQLite
builder.Services.AddDbContext<CommandeDb>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Gestion",
        Version = "v1",
        Description = "Une API pour gérer les commandes et les articles",
        Contact = new OpenApiContact
        {
            Name = "EmZaAr",
            Email = "tonemail@exemple.com",
            Url = new Uri("https://simplon.fr"),
        }
    });
    c.EnableAnnotations();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gestion V1");
        c.RoutePrefix = string.Empty;
    });
}

// Initialiser les gestionnaires pour les routes
var gestionArticle = new GestionArticle(new CommandeDb(builder.Services.BuildServiceProvider().GetRequiredService<DbContextOptions<CommandeDb>>()));
var articles = app.MapGroup("/articles").MapArticleRoutes(gestionArticle);

var gestionCommande = new GestionCommande(new CommandeDb(builder.Services.BuildServiceProvider().GetRequiredService<DbContextOptions<CommandeDb>>()));
var commandes = app.MapGroup("/commandes").MapCommandeRoutes(gestionCommande);

var gestionClient = new GestionClient(new CommandeDb(builder.Services.BuildServiceProvider().GetRequiredService<DbContextOptions<CommandeDb>>()));
var clients = app.MapGroup("/clients").MapClientRoutes(gestionClient);

var gestionCategorie = new GestionCategorie(new CommandeDb(builder.Services.BuildServiceProvider().GetRequiredService<DbContextOptions<CommandeDb>>()));
var categories = app.MapGroup("/categories").MapCategorieRoutes(gestionCategorie);

app.Run();
