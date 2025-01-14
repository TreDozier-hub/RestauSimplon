using BreafComande.ClassFolder;
using BreafComande.DbFolder;
using BreafComande.DToFolder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;

public static class ClientRoutes
{
    public static RouteGroupBuilder MapClientRoutes(this RouteGroupBuilder group, GestionClient gestionClient)
    {
        group.MapPost("/", gestionClient.AddClient)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: "Ajouter un client",
                description: "Crée un nouveau client."))
            .WithMetadata(new SwaggerResponseAttribute(201, "Client créé"))
            .WithMetadata(new SwaggerResponseAttribute(400, "Requête invalide"));

        group.MapDelete("/{id}", gestionClient.DeleteClient)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: "Supprimer un client",
                description: "Supprime un client existant en fonction de l'ID fourni."))
            .WithMetadata(new SwaggerResponseAttribute(204, "Client supprimé"))
            .WithMetadata(new SwaggerResponseAttribute(404, "Client non trouvé"));

        group.MapGet("/{Prenom}", gestionClient.GetClient)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: "Récupérer un client par prénom",
                description: "Renvoie un client spécifique en fonction du prénom fourni."))
            .WithMetadata(new SwaggerResponseAttribute(200, "Client trouvé"))
            .WithMetadata(new SwaggerResponseAttribute(404, "Client non trouvé"));

        group.MapPut("/{id}", gestionClient.UpdateClient)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: "Mettre à jour un client",
                description: "Met à jour les informations d'un client existant en fonction de l'ID fourni."))
            .WithMetadata(new SwaggerResponseAttribute(200, "Client mis à jour"))
            .WithMetadata(new SwaggerResponseAttribute(404, "Client non trouvé"));

        return group;
    }
}
