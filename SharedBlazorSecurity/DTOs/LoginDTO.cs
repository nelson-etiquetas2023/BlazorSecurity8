﻿using System.ComponentModel.DataAnnotations;

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
		[MinLength(6, ErrorMessage = "El campo {0} debe teneral menos {1} caracteres.")]
		public string Password { get; set; } = null!;
    }
}
