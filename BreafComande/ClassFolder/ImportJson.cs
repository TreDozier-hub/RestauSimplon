using System.Text.Json;
using BreafComande.ClassFolder;
using BreafComande.DbFolder;

public static class ImportJson
{
    public static void SeedData(CommandeDb context)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Lire et désérialiser les clients
        var clientsJson = File.ReadAllText("clients.json");
        var clients = JsonSerializer.Deserialize<List<Client>>(clientsJson, options);
        context.Clients.AddRange(clients);

        // Lire et désérialiser les catégories
        var categoriesJson = File.ReadAllText("categories.json");
        var categories = JsonSerializer.Deserialize<List<Categorie>>(categoriesJson, options);
        context.Categories.AddRange(categories);

        // Lire et désérialiser les articles
        var articlesJson = File.ReadAllText("articles.json");
        var articles = JsonSerializer.Deserialize<List<Article>>(articlesJson, options);
        context.Articles.AddRange(articles);

        // Lire et désérialiser les commandes
        var commandesJson = File.ReadAllText("commandes.json");
        var commandes = JsonSerializer.Deserialize<List<Commande>>(commandesJson, options);
        context.Commandes.AddRange(commandes);

        context.SaveChanges();
    }
}



//using BreafComande.ClassFolder;
//using BreafComande.DbFolder;
//using Newtonsoft.Json;
//using System.IO;
//using System.Collections.Generic;
//using System.Linq;

//public static class ImportJson
//{
//    public static void SeedData(CommandeDb context)
//    {
//        if (!context.Articles.Any())
//        {
//            try
//            {
//                var jsonData = File.ReadAllText("Articles_Menu.json");
//                var articles = JsonConvert.DeserializeObject<List<Article>>(jsonData);

//                foreach (var article in articles)
//                {
//                    var categorie = context.Categories.SingleOrDefault(c => c.Nom == article.Categorie.Nom);
//                    if (categorie == null)
//                    {
//                        categorie = new Categorie { Nom = article.Categorie.Nom };
//                        context.Categories.Add(categorie);
//                    }

//                    article.CategorieId = categorie.CategorieId; // Assignez l'ID de la catégorie
//                    article.Categorie = categorie; // Associez la catégorie
//                    context.Articles.Add(article);
//                }
//                context.SaveChanges();
//            }
//            catch (FileNotFoundException ex)
//            {
//                Console.WriteLine($"Le fichier spécifié est introuvable: {ex.Message}");
//            }
//            catch (JsonException ex)
//            {
//                Console.WriteLine($"Erreur lors de la désérialisation du fichier JSON: {ex.Message}");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Une erreur s'est produite: {ex.Message}");
//            }
//        }
//    }
//}
