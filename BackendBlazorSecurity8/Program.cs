using BackendBlazorSecurity8.Data;
using BackendBlazorSecurity8.Repositories.Implementations;
using BackendBlazorSecurity8.Repositories.Interfaces;
using BackendBlazorSecurity8.UnitsOfWork.Implementations;
using BackendBlazorSecurity8.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Identity;
using SharedBlazorSecurity.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer("name=SettingEtiquetas"));

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
