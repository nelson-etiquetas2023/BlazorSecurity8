using BackendBlazorSecurity8.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SharedBlazorSecurity.DTOs;
using SharedBlazorSecurity.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackendBlazorSecurity8.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController(IUserUnitsOfWork usersUnitOfWork, IConfiguration configuration) : ControllerBase
	{
        private readonly IUserUnitsOfWork _usersUnitOfWork = usersUnitOfWork;
        private readonly IConfiguration _configuration = configuration;

		[HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO model) 
        {
            User user = model;
            var result = await _usersUnitOfWork.AddUserAsync(user, model.Password);
            if (result.Succeeded) 
            {
                await _usersUnitOfWork.AddUserToRoleAsync(user, user.UserType.ToString());
                return Ok(BuildToken(user));
            }

            return BadRequest(result.Errors.FirstOrDefault());
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO model) 
        {
            var result = await _usersUnitOfWork.LoginAsync(model);
            if (result.Succeeded) 
            {
                var user = await _usersUnitOfWork.GetUserAsync(model.Email);
                return Ok(BuildToken(user));
            }

            return BadRequest("Email o contraseña incorrectos");
        }


        private TokenDTO BuildToken(User user) 
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Email!),
                new(ClaimTypes.Role, user.UserType.ToString()),
                new("Document", user.Document),
                new("FirstName", user.FirstName),
                new("LastName", user.LastName),
                new("Address", user.Address),
                new("Photo", user.Photo ?? string.Empty)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtkey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddDays(30);
            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new TokenDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
