using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API_Secure.Models;

public partial class SecureDbContext : DbContext
{
    public SecureDbContext()
    {
    }

    public SecureDbContext(DbContextOptions<SecureDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer("Server=(local); Database=SecureDB; Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.Identificador).HasName("PK__Personas__F2374EB1C597D505");

            entity.HasIndex(e => e.Email, "UQ__Personas__A9D1053423C6F974").IsUnique();

            entity.HasIndex(e => e.NumeroIdentificacion, "UQ__Personas__FCA68D91D8C405A2").IsUnique();

            entity.Property(e => e.Apellidos).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombres).HasMaxLength(100);

            entity.Property(e => e.NombresApellidos)
                .HasMaxLength(201)
                .HasComputedColumnSql("(([Nombres]+' ')+[Apellidos])", true)
                .IsRequired(false);

            entity.Property(e => e.NumeroIdentificacion).HasMaxLength(20);

            entity.Property(e => e.NumeroIdentificacionConTipo)
                .HasMaxLength(73)
                .HasComputedColumnSql("(([NumeroIdentificacion]+' - ')+[TipoIdentificacion])", true)
                .IsRequired(false); 

            entity.Property(e => e.TipoIdentificacion).HasMaxLength(50);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Identificador).HasName("PK__Usuario__F2374EB1853C1B22");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Usuario1, "UQ__Usuario__E3237CF7232330BE").IsUnique();

            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Pass).HasMaxLength(128);
            entity.Property(e => e.Usuario1)
                .HasMaxLength(50)
                .HasColumnName("Usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
