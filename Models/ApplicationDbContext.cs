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

            builder.Entity<Cart>()
            .HasMany(c => c.Items)
            .WithOne(cI => cI.Cart)
            .HasForeignKey(cI => cI.CartId);

            builder.Entity<CartItem>()
            .HasOne(cI => cI.Product)
            .WithMany()
            .HasForeignKey(cI => cI.ProductId);
        }
    }
}