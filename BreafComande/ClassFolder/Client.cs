using System.ComponentModel.DataAnnotations;

namespace BreafComande.ClassFolder
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        public string Prenom { get; set; }

        public string Nom { get; set; }

        public string Telephone { get; set; }

        public string Adresse { get; set; }

        // Collection de commandes liées à ce client
        public ICollection<Commande> Commandes { get; set; } // Un client peut avoir plusieurs commandes
    }
}