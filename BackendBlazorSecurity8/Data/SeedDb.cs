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
			await CheckUserAsync("1010", "Juan", "Zuluaga", "zulu@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", UserType.Admin);

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
					City = _context.Ciudades.FirstOrDefault(),
					UserType = userType
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
				_context.Paises.Add(new Country{Name = "Colombia"});
				_context.Paises.Add(new Country { Name = "Venezuela" });
				_context.Paises.Add(new Country { Name = "Brazil" });
			}
			await _context.SaveChangesAsync();
		}
	}
}
