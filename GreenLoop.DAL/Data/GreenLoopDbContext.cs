using GreenLoop.DAL.Entities;
using Microsoft.EntityFrameworkCore;


namespace GreenLoop.DAL.Data;

public class GreenLoopDbContext: DbContext
{
    public GreenLoopDbContext(DbContextOptions<GreenLoopDbContext> options): base(options){}


    public DbSet<User> Users { get; set; }
    public DbSet<UserAddress> UserAddresses { get; set; }
    public DbSet<WasteCategory> WasteCategories { get; set; }
    public DbSet<PickupRequest> PickupRequests { get; set; }
    public DbSet<RequestDetail> RequestDetails { get; set; }
    public DbSet<Coupon> Coupons { get; set; }
    public DbSet<WalletTransaction> WalletTransactions { get; set; }
    public DbSet<UserCoupon> UserCoupons { get; set; }
    public DbSet<PointTransaction> PointTransactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasDiscriminator<string>("UserType")
            .HasValue<Customer>("Customer")
            .HasValue<Driver>("Driver")
            .HasValue<Business>("Business")
            .HasValue<Admin>("Admin"); 

        // 2. User Index
        modelBuilder.Entity<User>()
            .HasIndex(u => u.PhoneNumber)
            .IsUnique();

        modelBuilder.Entity<PickupRequest>()
            .HasOne(r => r.Customer)     
            .WithMany(c => c.Requests)   
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<PickupRequest>()
            .HasOne(r => r.Driver)       
            .WithMany(d => d.Tasks)      
            .HasForeignKey(r => r.DriverId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}

