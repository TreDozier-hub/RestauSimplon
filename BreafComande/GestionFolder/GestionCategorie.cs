using BreafComande.ClassFolder;
using BreafComande.DbFolder;
using BreafComande.DToFolder;
using Microsoft.EntityFrameworkCore;

public class GestionCategorie
{
    private readonly CommandeDb _context;

    public GestionCategorie(CommandeDb context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CategorieDTO>> GetAllCategories()
    {
        return await _context.Categories
            .Select(categorie => new CategorieDTO
            {
                //CategorieId = categorie.CategorieId,
                Nom = categorie.Nom
            }).ToListAsync();
    }

    public async Task<CategorieDTO> GetCategorie(int id)
    {
        var categorie = await _context.Categories.FindAsync(id);

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
            return null; // ou retournez une réponse d'erreur appropriée

        var categorie = new Categorie
        {
            Nom = categorieDTO.Nom
        };

        _context.Categories.Add(categorie);
        await _context.SaveChangesAsync();

        // Mise à jour du DTO avec l'ID généré
        categorieDTO.CategorieId = categorie.CategorieId;

        return categorieDTO;
    }


    public async Task<bool> UpdateCategorie(int id, CategorieDTO categorieDTO)
    {
        var categorie = await _context.Categories.FindAsync(id);
        if (categorie == null)
        {
            return false;
        }

        categorie.Nom = categorieDTO.Nom;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Categories.Any(categorie => categorie.CategorieId == id))
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
        var categorie = await _context.Categories.FindAsync(id);
        if (categorie == null)
        {
            return false;
        }

        _context.Categories.Remove(categorie);
        await _context.SaveChangesAsync();

        return true;
    }
}
