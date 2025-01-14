using Microsoft.EntityFrameworkCore;
using BreafComande.ClassFolder;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using BreafComande.DbFolder;
using BreafComande.RouteFolder;

var builder = WebApplication.CreateBuilder(args);

// Configurer DbContext avec SQLite
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

// Exécuter la logique de menu console dans un thread séparé pour éviter les conflits avec le serveur web
Task.Run(() =>
{
    while (true)
    {
        Console.WriteLine("Sélectionnez une action:");
        Console.WriteLine("1. Créer un client");
        Console.WriteLine("2. Créer une catégorie");
        Console.WriteLine("3. Créer un article");
        Console.WriteLine("4. Créer une commande");
        Console.WriteLine("5. Afficher toutes les commandes");
        Console.WriteLine("0. Quitter");

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                CreateClient().GetAwaiter().GetResult();
                break;
            case "2":
                CreateCategorie().GetAwaiter().GetResult();
                break;
            case "3":
                CreateArticle().GetAwaiter().GetResult();
                break;
            case "4":
                CreateCommande().GetAwaiter().GetResult();
                break;
            case "5":
                DisplayCommandes().GetAwaiter().GetResult();
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Choix invalide, veuillez réessayer.");
                break;
        }
    }
});

app.Run();

// Méthode pour créer un client
static async Task CreateClient()
{
    using var db = new CommandeDb(new DbContextOptionsBuilder<CommandeDb>().UseSqlite("Data Source=commandes.db").Options);

    Console.WriteLine("Entrez le prénom du client:");
    var prenom = Console.ReadLine();
    Console.WriteLine("Entrez le nom du client:");
    var nom = Console.ReadLine();
    Console.WriteLine("Entrez le téléphone du client:");
    var telephone = Console.ReadLine();
    Console.WriteLine("Entrez l'adresse du client:");
    var adresse = Console.ReadLine();

    var client = new Client { Prenom = prenom, Nom = nom, Telephone = telephone, Adresse = adresse };
    db.Clients.Add(client);
    await db.SaveChangesAsync();

    Console.WriteLine("Client créé avec succès.");
}

// Méthode pour créer une catégorie
static async Task CreateCategorie()
{
    using var db = new CommandeDb(new DbContextOptionsBuilder<CommandeDb>().UseSqlite("Data Source=commandes.db").Options);

    Console.WriteLine("Entrez le nom de la catégorie:");
    var nom = Console.ReadLine();

    var categorie = new Categorie { Nom = nom };
    db.Categories.Add(categorie);
    await db.SaveChangesAsync();

    Console.WriteLine("Catégorie créée avec succès.");
}

// Méthode pour créer un article
static async Task CreateArticle()
{
    using var db = new CommandeDb(new DbContextOptionsBuilder<CommandeDb>().UseSqlite("Data Source=commandes.db").Options);

    Console.WriteLine("Entrez le nom de l'article:");
    var nom = Console.ReadLine();
    Console.WriteLine("Entrez le prix de l'article:");
    var prix = decimal.Parse(Console.ReadLine());
    Console.WriteLine("Entrez l'ID de la catégorie de l'article:");
    var categorieId = int.Parse(Console.ReadLine());

    var article = new Article { Nom = nom, Prix = prix, CategorieId = categorieId };
    db.Articles.Add(article);
    await db.SaveChangesAsync();

    Console.WriteLine("Article créé avec succès.");
}

// Méthode pour créer une commande
static async Task CreateCommande()
{
    using var db = new CommandeDb(new DbContextOptionsBuilder<CommandeDb>().UseSqlite("Data Source=commandes.db").Options);

    Console.WriteLine("Entrez l'ID du client:");
    var clientId = int.Parse(Console.ReadLine());
    Console.WriteLine("Entrez la date de la commande (format YYYY-MM-DD):");
    var date = DateTime.Parse(Console.ReadLine());
    Console.WriteLine("Entrez le statut de la commande (true/false):");
    var statut = bool.Parse(Console.ReadLine());

    var commande = new Commande
    {
        IdClient = clientId,
        Date = date,
        StatutCommande = statut
    };

    while (true)
    {
        Console.WriteLine("Entrez l'ID de l'article (ou '0' pour terminer):");
        var articleId = int.Parse(Console.ReadLine());
        if (articleId == 0) break;

        Console.WriteLine("Entrez la quantité:");
        var quantite = int.Parse(Console.ReadLine());

        var article = await db.Articles.FindAsync(articleId);
        if (article != null)
        {
            commande.ListeArticles.Add(new Commande_Articles
            {
                ArticleId = article.Id,
                Nom = article.Nom,
                Prix = article.Prix,
                Quantite = quantite,
                Article = article
            });
        }
        else
        {
            Console.WriteLine("Article non trouvé.");
        }
    }

    commande.Total = commande.ListeArticles.Sum(a => a.Prix * a.Quantite);
    db.Commandes.Add(commande);
    await db.SaveChangesAsync();

    Console.WriteLine("Commande créée avec succès.");
}

