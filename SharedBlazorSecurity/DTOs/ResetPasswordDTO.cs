using System.ComponentModel.DataAnnotations;


namespace SharedBlazorSecurity.DTOs
{
	public class ResetPasswordDTO
	{

		[Display(Name = "Email")]
		[EmailAddress(ErrorMessage = "Debes Ingresar un correo Valido")]
		[Required(ErrorMessage = "El compo {0} es Obligatorio.")]
		public string Email { get; set; } = null!;

		[DataType(DataType.Password)]
		[Display(Name = "Contraseña")]
		[Required(ErrorMessage = "El compo {0} es Obligatorio.")]
		[StringLength(20,MinimumLength = 6, ErrorMessage = "El campo {0} debe tener entre {2} y {1}")]
		public string Password { get; set; } = null!;

		[Compare("Password", ErrorMessage = "la nueva contraseña y la confirmacion non son iguales")]
		[DataType(DataType.Password)]
		[Display(Name = "Confirmacion de la contraseña")]
		[Required(ErrorMessage = "El compo {0} es Obligatorio.")]
		[StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener entre {2} y {1}")]
		public string ConfirPassword { get; set; } = null!;

        public string Token { get; set; } = null!;


    }
}
