using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RentalShop.Domain.Entities;
using RentalShop.Domain.States;

namespace RentalShop.Infrastructure.Persistence
{
    public sealed class RentalDbContext : DbContext
    {
        public DbSet<RentalItem>       RentalItems       { get; set; } = null!;
        public DbSet<PackageComponent> PackageComponents { get; set; } = null!;
        public DbSet<AppUser>          Users             { get; set; } = null!;

        public RentalDbContext() { }
        public RentalDbContext(DbContextOptions<RentalDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;

            var connStr = System.Environment.GetEnvironmentVariable("RENTALSHOP_DB")
                ?? "Host=localhost;Port=5432;Database=rental_shop;Username=rental_user;Password=rental_pass";

            optionsBuilder
                .UseNpgsql(connStr)
                .EnableSensitiveDataLogging(false);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureRentalItemHierarchy(modelBuilder);
            ConfigurePackageComponentHierarchy(modelBuilder);
            ConfigureUsers(modelBuilder);
            SeedCatalog(modelBuilder);
        }

        private static void ConfigureRentalItemHierarchy(ModelBuilder modelBuilder)
        {
            var stateConverter = new ValueConverter<IItemState, string>(
                state => state.GetType().Name,
                name  => RehydrateState(name));

            var stateComparer = new ValueComparer<IItemState>(
                (a, b) => a != null && b != null && a.GetType() == b.GetType(),
                s => s.GetType().Name.GetHashCode(),
                s => RehydrateState(s.GetType().Name));

            modelBuilder.Entity<RentalItem>(entity =>
            {
                entity.ToTable("RentalItems");
                entity.HasKey(e => e.Sku);
                entity.Property(e => e.Sku).HasMaxLength(20).IsRequired();
                entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
                entity.Property(e => e.BasePricePerDay).HasPrecision(10, 2).IsRequired();

                entity.Property(e => e.CurrentState)
                      .HasConversion(stateConverter, stateComparer)
                      .HasColumnName("CurrentStateName")
                      .HasMaxLength(50)
                      .IsRequired()
                      .HasDefaultValue(new AvailableState());

                entity.Property(e => e.ExpectedReturnDate);

                entity.Property(e => e.Version)
                      .IsConcurrencyToken()
                      .IsRequired();
            });

            modelBuilder.Entity<RentalItem>()
                .HasDiscriminator<string>("ItemType")
                .HasValue<ToolItem>("Tool")
                .HasValue<GearItem>("Gear");
        }

        private static void ConfigurePackageComponentHierarchy(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PackageComponent>(entity =>
            {
                entity.ToTable("PackageComponents");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            });

            modelBuilder.Entity<RentalLineItem>(entity =>
            {
                entity.Property(e => e.Price).HasPrecision(10, 2).IsRequired();
            });

            modelBuilder.Entity<RentalPackage>()
                .HasMany<PackageComponent>("_children")
                .WithOne()
                .HasForeignKey("ParentPackageId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PackageComponent>()
                .HasDiscriminator<string>("ComponentType")
                .HasValue<RentalLineItem>("LineItem")
                .HasValue<RentalPackage>("Package");
        }

        private static void ConfigureUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).HasMaxLength(100).IsRequired();
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.PasswordHash).IsRequired();
            });
        }

        private static void SeedCatalog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToolItem>().HasData(
                new ToolItem("TOOL-001", "Cordless Drill", 12m) { Version = new System.Guid("aaaaaaaa-0001-0001-0001-000000000001") },
                new ToolItem("TOOL-002", "Circular Saw",   18m) { Version = new System.Guid("aaaaaaaa-0001-0001-0001-000000000002") },
                new ToolItem("TOOL-003", "Hammer-set",      6m) { Version = new System.Guid("aaaaaaaa-0001-0001-0001-000000000003") });

            modelBuilder.Entity<GearItem>().HasData(
                new GearItem("GEAR-001", "Camping Tent",   22m) { Version = new System.Guid("bbbbbbbb-0002-0002-0002-000000000001") },
                new GearItem("GEAR-002", "Sleeping Bag",    8m) { Version = new System.Guid("bbbbbbbb-0002-0002-0002-000000000002") },
                new GearItem("GEAR-003", "Kayak",          35m) { Version = new System.Guid("bbbbbbbb-0002-0002-0002-000000000003") });
        }

        internal static IItemState RehydrateState(string? name) => name switch
        {
            nameof(RentedState)      => (IItemState)new RentedState(),
            nameof(UnderRepairState) => new UnderRepairState(),
            _                        => new AvailableState()
        };
    }
}
