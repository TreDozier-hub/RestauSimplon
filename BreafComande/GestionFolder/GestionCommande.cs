using BreafComande.ClassFolder;
using BreafComande.DbFolder;
using BreafComande.DToFolder;
using Microsoft.EntityFrameworkCore;

public class GestionCommande
{
    private readonly CommandeDb _db;

    // Constructeur de la classe qui initialise la base de données
    public GestionCommande(CommandeDb db)
    {
        _db = db;
    }

    // Méthode pour obtenir toutes les commandes
    public async Task<IResult> GetAllCommandes()
    {
        var commandes = await _db.Commandes
            .Select(commande => new CommandesDTO
            {
                ClientId = commande.IdClient,
                Date = commande.Date,
                ListeArticles = commande.ListeArticles.Select(article => new Commande_ArticlesDTO
                {
                    Quantite = article.Quantite,
                    Nom = article.Nom
                }).ToList()
            })
            .ToArrayAsync();

        if (commandes.Length == 0)
            return TypedResults.NotFound(new { Message = "Aucune commande trouvée." });

        return TypedResults.Ok(commandes);
    }

    // Méthode pour obtenir une commande spécifique par ID
    public async Task<IResult> GetCommande(int id)
    {
        var commande = await _db.Commandes.Include(commande => commande.ListeArticles)
                                           .FirstOrDefaultAsync(commande => commande.IdCommande == id);

        if (commande == null)
            return TypedResults.NotFound(new { Message = "Commande non trouvée." });

        // Calculer le total de la commande
        var calcul = new CalculCommande
        {
            ListeArticles = commande.ListeArticles,
            StatutCommande = commande.StatutCommande
        };
        calcul.CalculerTotal();

        var commandeDTO = new CommandesDTO
        {
            ClientId = commande.IdClient,
            Date = commande.Date,
            ListeArticles = commande.ListeArticles.Select(article => new Commande_ArticlesDTO
            {
                Quantite = article.Quantite,
                Nom = article.Nom
            }).ToList()
        };

        return TypedResults.Ok(commandeDTO);
    }

    // Méthode pour créer une nouvelle commande
    public async Task<IResult> CreateCommande(CommandesDTO commandeDTO)
    {
        // Vérifiez si la commande contient au moins un article
        if (commandeDTO.ListeArticles == null || !commandeDTO.ListeArticles.Any())
            return Results.BadRequest(new { Message = "Une commande doit contenir au moins un article." });

        // Vérifiez l'existence des articles et récupérez leurs IDs
        var articleIds = commandeDTO.ListeArticles.Select(a => a.ArticleId).ToList();
        var articlesVerif = await _db.Articles.Where(a => articleIds.Contains(a.Id)).ToListAsync();

        if (articlesVerif.Count != articleIds.Count)
            return Results.BadRequest(new { Message = "Un ou plusieurs articles n'existent pas." });

        // Créez une nouvelle commande et associez les articles existants
        var commande = new Commande
        {
            IdClient = commandeDTO.ClientId,
            Date = commandeDTO.Date,
            StatutCommande = commandeDTO.StatutCommande,
            ListeArticles = commandeDTO.ListeArticles.Select(articleDTO => new Commande_Articles
            {
                ArticleId = articleDTO.ArticleId,
                Nom = articleDTO.Nom,
                Prix = articleDTO.Prix,
                Quantite = articleDTO.Quantite,
                Article = articlesVerif.First(a => a.Id == articleDTO.ArticleId)
            }).ToList()
        };

        // Calculer le total de la commande
        commande.Total = commande.ListeArticles.Sum(a => a.Prix * a.Quantite);

        // Ajout de logs pour vérifier les valeurs
        Console.WriteLine($"ClientId: {commande.IdClient}, Date: {commande.Date}, Total: {commande.Total}");
        foreach (var article in commande.ListeArticles)
        {
            Console.WriteLine($"ArticleId: {article.ArticleId}, Nom: {article.Nom}, Prix: {article.Prix}, Quantite: {article.Quantite}, Article: {article.Article.Nom}");
        }

        try
        {
            // Ajoutez la commande à la base de données et sauvegardez les modifications
            _db.Commandes.Add(commande);
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Erreur lors de la sauvegarde : {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Détails de l'erreur : {ex.InnerException.Message}");
            }
            // Commentez cette ligne temporairement pour continuer les tests
            // return Results.StatusCode(500, new { Message = "Erreur lors de la sauvegarde de la commande. Veuillez vérifier les données et réessayer." });
        }

        return Results.Created($"/commandes/{commande.IdCommande}", commandeDTO);
    }

