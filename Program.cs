using Core.Entity;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


namespace Core.Entity
{
    public class Cliente : EntityBase
    {
        public required string Nome { get; set; }
        public DateTime? DataNascimento { get; set; }
        public ICollection<Pedido> Pedidos { get; set; }
    }

    public class Livro : EntityBase
    {
        public required string Nome { get; set; }
        public required string Editora { get; set; }
        public ICollection<Pedido> Pedidos { get; set; }
    }
    public class Pedido : EntityBase
    {
        public int ClienteId { get; set; }
        public int LivroId { get; set; }
        public Cliente Cliente { get; set; }
        public Livro Livro { get; set; }
    }

    public abstract class EntityBase
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}

namespace Infrastructure.Configuration
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Cliente");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnType("INT").ValueGeneratedNever().UseIdentityColumn();
            builder.Property(u => u.DataCriacao).HasColumnType("DATETIME").IsRequired();
            builder.Property(u => u.Nome).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(u => u.DataNascimento).HasColumnType("DATETIME");
        }
    }

    public class LivroConfiguration : IEntityTypeConfiguration<Livro>
    {
        public void Configure(EntityTypeBuilder<Livro> builder)
        {
            builder.ToTable("Livro");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnType("INT").ValueGeneratedNever().UseIdentityColumn();
            builder.Property(u => u.DataCriacao).HasColumnType("DATETIME").IsRequired();
            builder.Property(u => u.Nome).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(u => u.Editora).HasColumnType("VARCHAR(100)").IsRequired();
        }
    }

    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedido");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnType("INT").ValueGeneratedNever().UseIdentityColumn();
            builder.Property(u => u.DataCriacao).HasColumnType("DATETIME").IsRequired();
            builder.Property(u => u.ClienteId).HasColumnType("INT").IsRequired();
            builder.Property(u => u.LivroId).HasColumnType("INT").IsRequired();
            builder.HasOne(p => p.Cliente)
                .WithMany(u => u.Pedidos)
                .HasPrincipalKey(u => u.Id);
            builder.HasOne(p => p.Livro)
                .WithMany(u => u.Pedidos)
                .HasPrincipalKey(u => u.Id);
        }
    }
}

namespace Infrastructure.Repository
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Livro> Livro { get; set; }
        public DbSet<Pedido> Pedido { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=127.0.0.1,1433;Database=BookStore;User Id=sa;Password=FiapGames!;Trusted_Connection=False;MultipleActiveResultSets=true;TrustServerCertificate=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            modelBuilder.ApplyConfiguration(new LivroConfiguration());
            modelBuilder.ApplyConfiguration(new PedidoConfiguration());
        }
    }
}


