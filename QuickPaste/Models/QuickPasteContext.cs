using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QuickPaste.Models;

public partial class QuickPasteContext : DbContext
{
    public QuickPasteContext()
    {
    }

    public QuickPasteContext(DbContextOptions<QuickPasteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FileStorage> FileStorages { get; set; }

    public virtual DbSet<PastedCode> PastedCodes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("server=.;Database=QuickPaste;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FileStorage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FileStor__3213E83F315726D1");

            entity.ToTable("FileStorage");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Filename)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("filename");
            entity.Property(e => e.PastedCodeId).HasColumnName("pasted_code_id");
            entity.Property(e => e.TimeUploaded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("time_uploaded");

            entity.HasOne(d => d.PastedCode).WithMany(p => p.FileStorages)
                .HasForeignKey(d => d.PastedCodeId)
                .HasConstraintName("FK__FileStora__paste__44FF419A");
        });

        modelBuilder.Entity<PastedCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PastedCo__3213E83F0E5F6162");

            entity.ToTable("PastedCode");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CodeHash)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("code_hash");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
