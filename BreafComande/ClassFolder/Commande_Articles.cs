using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BreafComande.ClassFolder
{
    public class Commande_Articles
    {
        [Key, Column(Order = 0)]
        public int IdCommande { get; set; }

        [Key, Column(Order = 1)]
        public int ArticleId { get; set; }

        [Required]
        public int Quantite { get; set; }

        [Required]
        public decimal Prix { get; set; }

        [Required]
        [StringLength(100)]
        public string Nom { get; set; } = string.Empty;

        // Propriétés de navigation
        [ForeignKey("IdCommande")]
        public Commande Commande { get; set; }

        [ForeignKey("ArticleId")]
        public Article Article { get; set; }
    }
}
