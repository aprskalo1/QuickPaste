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

    public virtual DbSet<TextStorage> TextStorages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("server=.;Database=QuickPaste;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FileStorage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FileStor__3213E83F8177C759");

            entity.ToTable("FileStorage");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Filename)
                .HasMaxLength(255)
                .HasColumnName("filename");
            entity.Property(e => e.HashedCode)
                .HasMaxLength(255)
                .HasColumnName("hashed_code");
            entity.Property(e => e.TimeUploaded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("time_uploaded");
        });

        modelBuilder.Entity<TextStorage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TextStor__3213E83F0FF25B2A");

            entity.ToTable("TextStorage");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Filename)
                .HasMaxLength(255)
                .HasColumnName("filename");
            entity.Property(e => e.HashedCode)
                .HasMaxLength(255)
                .HasColumnName("hashed_code");
            entity.Property(e => e.TimeUploaded)
                .HasColumnType("datetime")
                .HasColumnName("time_uploaded");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
