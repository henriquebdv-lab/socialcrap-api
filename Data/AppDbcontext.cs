using Microsoft.EntityFrameworkCore;
using SocialCrap.Models;

namespace SocialCrap.Data
{
    // DbContext do EF Core para o SocialCrap.
    public class AppDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Crap> Craps { get; set; }
        public DbSet<Noticia> Noticias { get; set; }
        public DbSet<Poop> Poops { get; set; }
        public DbSet<Amizade> Amizades { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Amizade tem dois relacionamentos com Usuário, por isso mapeamos ambos.
            modelBuilder.Entity<Amizade>()
                .HasOne(a => a.Usuario)
                .WithMany(u => u.AmizadesEnviadas)
                .HasForeignKey(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Amizade>()
                .HasOne(a => a.Amigo)
                .WithMany(u => u.AmizadesRecebidas)
                .HasForeignKey(a => a.AmigoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Poop>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Poops)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}
