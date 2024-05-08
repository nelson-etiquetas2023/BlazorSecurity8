using System.ComponentModel.DataAnnotations;

namespace SharedBlazorSecurity.DTOs
{
	public class LoginDTO
	{
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
		[EmailAddress(ErrorMessage = "Debes ingresar un correo válido.")]
		public string Email { get; set; } = null!;

		[DataType(DataType.Password)]
		[Display(Name = "Contraseña")]
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
		[StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener entre {2} y {1} carácteres.")]
		public string Password { get; set; } = null!;

		[Compare("Password", ErrorMessage = "La nueva contraseña y la confirmación no son iguales.")]
		[DataType(DataType.Password)]
		[Display(Name = "Conifrmacion de la contraseña")]
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
		[StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener entre {2} y {1} carácteres.")]
		public string ConfirmPassword { get; set; } = null!;

        public string Token { get; set; } = null!;
    }
}