// Méthode pour afficher toutes les commandes
static async Task DisplayCommandes()
{
    using var db = new CommandeDb(new DbContextOptionsBuilder<CommandeDb>().UseSqlite("Data Source=commandes.db").Options);

    var commandes = await db.Commandes.Include(c => c.ListeArticles).ThenInclude(ca => ca.Article).ToListAsync();
    foreach (var cmd in commandes)
    {
        Console.WriteLine($"Commande Id: {cmd.IdCommande}, Client Id: {cmd.IdClient}, Total: {cmd.Total}");
        foreach (var ca in cmd.ListeArticles)
        {
            Console.WriteLine($" - Article: {ca.Nom}, Quantité: {ca.Quantite}, Prix: {ca.Prix}");
        }
    }
}



//using Microsoft.EntityFrameworkCore;
//using BreafComande.ClassFolder;
//using Microsoft.OpenApi.Models;
//using Swashbuckle.AspNetCore.Annotations;
//using BreafComande.DbFolder;
//using BreafComande.RouteFolder;

//var builder = WebApplication.CreateBuilder(args);

//// Configurer DbContext avec SQLite
//builder.Services.AddDbContext<CommandeDb>(options =>
//    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
////builder.Services.AddDbContext<CommandeDb>(options =>
////    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
////builder.Services.AddDbContext<CommandeDb>(options =>
////    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "API Gestion",
//        Version = "v1",
//        Description = "Une API pour gérer les commandes et les articles",
//        Contact = new OpenApiContact
//        {
//            Name = "EmZaAr",
//            Email = "tonemail@exemple.com",
//            Url = new Uri("https://simplon.fr"),
//        }
//    });
//    c.EnableAnnotations();
//});

//var app = builder.Build();

////using (var scope = app.Services.CreateScope())
////{
////    var services = scope.ServiceProvider;
////    var context = services.GetRequiredService<CommandeDb>();
////    context.Database.Migrate(); // Applique les migrations
////    ImportJson.SeedData(context); // Appel à la méthode ImportJson
////}


//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gestion V1");
//        c.RoutePrefix = string.Empty;
//    });
//}

//var gestionArticle = new GestionArticle(new CommandeDb(builder.Services.BuildServiceProvider().GetRequiredService<DbContextOptions<CommandeDb>>()));
//var articles = app.MapGroup("/articles").MapArticleRoutes(gestionArticle);

//var gestionCommande = new GestionCommande(new CommandeDb(builder.Services.BuildServiceProvider().GetRequiredService<DbContextOptions<CommandeDb>>()));
//var commandes = app.MapGroup("/commandes").MapCommandeRoutes(gestionCommande);

//var gestionClient = new GestionClient(new CommandeDb(builder.Services.BuildServiceProvider().GetRequiredService<DbContextOptions<CommandeDb>>()));
//var clients = app.MapGroup("/clients").MapClientRoutes(gestionClient);

//var gestionCategorie = new GestionCategorie(new CommandeDb(builder.Services.BuildServiceProvider().GetRequiredService<DbContextOptions<CommandeDb>>()));
//var categories = app.MapGroup("/categories").MapCategorieRoutes(gestionCategorie);

//app.Run();


//using Microsoft.EntityFrameworkCore;
//using BreafComande.ClassFolder;
//using Microsoft.OpenApi.Models;
//using Swashbuckle.AspNetCore.Annotations;
//using BreafComande.DbFolder;
//using BreafComande.RouteFolder;

//var builder = WebApplication.CreateBuilder(args);

//// Configuration de DbContext avec SQLite
//builder.Services.AddDbContext<CommandeDb>(options =>
//    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "API Gestion",
//        Version = "v1",
//        Description = "Une API pour gérer les commandes et les articles",
//        Contact = new OpenApiContact
//        {
//            Name = "EmZaAr",
//            Email = "tonemail@exemple.com",
//            Url = new Uri("https://simplon.fr"),
//        }
//    });
//    c.EnableAnnotations();
//});

//var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gestion V1");
//        c.RoutePrefix = string.Empty;
//    });
//}

//// Initialiser les gestionnaires pour les routes
//var gestionArticle = new GestionArticle(new CommandeDb(builder.Services.BuildServiceProvider().GetRequiredService<DbContextOptions<CommandeDb>>()));
//var articles = app.MapGroup("/articles").MapArticleRoutes(gestionArticle);

//var gestionCommande = new GestionCommande(new CommandeDb(builder.Services.BuildServiceProvider().GetRequiredService<DbContextOptions<CommandeDb>>()));
//var commandes = app.MapGroup("/commandes").MapCommandeRoutes(gestionCommande);

//var gestionClient = new GestionClient(new CommandeDb(builder.Services.BuildServiceProvider().GetRequiredService<DbContextOptions<CommandeDb>>()));
//var clients = app.MapGroup("/clients").MapClientRoutes(gestionClient);

//var gestionCategorie = new GestionCategorie(new CommandeDb(builder.Services.BuildServiceProvider().GetRequiredService<DbContextOptions<CommandeDb>>()));
//var categories = app.MapGroup("/categories").MapCategorieRoutes(gestionCategorie);

//app.Run();
