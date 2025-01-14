using BreafComande.ClassFolder;

public class ClientDTO
{

    public string Prenom { get; set; }    // Le prénom du client
    public string Nom { get; set; }       // Le nom du client
    public string Telephone { get; set; } // Le numéro de téléphone du client
    public string Adresse { get; set; }   // L'adresse du client

    // Constructeur sans paramètre
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