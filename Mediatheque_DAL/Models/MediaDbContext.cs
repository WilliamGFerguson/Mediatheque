using Microsoft.EntityFrameworkCore;

namespace Mediatheque_DAL.Models
{
    public class MediaDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<MediaItem> MediaItems { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<MediaStatus> Media_Statuses { get; set; }
        public DbSet<MediaGenre> Media_Genres { get; set; }

        public MediaDbContext() { }

        public MediaDbContext(DbContextOptions<MediaDbContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Mich: server=localhost;port=3306;uid=root;pwd=18dEveloppement;database=mediatheque
            //Will: server=localhost;port=3306;uid=root;pwd=dev;database=mediatheque
            optionsBuilder.UseMySql("server=localhost;port=3306;uid=root;pwd=18dEveloppement;database=mediatheque", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.33-mysql"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u  => u.Id).HasName("PRIMARY");
                entity.Property(u => u.Id).HasColumnName("id");
                entity.Property(u => u.Username).IsRequired().HasMaxLength(100).HasColumnName("username");
                entity.Property(u => u.Email).HasMaxLength(100).HasColumnName("email");
                entity.HasMany(u => u.MediaStatuses).WithOne(m => m.User)
                    .HasForeignKey(u => u.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("user_ibfk_1");
                entity.ToTable("Users");
            });

            modelBuilder.Entity<MediaItem>(entity =>
            {
                entity.HasKey(m => m.Id).HasName("PRIMARY");
                entity.Property(m => m.Id).HasColumnName("id");
                entity.Property(m => m.Type).IsRequired().HasMaxLength(100).HasColumnName("type");
                entity.Property(m => m.Title).IsRequired().HasMaxLength(150).HasColumnName("title");
                entity.Property(m => m.LengthMinutes).HasMaxLength(4).HasColumnName("length_minutes");
                entity.Property(m => m.Year).IsRequired().HasMaxLength(4).HasColumnName("year");
                entity.ToTable("MediaItems");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(g => g.Id).HasName("PRIMARY");
                entity.Property(g => g.Id).HasColumnName("id");
                entity.HasIndex(g => g.GenreName).IsUnique();
                entity.Property(g => g.GenreName).HasColumnName("genre_name");
                entity.ToTable("Genres");
            });

            modelBuilder.Entity<MediaStatus>(entity =>
            {
                entity.HasKey(ms => new { ms.UserId, ms.MediaId });
                entity.Property(ms => ms.UserId).HasColumnName("user_id");
                entity.Property(ms => ms.MediaId).HasColumnName("media_id");
                entity.Property(ms => ms.Status).HasColumnName("status");
                entity.HasOne(u => u.User).WithMany(ms => ms.MediaStatuses)
                    .HasForeignKey(ms => ms.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Status_user_ibfk");
                entity.HasOne(m => m.MediaItem).WithMany(ms => ms.MediaStatuses)
                    .HasForeignKey(ms => ms.MediaId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("media_item_status_ibfk");
                entity.ToTable("Media_Statuses");
            });

            modelBuilder.Entity<MediaGenre>(entity =>
            {
                entity.HasKey(mg => new { mg.GenreId, mg.MediaId });
                entity.Property(mg => mg.GenreId).HasColumnName("genre_id");
                entity.Property(mg => mg.MediaId).HasColumnName("media_id");
                entity.HasOne(m => m.MediaItem).WithMany(mg => mg.MediaGenres)
                    .HasForeignKey(mg => mg.MediaId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("media_item_genre_ibfk");
                entity.HasOne(g => g.Genre).WithMany(mg => mg.MediaGenres)
                    .HasForeignKey(mg => mg.GenreId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("genre_ibfk");
                entity.ToTable("Media_Genres");
            });
        }
    }
}
