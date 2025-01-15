using BreafComande.ClassFolder;

public class ClientDTO
{

    public string Prenom { get; set; }  
    public string Nom { get; set; }
    public string Telephone { get; set; }
    public string Adresse { get; set; }
    
    public ClientDTO() { }

    // Constructeur pour créer un DTO à partir de l'entité Clients
    public ClientDTO(Client client)
    {

        Prenom = client.Prenom;
        Nom = client.Nom;
        Telephone = client.Telephone;
        Adresse = client.Adresse;
    }
}