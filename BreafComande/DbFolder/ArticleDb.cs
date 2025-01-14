using BreafComande.ClassFolder;
using Microsoft.EntityFrameworkCore;

namespace BreafComande.DbFolder
{
    public class ArticleDb : DbContext
    {
        public ArticleDb(DbContextOptions<ArticleDb> options)
        : base(options) { }

        public DbSet<Article> Articles => Set<Article>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // relations entre les entités
            modelBuilder.Entity<Article>()
                .HasMany(article => article.CommandeArticles)
                .WithOne()
                .HasForeignKey("Id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
