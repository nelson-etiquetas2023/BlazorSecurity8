using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Orders.Shared.Entities;
using SharedBlazorSecurity.Models;

#pragma warning disable IDE0290 // Usar constructor principal
namespace BackendBlazorSecurity8.Data
{
	public class ApplicationDbContext : IdentityDbContext<User>
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Category> Categorias { get; set; }
		public DbSet<City> Ciudades { get; set; }
		public DbSet<Country> Paises { get; set; }
		public DbSet<State> Estados { get; set; }
	}
}
