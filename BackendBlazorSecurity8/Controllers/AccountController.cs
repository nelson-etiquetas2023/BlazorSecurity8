using BackendBlazorSecurity8.Helpers;
using BackendBlazorSecurity8.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SharedBlazorSecurity.DTOs;
using SharedBlazorSecurity.Models;
using SharedBlazorSecurity.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

#pragma warning disable IDE0290 // Usar constructor principal
namespace BackendBlazorSecurity8.Controllers
{
	[ApiController]
	[Route("Api/[controller]")]
	public class AccountController : ControllerBase
	{
        private readonly IUserUnitsOfWork _usersUnitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMailHelper _mailHelper;


        public AccountController(IUserUnitsOfWork usersUnitOfWork, IConfiguration configuration, IMailHelper mailHelper)

        {
            _usersUnitOfWork = usersUnitOfWork;
            _configuration = configuration;
            _mailHelper = mailHelper;
		}

        [HttpPost("changePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDTO model) 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var user = await _usersUnitOfWork.GetUserAsync(User.Identity!.Name!);
            if (user == null) 
            {
                return NotFound();
            }

            var result = await _usersUnitOfWork.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded) 
            {
                return BadRequest(result.Errors.FirstOrDefault()!.Description);
            }

            return NoContent();



        }



        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutAsync(User user) 
        {
            try
            {
                var currentUser = await _usersUnitOfWork.GetUserAsync(User.Identity!.Name!);

                if (currentUser == null) 
                {
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(user.Photo)) 
                {
                    var photoUser = Convert.FromBase64String(user.Photo);
                    //user.Photo = await _fileStorage.SaveFileAsync(photoUser, ".jpg", _container);
                }

                currentUser.Document = user.Document;
                currentUser.FirstName = user.FirstName;
                currentUser.LastName = user.LastName;
                currentUser.Address = user.Address;
                currentUser.PhoneNumber = user.PhoneNumber;
                currentUser.Photo = !string.IsNullOrEmpty(user.Photo) && user.Photo != currentUser.Photo ? 
                    user.Photo : currentUser.Photo;
                currentUser.CityId = user.CityId;

                var result = await _usersUnitOfWork.UpdateUserAsync(currentUser);
                if (result.Succeeded) 
                {
                    return Ok(BuildToken(currentUser));
                }

                return BadRequest(result.Errors.FirstOrDefault());

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _usersUnitOfWork.GetUserAsync(User.Identity!.Name!));
        }

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

		[HttpPost("LoginUser")]
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

		[HttpPost("RecoveryPassword")]
        public async Task<IActionResult> RecoveryPasswordAsync([FromBody] EmailDTO model) 
        {
            var user = await _usersUnitOfWork.GetUserAsync(model.Email);
            if (user == null) 
            {
                return NotFound();
            }

            var myToken = await _usersUnitOfWork.GeneratePasswordResetTokenAsync(user);
            var tokenLink = Url.Action("ResetPassword", "accounts", new { 
                userId = user.Id,
                token = myToken
            }, HttpContext.Request.Scheme, _configuration["Url Frontend"]);

            var response = _mailHelper.SendMail(user.FullName, user.Email!,
                $"Orders - Recuperación de Contraseña",
                $"<h1>Orders - Recuperacion de contraseña</h1>" +
                $"<p>Para recuperar su contraseña, por favor hacer click 'Recuperar Contraseña':</p>" +
                $"<b><a href={tokenLink}>Recuperar Contraseña</a></b>");

            if (response.WasSuccess) 
            {
                return NoContent();
            }

            return BadRequest(response.Message);

        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordDTO model) 
        {
            var user = await _usersUnitOfWork.GetUserAsync(model.Email);
            if (user == null) 
            {
                return NotFound();
            }

            var result = await _usersUnitOfWork.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded) 
            {
                return NoContent(); 
            }

            return BadRequest(result.Errors.FirstOrDefault()!.Description);
        }

        [HttpPost("ResedTokenAsync")]
        public async Task<IActionResult> ResendTokenAsync([FromBody] EmailDTO model) 
        {
            var user = await _usersUnitOfWork.GetUserAsync(model.Email);
            if (user == null) 
            {
                return NotFound();
            }
            var response = await SendConfirmationEmailAsync(user);
            if (response.WasSuccess) 
            {
                return NoContent();
            }
            return BadRequest(response.Message);
        }
   
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string token) 
        {
			token = token.Replace(" ", "+");
            var userGuid = new Guid(userId).ToString();
			var user = await _usersUnitOfWork.GetUserAsync(userGuid);
			if (user == null)
			{
				return NotFound();
			}

			var result = await _usersUnitOfWork.ConfirmEmailAsync(user, token);
			if (!result.Succeeded)
			{
				return BadRequest(result.Errors.FirstOrDefault());
			}

			return NoContent();
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
		private async Task<ActionResponse<string>> SendConfirmationEmailAsync(User user)
		{
			var myToken = await _usersUnitOfWork.GenerateEmailConfirmationTokenAsync(user);
			var tokenLink = Url.Action("ConfirmEmail", "accounts", new
			{
				userId = user.Id,
				token = myToken
			}, HttpContext.Request.Scheme, _configuration["Url Frontend"]);

			return _mailHelper.SendMail(user.FullName, user.Email!,
				$"Orders - Confirmación de cuenta",
				$"<h1>Orders - Confirmación de cuenta</h1>" +
				$"<p>Para habilitar el usuario, por favor hacer clic 'Confirmar Email':</p>" +
				$"<b><a href ={tokenLink}>Confirmar Email</a></b>");
		}
	}
}
