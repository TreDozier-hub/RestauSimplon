using Swashbuckle.AspNetCore.Annotations;
using BreafComande.DbFolder;
using BreafComande.DToFolder;
using Microsoft.AspNetCore.Mvc;

namespace BreafComande.RouteFolder
{
    public static class ArticleRoutes
    {
        public static RouteGroupBuilder MapArticleRoutes(this RouteGroupBuilder article, GestionArticle gestionArticle)
        {
            article.MapGet("/", gestionArticle.GetAllArticles)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Récupère tous les articles",
                    description: "Renvoie une liste de tous les articles."))
                .WithMetadata(new SwaggerResponseAttribute(200, "Liste d'articles trouvée"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Aucun article trouvé"));

            article.MapGet("/{Id}", (int Id) => gestionArticle.GetArticle(Id))
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Récupère un article par ID",
                    description: "Renvoie un article spécifique en fonction de l'ID fourni dans la route."))
                .WithMetadata(new SwaggerResponseAttribute(200, "Article trouvé"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Article non trouvé"));

            article.MapPost("/", gestionArticle.CreateArticle)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Crée un nouvel article",
                    description: "Ajoute un nouvel article à la base de données."))
                .WithMetadata(new SwaggerResponseAttribute(201, "Article créé avec succès"));

            article.MapPut("/{Id}", (int Id, ArticleDTO articleDTO) => gestionArticle.UpdateArticle(Id, articleDTO))
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Met à jour un article existant",
                    description: "Met à jour les détails d'un article en fonction de l'ID fourni."))
                .WithMetadata(new SwaggerResponseAttribute(204, "Article mis à jour avec succès"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Article non trouvé"));

            article.MapDelete("/{Id}", (int Id) => gestionArticle.DeleteArticle(Id))
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Supprime un article",
                    description: "Supprime un article en fonction de l'ID fourni."))
                .WithMetadata(new SwaggerResponseAttribute(204, "Article supprimé avec succès"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Article non trouvé"));

            return article;
        }
    }
}
