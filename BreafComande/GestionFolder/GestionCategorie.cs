using BreafComande.ClassFolder;
using BreafComande.DbFolder;
using BreafComande.DToFolder;
using Microsoft.EntityFrameworkCore;

public class GestionCategorie
{
    private readonly CommandeDb _db;

    public GestionCategorie(CommandeDb db)
    {
        _db = db;
    }

    public async Task<IEnumerable<CategorieDTO>> GetAllCategories()
    {
        return await _db.Categories
            .Select(categorie => new CategorieDTO
            {
                //CategorieId = categorie.CategorieId,
                Nom = categorie.Nom
            }).ToListAsync();
    }

    public async Task<CategorieDTO> GetCategorie(int id)
    {
        var categorie = await _db.Categories.FindAsync(id);

        if (categorie == null)
        {
            return null;
        }

        return new CategorieDTO
        {
            //CategorieId = categorie.CategorieId,
            Nom = categorie.Nom
        };
    }

    public async Task<CategorieDTO> CreateCategorie(CategorieDTO categorieDTO)
    {
        if (string.IsNullOrWhiteSpace(categorieDTO.Nom))
            return null;

        var categorie = new Categorie
        {
            Nom = categorieDTO.Nom
        };

        _db.Categories.Add(categorie);
        await _db.SaveChangesAsync();

        // Mise à jour du DTO avec l'ID généré
        categorieDTO.CategorieId = categorie.CategorieId;

        return categorieDTO;
    }


    public async Task<bool> UpdateCategorie(int id, CategorieDTO categorieDTO)
    {
        var categorie = await _db.Categories.FindAsync(id);
        if (categorie == null)
        {
            return false;
        }

        categorie.Nom = categorieDTO.Nom;

        try
        {
            await _db.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_db.Categories.Any(categorie => categorie.CategorieId == id))
            {
                return false;
            }
            else
            {
                throw;
            }
        }
    }

    public async Task<bool> DeleteCategorie(int id)
    {
        var categorie = await _db.Categories.FindAsync(id);
        if (categorie == null)
        {
            return false;
        }

        _db.Categories.Remove(categorie);
        await _db.SaveChangesAsync();

        return true;
    }
}
