using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace eproject3.Models.Generated;

public partial class ABCDMallContext : DbContext
{
    public ABCDMallContext()
    {
    }

    public ABCDMallContext(DbContextOptions<ABCDMallContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Gallery> Galleries { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<MovieShowtime> MovieShowtimes { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<Theater> Theaters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Connection string is provided through dependency injection in Program.cs
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MovieShowtime>(entity =>
        {
            entity.ToTable("MovieShowtimes");
            entity.HasKey(e => e.ShowtimeId);
            
            entity.Property(e => e.TicketPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.HasOne(d => d.Movie)
                .WithMany(p => p.MovieShowtimes)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("Bookings");
            entity.HasKey(e => e.BookingId);

            entity.Property(e => e.BookingId)
                .HasDefaultValueSql("(newid())");

            entity.Property(e => e.CustomerName)
                .HasMaxLength(100);

            entity.Property(e => e.Email)
                .HasMaxLength(100);

            entity.Property(e => e.Phone)
                .HasMaxLength(20);

            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            entity.Property(e => e.Qrcode)
                .HasColumnName("QRCode")
                .HasMaxLength(100);

            entity.Property(e => e.BookingTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.ExpiryTime)
                .HasColumnType("datetime");

            entity.HasIndex(e => e.Qrcode)
                .IsUnique();

            entity.HasOne(d => d.MovieShowtime)
                .WithMany(p => p.Bookings)
                .HasForeignKey(d => d.ShowtimeId);

            entity.HasOne(d => d.Seat)
                .WithMany(p => p.Bookings)
                .HasForeignKey(d => d.SeatId);
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__6A4BEDD6F70CCA68");

            entity.ToTable("Feedback");

            entity.Property(e => e.CustomerName).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.SubmissionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Gallery>(entity =>
        {
            entity.HasKey(e => e.GalleryId).HasName("PK__Gallery__CF4F7BB5E0A4453A");

            entity.ToTable("Gallery");

            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.UploadDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.MovieId).HasName("PK__Movies__4BD2941AAC042187");

            entity.Property(e => e.CoverImage).HasMaxLength(500);
            entity.Property(e => e.IsHot).HasDefaultValue(false);
            entity.Property(e => e.IsPopular).HasDefaultValue(false);
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.TrailerUrl).HasMaxLength(500);
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.SeatId).HasName("PK__Seats__311713F336A23474");

            entity.HasIndex(e => e.SeatCode, "UQ__Seats__B0DB9629F23F8AD2").IsUnique();

            entity.Property(e => e.IsBooked).HasDefaultValue(false);
            entity.Property(e => e.SeatCode).HasMaxLength(10);

            entity.HasOne(d => d.Theater).WithMany(p => p.Seats)
                .HasForeignKey(d => d.TheaterId)
                .HasConstraintName("FK__Seats__TheaterId__4222D4EF");
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.HasKey(e => e.ShopId).HasName("PK__Shops__67C557C924430769");

            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.ShopName).HasMaxLength(100);
        });

        modelBuilder.Entity<Theater>(entity =>
        {
            entity.HasKey(e => e.TheaterId).HasName("PK__Theaters__4D68B2190F04751D");

            entity.Property(e => e.TheaterName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