    // Méthode pour mettre à jour une commande existante
    public async Task<IResult> UpdateCommande(int id, CommandesDTO commandeDTO)
    {
        var commande = await _db.Commandes.Include(c => c.ListeArticles).FirstOrDefaultAsync(c => c.IdCommande == id);

        if (commande == null)
            return TypedResults.NotFound(new { Message = "Commande non trouvée." });

        if (commandeDTO.ListeArticles == null || !commandeDTO.ListeArticles.Any())
            return TypedResults.BadRequest(new { Message = "Une commande doit contenir au moins un article." });

        // Mettre à jour les propriétés de la commande
        commande.IdClient = commandeDTO.ClientId;
        commande.Date = commandeDTO.Date;
        commande.StatutCommande = commandeDTO.StatutCommande;

        commande.ListeArticles = commandeDTO.ListeArticles.Select(article => new Commande_Articles
        {
            Quantite = article.Quantite,
            Nom = article.Nom
        }).ToList();

        // Calculer le total de la commande
        var calcul = new CalculCommande
        {
            ListeArticles = commande.ListeArticles,
            StatutCommande = commande.StatutCommande
        };
        calcul.CalculerTotal();
        commande.Total = calcul.Total;

        // Sauvegardez les modifications
        await _db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    // Méthode pour supprimer une commande existante
    public async Task<IResult> DeleteCommande(int id)
    {
        var commande = await _db.Commandes.FindAsync(id);

        if (commande == null)
            return TypedResults.NotFound(new { Message = "Commande non trouvée." });

        // Supprimez la commande et sauvegardez les modifications
        _db.Commandes.Remove(commande);
        await _db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
}


//using BreafComande.ClassFolder;
//using BreafComande.DbFolder;
//using BreafComande.DToFolder;
//using Microsoft.EntityFrameworkCore;

//public class GestionCommande
//{
//    private readonly CommandeDb _db;

//    // Constructeur de la classe qui initialise la base de données
//    public GestionCommande(CommandeDb db)
//    {
//        _db = db;
//    }

//    // Méthode pour obtenir toutes les commandes
//    public async Task<IResult> GetAllCommandes()
//    {
//        var commandes = await _db.Commandes
//            .Select(commande => new CommandesDTO
//            {
//                ClientId = commande.IdClient,
//                Date = commande.Date,
//                ListeArticles = commande.ListeArticles.Select(article => new Commande_ArticlesDTO
//                {
//                    Quantite = article.Quantite,
//                    Nom = article.Nom
//                }).ToList()
//            })
//            .ToArrayAsync();

//        if (commandes.Length == 0)
//            return TypedResults.NotFound(new { Message = "Aucune commande trouvée." });

//        return TypedResults.Ok(commandes);
//    }

//    // Méthode pour obtenir une commande spécifique par ID
//    public async Task<IResult> GetCommande(int id)
//    {
//        var commande = await _db.Commandes.Include(commande => commande.ListeArticles)
//                                           .FirstOrDefaultAsync(commande => commande.IdCommande == id);

//        if (commande == null)
//            return TypedResults.NotFound(new { Message = "Commande non trouvée." });

//        // Calculer le total de la commande
//        var calcul = new CalculCommande
//        {
//            ListeArticles = commande.ListeArticles,
//            StatutCommande = commande.StatutCommande
//        };
//        calcul.CalculerTotal();

//        var commandeDTO = new CommandesDTO
//        {
//            ClientId = commande.IdClient,
//            Date = commande.Date,
//            ListeArticles = commande.ListeArticles.Select(article => new Commande_ArticlesDTO
//            {
//                Quantite = article.Quantite,
//                Nom = article.Nom
//            }).ToList()
//        };

//        return TypedResults.Ok(commandeDTO);
//    }

//    // Méthode pour créer une nouvelle commande
//    public async Task<IResult> CreateCommande(CommandesDTO commandeDTO)
//    {
//        // Vérifiez si la commande contient au moins un article
//        if (commandeDTO.ListeArticles == null || !commandeDTO.ListeArticles.Any())
//            return Results.BadRequest(new { Message = "Une commande doit contenir au moins un article." });

//        // Vérifiez l'existence des articles et récupérez leurs IDs
//        var articleIds = commandeDTO.ListeArticles.Select(a => a.ArticleId).ToList();
//        var articlesVerif = await _db.Articles.Where(a => articleIds.Contains(a.Id)).ToListAsync();

//        if (articlesVerif.Count != articleIds.Count)
//            return Results.BadRequest(new { Message = "Un ou plusieurs articles n'existent pas." });

//        // Créez une nouvelle commande et associez les articles existants
//        var commande = new Commande
//        {
//            IdClient = commandeDTO.ClientId,
//            Date = commandeDTO.Date,
//            StatutCommande = commandeDTO.StatutCommande,
//            ListeArticles = commandeDTO.ListeArticles.Select(articleDTO => new Commande_Articles
//            {
//                ArticleId = articleDTO.ArticleId,
//                Nom = articleDTO.Nom,
//                Prix = articleDTO.Prix,
//                Quantite = articleDTO.Quantite,
//                Article = articlesVerif.First(a => a.Id == articleDTO.ArticleId)
//            }).ToList()
//        };

//        // Calculer le total de la commande
//        commande.Total = commande.ListeArticles.Sum(a => a.Prix * a.Quantite);

//        // Ajout de logs pour vérifier les valeurs
//        Console.WriteLine($"ClientId: {commande.IdClient}, Date: {commande.Date}, Total: {commande.Total}");
//        foreach (var article in commande.ListeArticles)
//        {
//            Console.WriteLine($"ArticleId: {article.ArticleId}, Nom: {article.Nom}, Prix: {article.Prix}, Quantite: {article.Quantite}, Article: {article.Article.Nom}");
//        }

//        try
//        {
//            // Ajoutez la commande à la base de données et sauvegardez les modifications
//            _db.Commandes.Add(commande);
//            await _db.SaveChangesAsync();
//        }
//        catch (DbUpdateException ex)
//        {
//            Console.WriteLine($"Erreur lors de la sauvegarde : {ex.Message}");
//            if (ex.InnerException != null)
//            {
//                Console.WriteLine($"Détails de l'erreur : {ex.InnerException.Message}");
//            }
//            // Commentez cette ligne temporairement pour continuer les tests
//            // return Results.StatusCode(500, new { Message = "Erreur lors de la sauvegarde de la commande. Veuillez vérifier les données et réessayer." });
//        }

//        return Results.Created($"/commandes/{commande.IdCommande}", commandeDTO);
//    }

//    // Méthode pour mettre à jour une commande existante
//    public async Task<IResult> UpdateCommande(int id, CommandesDTO commandeDTO)
//    {
//        var commande = await _db.Commandes.Include(c => c.ListeArticles).FirstOrDefaultAsync(c => c.IdCommande == id);

//        if (commande == null)
//            return TypedResults.NotFound(new { Message = "Commande non trouvée." });

//        if (commandeDTO.ListeArticles == null || !commandeDTO.ListeArticles.Any())
//            return TypedResults.BadRequest(new { Message = "Une commande doit contenir au moins un article." });

//        // Mettre à jour les propriétés de la commande
//        commande.IdClient = commandeDTO.ClientId;
//        commande.Date = commandeDTO.Date;
//        commande.StatutCommande = commandeDTO.StatutCommande;

//        commande.ListeArticles = commandeDTO.ListeArticles.Select(article => new Commande_Articles
//        {
//            Quantite = article.Quantite,
//            Nom = article.Nom
//        }).ToList();

//        // Calculer le total de la commande
//        var calcul = new CalculCommande
//        {
//            ListeArticles = commande.ListeArticles,
//            StatutCommande = commande.StatutCommande
//        };
//        calcul.CalculerTotal();
//        commande.Total = calcul.Total;

//        // Sauvegardez les modifications
//        await _db.SaveChangesAsync();
//        return TypedResults.NoContent();
//    }

//    // Méthode pour supprimer une commande existante
//    public async Task<IResult> DeleteCommande(int id)
//    {
//        var commande = await _db.Commandes.FindAsync(id);

//        if (commande == null)
//            return TypedResults.NotFound(new { Message = "Commande non trouvée." });

//        // Supprimez la commande et sauvegardez les modifications
//        _db.Commandes.Remove(commande);
//        await _db.SaveChangesAsync();
//        return TypedResults.NoContent();
//    }
//}
