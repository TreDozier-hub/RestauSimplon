namespace BreafComande.ClassFolder
{
    public class CalculCommande
    {
        public ICollection<Commande_Articles> ListeArticles { get; set; } = new List<Commande_Articles>();
        public bool StatutCommande { get; set; }
        public decimal Total { get; set; }

        public int NombreArticles()
        {
            return ListeArticles?.Sum(article => article.Quantite) ?? 0;
        }

        public void CalculerTotal()
        {
            if (NombreArticles() == 0)
            {
                Console.WriteLine("Pas assez d'article pour valider la commande");
                return;
            }
            else if (!StatutCommande)
            {
                Console.WriteLine("Le statut de la commande est 'false'.");
                return;
            }
            Total = Math.Round(ListeArticles?.Sum(article => article.Prix * article.Quantite) ?? 0, 2);
            Console.WriteLine($"Total : {Total}");
        }
    }
}
