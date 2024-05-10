using BackendBlazorSecurity8.UnitsOfWork.Interfaces;
using Orders.Shared.Entities;
using SharedBlazorSecurity.Enums;
using SharedBlazorSecurity.Models;


#pragma warning disable IDE0290 // Usar constructor principal
namespace BackendBlazorSecurity8.Data
{
	public class SeedDb
	{
        private readonly ApplicationDbContext _context;
        private readonly IUserUnitsOfWork _usersUnitsOfWork; 

        public SeedDb(ApplicationDbContext context, IUserUnitsOfWork usersUnitOfWork)

        {
            _context = context;
            _usersUnitsOfWork = usersUnitOfWork;
        }

        public async Task SeedAsync() 
        {
            await _context.Database.EnsureCreatedAsync();
			await CheckCategoriesAsync();
			await CheckCountriesAsync();
			await CheckRolesAsync();
			await CheckUserAsync("101010", "Juan", "Zuluaga", "zulu@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", UserType.Admin);

		}

		private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, UserType userType) 
		{
			var user = await _usersUnitsOfWork.GetUserAsync(email);
			if (user == null) 
			{
				user = new User
				{
					FirstName = firstName,
					LastName = lastName,
					Email = email,
					UserName = email,
					PhoneNumber = phone,
					Address = address,
					Document = document,
					UserType = userType,
					City = _context.Ciudades.FirstOrDefault(),

				};
				
				await _usersUnitsOfWork.AddUserAsync(user, "123456");
				await _usersUnitsOfWork.AddUserToRoleAsync(user, userType.ToString());
			}
			return user;
		}		

		private async Task CheckRolesAsync()
		{
			await _usersUnitsOfWork.CheckRoleAsync(UserType.Admin.ToString());
			await _usersUnitsOfWork.CheckRoleAsync(UserType.User.ToString());
		}

		private async Task CheckCategoriesAsync()
		{
			if (!_context.Categories.Any())
			{
				_context.Categories.Add(new Category { Name = "Apple" });
				_context.Categories.Add(new Category { Name = "Autos" });
				_context.Categories.Add(new Category { Name = "Belleza" });
				_context.Categories.Add(new Category { Name = "Calzado" });
				_context.Categories.Add(new Category { Name = "Comida" });
				_context.Categories.Add(new Category { Name = "Cosmeticos" });
				_context.Categories.Add(new Category { Name = "Deportes" });
				_context.Categories.Add(new Category { Name = "Erótica" });
				_context.Categories.Add(new Category { Name = "Ferreteria" });
				_context.Categories.Add(new Category { Name = "Gamer" });
				_context.Categories.Add(new Category { Name = "Hogar" });
				_context.Categories.Add(new Category { Name = "Jardín" });
				_context.Categories.Add(new Category { Name = "Jugetes" });
				_context.Categories.Add(new Category { Name = "Lenceria" });
				_context.Categories.Add(new Category { Name = "Mascotas" });
				_context.Categories.Add(new Category { Name = "Nutrición" });
				_context.Categories.Add(new Category { Name = "Ropa" });
				_context.Categories.Add(new Category { Name = "Tecnología" });
				await _context.SaveChangesAsync();
			}
		}

		private async Task CheckCountriesAsync()
		{
			if (!_context.Paises.Any())
			{
				_context.Paises.Add(new Country
				{
					name = "Colombia",
					states =
			[
				new State()
				{
					Name = "Antioquia",
					Cities = [
						new City() { Name = "Medellín" },
						new City() { Name = "Itagüí" },
						new City() { Name = "Envigado" },
						new City() { Name = "Bello" },
						new City() { Name = "Rionegro" },
					]
				},
				new State()
				{
					Name = "Bogotá",
					Cities = [
						new City() { Name = "Usaquen" },
						new City() { Name = "Champinero" },
						new City() { Name = "Santa fe" },
						new City() { Name = "Useme" },
						new City() { Name = "Bosa" },
					]
				},
			]
				});
				_context.Paises.Add(new Country
				{
					name = "Estados Unidos",
					states =
			[
				new State()
				{
					Name = "Florida",
					Cities = [
						new City() { Name = "Orlando" },
						new City() { Name = "Miami" },
						new City() { Name = "Tampa" },
						new City() { Name = "Fort Lauderdale" },
						new City() { Name = "Key West" },
					]
				},
				new State()
				{
					Name = "Texas",
					Cities = [
						new City() { Name = "Houston" },
						new City() { Name = "San Antonio" },
						new City() { Name = "Dallas" },
						new City() { Name = "Austin" },
						new City() { Name = "El Paso" },
					]
				},
			]
				});
			}

			await _context.SaveChangesAsync();

		}
	}
}
