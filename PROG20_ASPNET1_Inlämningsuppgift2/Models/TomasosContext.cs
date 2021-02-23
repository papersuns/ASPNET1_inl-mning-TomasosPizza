using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace PROG20_ASPNET1_Inlämningsuppgift2.Models
{
    public partial class TomasosContext : DbContext
    {
        public TomasosContext()
        {
        }

        public TomasosContext(DbContextOptions<TomasosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bestallning> Bestallnings { get; set; }
        public virtual DbSet<BestallningMatratt> BestallningMatratts { get; set; }
        public virtual DbSet<Kund> Kunds { get; set; }
        public virtual DbSet<Matratt> Matratts { get; set; }
        public virtual DbSet<MatrattProdukt> MatrattProdukts { get; set; }
        public virtual DbSet<MatrattTyp> MatrattTyps { get; set; }
        public virtual DbSet<Produkt> Produkts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost; Database=Tomasos ;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Finnish_Swedish_CI_AS_KS_WS");

            modelBuilder.Entity<Bestallning>(entity =>
            {
                entity.ToTable("Bestallning");

                entity.Property(e => e.BestallningId).HasColumnName("BestallningID");

                entity.Property(e => e.BestallningDatum).HasColumnType("datetime");

                entity.Property(e => e.KundId).HasColumnName("KundID");

                entity.HasOne(d => d.Kund)
                    .WithMany(p => p.Bestallnings)
                    .HasForeignKey(d => d.KundId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bestallning_Kund");
            });

            modelBuilder.Entity<BestallningMatratt>(entity =>
            {
                entity.HasKey(e => new { e.MatrattId, e.BestallningId });

                entity.ToTable("BestallningMatratt");

                entity.Property(e => e.MatrattId).HasColumnName("MatrattID");

                entity.Property(e => e.BestallningId).HasColumnName("BestallningID");

                entity.Property(e => e.Antal).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Bestallning)
                    .WithMany(p => p.BestallningMatratts)
                    .HasForeignKey(d => d.BestallningId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BestallningMatratt_Bestallning");

                entity.HasOne(d => d.Matratt)
                    .WithMany(p => p.BestallningMatratts)
                    .HasForeignKey(d => d.MatrattId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BestallningMatratt_Matratt");
            });

            modelBuilder.Entity<Kund>(entity =>
            {
                entity.HasKey(e => e.KundId);

                entity.ToTable("Kund");

                entity.Property(e => e.KundId).HasColumnName("KundID");

                entity.Property(e => e.AnvandarNamn)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Gatuadress)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasMaxLength(450);

                entity.Property(e => e.Losenord)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Namn)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Postnr)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Postort)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Telefon)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Matratt>(entity =>
            {
                entity.ToTable("Matratt");

                entity.Property(e => e.MatrattId).HasColumnName("MatrattID");

                entity.Property(e => e.Beskrivning)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MatrattNamn)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.MatrattTypNavigation)
                    .WithMany(p => p.Matratts)
                    .HasForeignKey(d => d.MatrattTyp)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Matratt_MatrattTyp");
            });

            modelBuilder.Entity<MatrattProdukt>(entity =>
            {
                entity.HasKey(e => new { e.MatrattId, e.ProduktId });

                entity.ToTable("MatrattProdukt");

                entity.Property(e => e.MatrattId).HasColumnName("MatrattID");

                entity.Property(e => e.ProduktId).HasColumnName("ProduktID");

                entity.HasOne(d => d.Matratt)
                    .WithMany(p => p.MatrattProdukts)
                    .HasForeignKey(d => d.MatrattId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MatrattProdukt_Matratt");

                entity.HasOne(d => d.Produkt)
                    .WithMany(p => p.MatrattProdukts)
                    .HasForeignKey(d => d.ProduktId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MatrattProdukt_Produkt");
            });

            modelBuilder.Entity<MatrattTyp>(entity =>
            {
                entity.HasKey(e => e.MatrattTyp1);

                entity.ToTable("MatrattTyp");

                entity.Property(e => e.MatrattTyp1).HasColumnName("MatrattTyp");

                entity.Property(e => e.Beskrivning)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Produkt>(entity =>
            {
                entity.ToTable("Produkt");

                entity.Property(e => e.ProduktId).HasColumnName("ProduktID");

                entity.Property(e => e.ProduktNamn)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
