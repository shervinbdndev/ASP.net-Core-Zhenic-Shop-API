using Microsoft.EntityFrameworkCore;
using ECommerceShopApi.Models.Category;
using ECommerceShopApi.Models.ProductModel;
using ECommerceShopApi.Models.CartNameSpace;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ECommerceShopApi.Models {

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) {
                
            }


        public DbSet<Cart> Carts {get; set;} = null!;
        public DbSet<CartItem> CartItems {get; set;} = null!;
        public DbSet<Product> Products {get; set;} = null!;
        public DbSet<CategoryModel> Categories {get; set;} = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Cart>().HasKey(c => c.Id);
            builder.Entity<Cart>().Property(c => c.UserId).IsRequired();
            builder.Entity<Cart>().HasMany(c => c.Items).WithOne(ci => ci.Cart).HasForeignKey(ci => ci.CartId).OnDelete(DeleteBehavior.Cascade);


            builder.Entity<CartItem>().HasKey(ci => ci.Id);
            builder.Entity<CartItem>().Property(ci => ci.Quantity).IsRequired().HasDefaultValue(1);
            builder.Entity<CartItem>().HasOne(ci => ci.Product).WithMany().HasForeignKey(ci => ci.ProductId).OnDelete(DeleteBehavior.Cascade);

            
            builder.Entity<CategoryModel>().HasKey(c => c.Id);
            builder.Entity<CategoryModel>().Property(c => c.Name).IsRequired().HasMaxLength(50);


            builder.Entity<Product>().HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryById).OnDelete(DeleteBehavior.Cascade);
        }
    }
}