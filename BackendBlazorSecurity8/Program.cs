using BackendBlazorSecurity8.Data;
using BackendBlazorSecurity8.Repositories.Implementations;
using BackendBlazorSecurity8.Repositories.Interfaces;
using BackendBlazorSecurity8.UnitsOfWork.Implementations;
using BackendBlazorSecurity8.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Identity;
using SharedBlazorSecurity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x => 
x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Orders Backend", Version = "v1" });
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = @"JWT Authorization header using the Bearer scheme. <br /> <br />
                      Enter 'Bearer' [space] and then your token in the text input below.<br /> <br />
                      Example: 'Bearer 12345abcdef'<br /> <br />",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});
	c.AddSecurityRequirement(new OpenApiSecurityRequirement()
	  {
		{
		  new OpenApiSecurityScheme
		  {
			Reference = new OpenApiReference
			  {
				Type = ReferenceType.SecurityScheme,
				Id = "Bearer"
			  },
			  Scheme = "oauth2",
			  Name = "Bearer",
			  In = ParameterLocation.Header,
			},
			new List<string>()
		  }
		});
});



builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer("name=SettingEtiquetas"));
builder.Services.AddTransient<SeedDb>();

builder.Services.AddScoped(typeof(IGenericUnitOfWork<>), typeof(GenericUnitOfWork<>));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

//Country Repository-UnitOfWork
builder.Services.AddScoped<ICountriesReposity, CountryRepository>();
builder.Services.AddScoped<ICountriesUnitOfWork, CountriesUnitOfWork>();

//State Repository-UnitOfWork
builder.Services.AddScoped<IStateRepository, StateRepository>();	
builder.Services.AddScoped<IStatesUnitOfWork, StatesUnitOfWork>();

//Cities Repository-UniOfWork
builder.Services.AddScoped<ICitiesRepository, CitiesRepository>();
builder.Services.AddScoped<ICitiesUnitOfWork, CitiesUnitOfWork>();


// 1.- Inyecto el servicio de Repositorio de  usuario.
builder.Services.AddScoped<IUserRepository, UserRepository>();

// 2.- Inyecto el servicio de Unit of Work.
builder.Services.AddScoped<IUserUnitsOfWork, UserUnitOfWork>();

builder.Services.AddIdentity<User, IdentityRole>(x =>
{
	x.User.RequireUniqueEmail = true;
	x.Password.RequireDigit = false;
	x.Password.RequiredUniqueChars = 0;
	x.Password.RequireLowercase = false;
	x.Password.RequireNonAlphanumeric = false;
	x.Password.RequireUppercase = false;
})
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders();




var app = builder.Build();

//Inyeccion de seeder de carga de datos.
SeedData(app);

#pragma warning disable IDE0062 // Convertir la función local "static"
void SeedData(WebApplication app) 
{
	var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

	using var scope = scopedFactory!.CreateScope();
	var service = scope.ServiceProvider.GetService<SeedDb>();
	service!.SeedAsync().Wait();
}
#pragma warning restore IDE0062 // Convertir la función local "static"

//Configuracion de los CORS.
app.UseCors(x => x
	.AllowAnyMethod()
	.AllowAnyHeader()
	.SetIsOriginAllowed(origin => true)
	.AllowCredentials()
);



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
