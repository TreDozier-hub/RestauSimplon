namespace BreafComande.DToFolder
{
    public class CommandesDTO
    {
        //public int IdCommande { get; set; }
        public int ClientId { get; set; }
        //public string ClientNom { get; set; }
        public DateTime Date { get; set; }
        //public double Total { get; set; }
        public bool StatutCommande { get; set; }
        public List<Commande_ArticlesDTO> ListeArticles { get; set; }
    }
}
