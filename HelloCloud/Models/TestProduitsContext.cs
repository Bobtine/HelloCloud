using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HelloCloud.Models
{
    public partial class TestProduitsContext : DbContext
    {
        public TestProduitsContext()
        {
        }

        public TestProduitsContext(DbContextOptions<TestProduitsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Produit> Produits { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TestProduitsDB;Trusted_Connection=True;");
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.IdCategorie)
                    .HasName("PK__Categori__A3C02A1C4F8AF20D");
            });

            modelBuilder.Entity<Produit>(entity =>
            {
                entity.HasKey(e => e.IdProduit)
                    .HasName("PK__Produits__2E8997F030687A84");

                entity.HasOne(d => d.IdCategorieNavigation)
                    .WithMany(p => p.Produits)
                    .HasForeignKey(d => d.IdCategorie)
                    .HasConstraintName("FK__Produits__IdCate__267ABA7A");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
