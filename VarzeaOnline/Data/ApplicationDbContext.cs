using System.Data.Entity;
using System.Web.UI.WebControls;
using VarzeaOnline.Migrations;
using VarzeaOnline.Models;
using static System.Data.Entity.Migrations.Model.UpdateDatabaseOperation;

namespace VarzeaOnline.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("CampeonatoConnection")
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Campeonato> Campeonatos { get; set; }
        public DbSet<Jogador> Jogadores { get; set; }
        public DbSet<Partida> Partidas { get; set; }
        public DbSet<Classificacao> Classificacoes { get; set; }
        public DbSet<Time> Times { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();

            modelBuilder.Entity<Partida>()
                .HasRequired(p => p.JogadorCasa)
                .WithMany()
                .HasForeignKey(p => p.IdJogadorCasa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Partida>()
                .HasRequired(p => p.JogadorFora)
                .WithMany()
                .HasForeignKey(p => p.IdJogadorFora)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
