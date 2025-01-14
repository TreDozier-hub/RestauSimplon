using BreafComande.ClassFolder;
using BreafComande.DbFolder;
using BreafComande.DToFolder;
using Microsoft.EntityFrameworkCore;

public class GestionClient
{
    private readonly CommandeDb _db;

    public GestionClient(CommandeDb db)
    {
        _db = db;
    }

    public async Task<IResult> AddClient(ClientDTO clientDTO)
    {
        var client = new Client
        {
            Prenom = clientDTO.Prenom,
            Nom = clientDTO.Nom,
            Telephone = clientDTO.Telephone,
            Adresse = clientDTO.Adresse
        };

        _db.Clients.Add(client);
        await _db.SaveChangesAsync();

        return TypedResults.Created($"/clients/{client.Id}", client);
    }

    public async Task<IResult> DeleteClient(int id)
    {
        var client = await _db.Clients.FindAsync(id);

        if (client == null)
            return TypedResults.NotFound();

        _db.Clients.Remove(client);
        await _db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    public async Task<IResult> GetClient(string prenom, int id)
    {
        var client = await _db.Clients
            .FirstOrDefaultAsync(c => c.Prenom == prenom || c.Id == id);

        if (client == null)
            return TypedResults.NotFound();

        return TypedResults.Ok(client);
    }

    public async Task<IResult> UpdateClient(int id, ClientDTO clientDTO)
    {
        var client = await _db.Clients.FindAsync(id);

        if (client == null)
            return TypedResults.NotFound();

        client.Prenom = clientDTO.Prenom;
        client.Nom = clientDTO.Nom;
        client.Telephone = clientDTO.Telephone;
        client.Adresse = clientDTO.Adresse;

        await _db.SaveChangesAsync();
        return TypedResults.Ok(client);
    }
}
