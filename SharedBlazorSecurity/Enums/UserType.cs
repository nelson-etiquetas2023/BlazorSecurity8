
using System.ComponentModel;

namespace SharedBlazorSecurity.Enums
{
	public enum UserType
	{
		[Description("Administrador")]
		Admin,
		[Description("Usuario")]
		User
	}
}
