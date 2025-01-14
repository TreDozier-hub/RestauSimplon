using System.ComponentModel.DataAnnotations;
using BreafComande.ClassFolder;

public class Commande
{
    [Key]
    public int IdCommande { get; set; }
    public int IdClient { get; set; }
    public DateTime Date { get; set; }
    public decimal Total { get; set; }
    public bool StatutCommande { get; set; }

    // Relation avec Client (un client pour une commande)
    public Client Client { get; set; }

    // Propriété de navigation pour Commande_Articles
    public ICollection<Commande_Articles> ListeArticles { get; set; } = new List<Commande_Articles>();
}
