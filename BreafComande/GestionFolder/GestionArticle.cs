using BreafComande.ClassFolder;
using BreafComande.DbFolder;
using BreafComande.DToFolder;
using Microsoft.EntityFrameworkCore;

public class GestionArticle
{
    private readonly CommandeDb _db;

    public GestionArticle(CommandeDb db)
    {
        _db = db;
    }

    public async Task<IResult> GetAllArticles()
    {
        var articles = await _db.Articles
            .Select(article => new ArticleDTO
            {
                //Id = article.Id,
                Nom = article.Nom,
                Prix = article.Prix,
                CategorieId = article.CategorieId,
            })
            .ToArrayAsync();

        if (articles.Length == 0)
            return TypedResults.NotFound(new { Message = "Aucun article trouvé." });

        return TypedResults.Ok(articles);
    }

    //méthode GetArticle
    public async Task<IResult> GetArticle(int id)
    {
        var article = await _db.Articles.FindAsync(id);

        if (article == null)
            return TypedResults.NotFound(new { Message = "Article non trouvé." });

        var articleDTO = new ArticleDTO
        {
            //Id = article.Id,
            Nom = article.Nom,
            Prix = article.Prix,
            CategorieId = article.CategorieId,
        };

        return TypedResults.Ok(articleDTO);
    }

    public async Task<IResult> CreateArticle(ArticleDTO articleDTO)
    {
        if (string.IsNullOrWhiteSpace(articleDTO.Nom))
            return TypedResults.BadRequest(new { Message = "Le nom de l'article est requis." });

        articleDTO.Prix = Math.Round(articleDTO.Prix, 2);

        var article = new Article
        {
            //Id = articleDTO.Id,
            Nom = articleDTO.Nom,
            Prix = articleDTO.Prix,
            CategorieId = articleDTO.CategorieId,
        };

        _db.Articles.Add(article);
        await _db.SaveChangesAsync();

        //articleDTO.Id = article.Id;
        return TypedResults.Created($"/articles/{article.Id}", articleDTO);
    }

    public async Task<IResult> UpdateArticle(int id, ArticleDTO articleDTO)
    {
        var article = await _db.Articles.FindAsync(id);

        if (article == null)
            return TypedResults.NotFound(new { Message = "Article non trouvé." });

        articleDTO.Prix = Math.Round(articleDTO.Prix, 2);

        article.Nom = articleDTO.Nom;
        article.Prix = articleDTO.Prix;
        article.CategorieId = articleDTO.CategorieId;

        await _db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    public async Task<IResult> DeleteArticle(int id)
    {
        var article = await _db.Articles.FindAsync(id);

        if (article == null)
            return TypedResults.NotFound(new { Message = "Article non trouvé." });

        _db.Articles.Remove(article);
        await _db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
}
