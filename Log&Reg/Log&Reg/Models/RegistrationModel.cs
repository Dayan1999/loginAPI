using System.ComponentModel.DataAnnotations;

namespace Log_Reg.Models
{
	public class RegistrationModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[MinLength(6)]
		public string Password { get; set; }

		[Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
		public string ConfirmPassword { get; set; }

		public DateTime CreatedDate { get; set; }
	}
}
