using api_project.Models;
using Microsoft.EntityFrameworkCore;

namespace api_project.Data
{
    public class ApplicationDbContext: DbContext

    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
    
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Gift> Gifts => Set<Gift>();
        public DbSet<Donor> Donors => Set<Donor>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            

                // ============================
                // Donor
                // ============================
                modelBuilder.Entity<Donor>(entity =>
                {
                    entity.HasKey(d => d.Id);
                    entity.Property(d => d.DonorName)
                          .IsRequired()
                          .HasMaxLength(100);
                    entity.Property(d => d.Email)
                          .IsRequired();

                    entity.HasMany(d => d.Gifts)
                          .WithOne(g => g.Donor)
                          .HasForeignKey(g => g.DonorId)
                          .OnDelete(DeleteBehavior.Restrict);
                });

                // ============================
                // Gift
                // ============================
                modelBuilder.Entity<Gift>(entity =>
                {
                    entity.HasKey(g => g.Id);

                    entity.Property(g => g.GiftName)
                          .IsRequired()
                          .HasMaxLength(150);

                    entity.Property(g => g.Description);
                         

                    entity.Property(g => g.ImageUrl);

                    entity.Property(g => g.NumOfPurchases)
                          .HasDefaultValue(0);

                    entity.Property(g => g.Price)
                          .IsRequired()
                          .HasColumnType("decimal(18,2)");

                    entity.Property(g => g.DonorId)
                    .IsRequired();


                    entity.Property(g => g.WinnerId);
                    
                });

                // ============================
                // User
                // ============================
                modelBuilder.Entity<User>(entity =>
                {
                    entity.HasKey(u => u.Id);

                    entity.Property(u => u.Name)
                          .IsRequired()
                          .HasMaxLength(100);

                    entity.Property(u => u.Email)
                          .IsRequired();

                    entity.Property(u => u.Phone)
                          .IsRequired()
                          .HasMaxLength(20);

                    entity.Property(u => u.Password)
                          .IsRequired();

                    entity.Property(u => u.Role)
                          .IsRequired().HasDefaultValue("User");

                    entity.HasMany(u => u.Orders)
                          .WithOne(o => o.User)
                          .HasForeignKey(o => o.UserId)
                          .OnDelete(DeleteBehavior.Cascade);
                });

                // ============================
                // Order
                // ============================
                modelBuilder.Entity<Order>(entity =>
                {
                    entity.HasKey(o => o.Id);

                    entity.Property(o => o.IsConfirmed)
                          .HasDefaultValue(false);
                });
            }
        }
    }

