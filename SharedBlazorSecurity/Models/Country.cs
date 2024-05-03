using System.ComponentModel.DataAnnotations;
using SharedBlazorSecurity.Interfaces;

namespace SharedBlazorSecurity.Models
{
	public class Country : IEntityWithName
	{
		
		public int id { get; set; }

		[Display(Name = "País")]
		[MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
		[Required(ErrorMessage = "El campo {0} es requerido.")]
		public string name { get; set; } = null!;

		public ICollection<State>? states { get; set; }

		[Display(Name = "Departamentos / Estados")]
		public int StatesNumber => states == null || states.Count == 0 ? 0 : states.Count;
	}
}