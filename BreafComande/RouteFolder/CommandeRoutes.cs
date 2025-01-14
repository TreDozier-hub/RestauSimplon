using BreafComande.ClassFolder;
using BreafComande.DbFolder;
using BreafComande.DToFolder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;

public static class CommandeRoutes
{
    public static RouteGroupBuilder MapCommandeRoutes(this RouteGroupBuilder group, GestionCommande gestionCommande)
    {
        group.MapGet("/", gestionCommande.GetAllCommandes)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: "Récupère toutes les commandes",
                description: "Renvoie une liste de toutes les commandes."))
            .WithMetadata(new SwaggerResponseAttribute(200, "Liste de commandes trouvée"))
            .WithMetadata(new SwaggerResponseAttribute(404, "Aucune commande trouvée"));

        group.MapGet("/{id}", gestionCommande.GetCommande)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: "Récupère une commande par ID",
                description: "Renvoie une commande spécifique en fonction de l'ID fourni dans la route."))
            .WithMetadata(new SwaggerResponseAttribute(200, "Commande trouvée"))
            .WithMetadata(new SwaggerResponseAttribute(404, "Commande non trouvée"));

        group.MapPost("/", gestionCommande.CreateCommande);
        group.MapPut("/{id}", gestionCommande.UpdateCommande);
        group.MapDelete("/{id}", gestionCommande.DeleteCommande);
        //group.MapGet("/search", gestionCommande.GetCommandesParClient);

        return group;
    }
}
