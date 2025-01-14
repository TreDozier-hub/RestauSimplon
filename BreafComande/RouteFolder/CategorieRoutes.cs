using Swashbuckle.AspNetCore.Annotations;
using BreafComande.DbFolder;
using BreafComande.DToFolder;
using Microsoft.AspNetCore.Mvc;

namespace BreafComande.RouteFolder
{
    public static class CategorieRoutes
    {
        public static RouteGroupBuilder MapCategorieRoutes(this RouteGroupBuilder categorie, GestionCategorie gestionCategorie)
        {
            categorie.MapGet("/", gestionCategorie.GetAllCategories)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Récupère toutes les catégories",
                    description: "Renvoie une liste de toutes les catégories."))
                .WithMetadata(new SwaggerResponseAttribute(200, "Liste de catégories trouvée"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Aucune catégorie trouvée"));

            categorie.MapGet("/{Id}", (int Id) => gestionCategorie.GetCategorie(Id))
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Récupère une catégorie par ID",
                    description: "Renvoie une catégorie spécifique en fonction de l'ID fourni dans la route."))
                .WithMetadata(new SwaggerResponseAttribute(200, "Catégorie trouvée"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Catégorie non trouvée"));

            categorie.MapPost("/", gestionCategorie.CreateCategorie)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Crée une nouvelle catégorie",
                    description: "Ajoute une nouvelle catégorie à la base de données."))
                .WithMetadata(new SwaggerResponseAttribute(201, "Catégorie créée avec succès"));

            categorie.MapPut("/{Id}", (int Id, CategorieDTO categorieDTO) => gestionCategorie.UpdateCategorie(Id, categorieDTO))
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Met à jour une catégorie existante",
                    description: "Met à jour les détails d'une catégorie en fonction de l'ID fourni."))
                .WithMetadata(new SwaggerResponseAttribute(204, "Catégorie mise à jour avec succès"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Catégorie non trouvée"));

            categorie.MapDelete("/{Id}", (int Id) => gestionCategorie.DeleteCategorie(Id))
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Supprime une catégorie",
                    description: "Supprime une catégorie en fonction de l'ID fourni."))
                .WithMetadata(new SwaggerResponseAttribute(204, "Catégorie supprimée avec succès"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Catégorie non trouvée"));

            return categorie;
        }
    }
}
