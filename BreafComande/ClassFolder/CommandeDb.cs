using BreafComande.ClassFolder;
using Microsoft.EntityFrameworkCore;

namespace BreafComande.DbFolder
{
    public class CommandeDb : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Commande_Articles> Commande_Articles { get; set; }

        public CommandeDb(DbContextOptions<CommandeDb> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurer la clé primaire composite pour Commande_Articles
            modelBuilder.Entity<Commande_Articles>()
                .HasKey(ca => new { ca.IdCommande, ca.ArticleId });

            // Relation entre Commande et Commande_Articles
            modelBuilder.Entity<Commande>()
                .HasMany(commande => commande.ListeArticles)
                .WithOne(ca => ca.Commande)
                .HasForeignKey(ca => ca.IdCommande);

            // Relation entre Article et Commande_Articles
            modelBuilder.Entity<Article>()
                .HasMany(article => article.CommandeArticles)
                .WithOne(ca => ca.Article)
                .HasForeignKey(ca => ca.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relation entre Categorie et Articles
            modelBuilder.Entity<Categorie>()
                .HasMany(categorie => categorie.Articles)
                .WithOne(article => article.Categorie)
                .HasForeignKey(article => article.CategorieId);

            // Relation entre Client et Commande
            modelBuilder.Entity<Client>()
                .HasMany(client => client.Commandes)
                .WithOne(commande => commande.Client)
                .HasForeignKey(commande => commande.IdClient)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
