using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shipping_System.Models;

namespace Shipping_System.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Branch> Branch { get; set; }
        public DbSet<Trader> Traders { get; set; }
        public DbSet<Representative> Representatives { get; set; }
        public DbSet<DeliverType> DeliverTypes { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<DiscountType> DiscountTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<WeightSetting> WeightSetting { get; set; }
        public DbSet<OrderType> OrderTypes { get; set; }
        public DbSet<OrderState> OrderStates { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Representative> Representative { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Representative>().HasKey("AppUserId");
            builder.Entity<Trader>().HasKey("AppUserId");

            var secutitySchema = "security";
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("Users", secutitySchema);
            builder.Entity<IdentityRole>().ToTable("Roles", secutitySchema);
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", secutitySchema);
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", secutitySchema);
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", secutitySchema);
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", secutitySchema);
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin", secutitySchema);


            builder.Entity<Branch>().Property(m => m.CreationDate).HasDefaultValueSql("GetDate()");
            builder.Entity<Branch>().Property(m => m.IsDeleted).HasDefaultValue(false);
            builder.Entity<Branch>().HasData(new Branch { Id = 1, Name = "Ramsess", CreationDate = DateTime.Now, IsDeleted = false });
            builder.Entity<Branch>().HasData(new Branch { Id = 2, Name = "Maady", CreationDate = DateTime.Now, IsDeleted = false });
            
            //builder.Entity<ApplicationUser>().Property(m => m.creationDate).HasDefaultValueSql("GetDate()");

            builder.Entity<Branch>().Property(m => m.CreationDate).HasDefaultValueSql("GetDate()");
            builder.Entity<Order>().Property(m => m.creationDate).HasDefaultValueSql("GetDate()");

            builder.Entity<Branch>().Property(m => m.IsDeleted).HasDefaultValue(false);
            builder.Entity<City>().Property(m => m.IsDeleted).HasDefaultValue(false);
            builder.Entity<Governorate>().Property(m => m.IsDeleted).HasDefaultValue(false);
            builder.Entity<Order>().Property(m => m.IsDeleted).HasDefaultValue(false);
            //builder.Entity<Order>().Property(m => m.RepresentativeId).HasDefaultValue("2a8a426c-a4bc-4335-a65d-700166a88e57");
            builder.Entity<Product>().Property(m => m.IsDeleted).HasDefaultValue(false);
            builder.Entity<Representative>().Property(m => m.IsDeleted).HasDefaultValue(false);
            builder.Entity<Trader>().Property(m => m.IsDeleted).HasDefaultValue(false);
            builder.Entity<TraderSpecialPriceForCities>().Property(m => m.IsDeleted).HasDefaultValue(false);



            builder.Entity<Governorate>().HasData(new Governorate { Id = 10, Name = "Matrooh", });



            builder.Entity<DeliverType>().HasData(new DeliverType { Id = 1, Type = "Normal", Price = 0 });
            builder.Entity<DeliverType>().HasData(new DeliverType { Id = 2, Type = "2 Days", Price = 30 });
            builder.Entity<DeliverType>().HasData(new DeliverType { Id = 3, Type = "24 Hours", Price = 50 });


            builder.Entity<OrderState>().HasData(new OrderState { Id = 1, Name = "New" });
            builder.Entity<OrderState>().HasData(new OrderState { Id = 2, Name = "Waiting" });
            builder.Entity<OrderState>().HasData(new OrderState { Id = 3, Name = "Delivered to Rep." });
            builder.Entity<OrderState>().HasData(new OrderState { Id = 4, Name = "Delivered to client" });
            builder.Entity<OrderState>().HasData(new OrderState { Id = 5, Name = "Cannot reach" });
            builder.Entity<OrderState>().HasData(new OrderState { Id = 6, Name = "Postponed" });
            builder.Entity<OrderState>().HasData(new OrderState { Id = 7, Name = "Partially delivered" });
            builder.Entity<OrderState>().HasData(new OrderState { Id = 8, Name = "Canceled by client" });
            builder.Entity<OrderState>().HasData(new OrderState { Id = 9, Name = "Declined, but Paid" });
            builder.Entity<OrderState>().HasData(new OrderState { Id = 10, Name = "Declined, Partially Paid" });
            builder.Entity<OrderState>().HasData(new OrderState { Id = 11, Name = "Declined, but not Paid" });



            builder.Entity<OrderType>().HasData(new OrderType { Id = 1, Name = "From Branch" });
            builder.Entity<OrderType>().HasData(new OrderType { Id = 2, Name = "From Trader" });

            builder.Entity<PaymentMethod>().HasData(new PaymentMethod { Id = 1, Name = "Cash" });
            builder.Entity<PaymentMethod>().HasData(new PaymentMethod { Id = 2, Name = "Visa" });

            builder.Entity<WeightSetting>().HasData(new WeightSetting { Id = 1, DefaultSize = 10, PriceForEachExtraKilo = 100 });
        }
    }
}