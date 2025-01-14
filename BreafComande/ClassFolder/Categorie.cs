using System.ComponentModel.DataAnnotations;

namespace BreafComande.ClassFolder
{
    public class Categorie
    {
        [Key]
        public int CategorieId { get; set; }
        public string Nom { get; set; } = string.Empty;

        // Propriété de navigation vers Articles
        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}
