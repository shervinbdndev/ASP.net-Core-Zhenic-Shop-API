using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ECommerceShopApi.Models {

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) {
                
            }


        public DbSet<Cart> carts {get; set;}
        public DbSet<CartItem> cartItems {get; set;}
        public DbSet<Product> Products {get; set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Cart>().HasKey(c => c.Id);
            builder.Entity<Cart>().Property(c => c.UserId).IsRequired();
            builder.Entity<Cart>().HasMany(c => c.Items).WithOne(ci => ci.Cart).HasForeignKey(ci => ci.CartId).OnDelete(DeleteBehavior.Cascade);


            builder.Entity<CartItem>().HasKey(ci => ci.Id);
            builder.Entity<CartItem>().Property(ci => ci.Quantity).IsRequired().HasDefaultValue(1);
            builder.Entity<CartItem>().HasOne(ci => ci.Product).WithMany().HasForeignKey(ci => ci.ProductId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}