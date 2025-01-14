namespace BreafComande.ClassFolder
{
    public class Article
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public decimal Prix { get; set; }
        public int CategorieId { get; set; }  // Clé étrangère vers Categorie

        // Propriété de navigation vers Categorie
        public Categorie Categorie { get; set; }
        public ICollection<Commande_Articles> CommandeArticles { get; set; } = new List<Commande_Articles>();
    }
}
