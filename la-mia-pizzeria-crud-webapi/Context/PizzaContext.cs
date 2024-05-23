using la_mia_pizzeria_crud_webapi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_crud_webapi.Context
{
    public class PizzaContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Category> Category { get; set; }
        public DbSet<Pizza> Pizza { get; set; }
        public DbSet<Ingredient> Ingrediente { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Initial Catalog=db_mypizzeria;");
        }
    }
}
