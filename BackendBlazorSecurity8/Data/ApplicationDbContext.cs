using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Orders.Shared.Entities;
using SharedBlazorSecurity.Models;
using System.Reflection.Emit;

#pragma warning disable CA1822 // Marcar miembros como static
#pragma warning disable IDE0290 // Usar constructor principal
namespace BackendBlazorSecurity8.Data
{
	public class ApplicationDbContext : IdentityDbContext<User>
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
		public DbSet<City> Ciudades { get; set; }
		public DbSet<Country> Paises { get; set; }
		public DbSet<State> Estados { get; set; }

		protected override void OnModelCreating(ModelBuilder modelbuilder)
		{
			base.OnModelCreating(modelbuilder);
			modelbuilder.Entity<Category>().HasIndex(x => x.Name).IsUnique();
			modelbuilder.Entity<Country>().HasIndex(x => x.name).IsUnique();
			modelbuilder.Entity<City>().HasIndex(x => new { x.StateId, x.Name }).IsUnique();
			modelbuilder.Entity<State>().HasIndex(x => new { x.CountryId, x.Name }).IsUnique();
			DisableCascadingDelete(modelbuilder);
		}
		private void DisableCascadingDelete(ModelBuilder modelBuilder)
		{
			var relationships = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());
			foreach (var relationship in relationships)
			{
				relationship.DeleteBehavior = DeleteBehavior.Restrict;
			}
		}

	}
	
}
