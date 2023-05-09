using Log_Reg.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Log_Reg.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		private string connectionString = "Data Source=LAPTOP-06GAVR62\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True";

		[HttpPost]
		public ActionResult Post(LoginModel model)
		{
			// Check if email exists in the database
			if (!IsEmailExists(model.Email))
			{
				return BadRequest("Invalid email address or password.");
			}

			// Check if password is correct
			if (!IsPasswordCorrect(model.Email, model.Password))
			{
				return BadRequest("Invalid email address or password.");
			}

			return Ok("Login successful.");
		}

		private bool IsEmailExists(string email)
		{
			// Check if email exists in the database
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string query = "SELECT COUNT(*) FROM UserInf WHERE Email=@Email";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@Email", email);
					connection.Open();
					int count = (int)command.ExecuteScalar();
					return count > 0;
				}
			}
		}

		private bool IsPasswordCorrect(string email, string password)
		{
			// Check if password is correct for the given email
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string query = "SELECT COUNT(*) FROM UserInf WHERE Email=@Email AND Password=@Password";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@Email", email);
					command.Parameters.AddWithValue("@Password", password);
					connection.Open();
					int count = (int)command.ExecuteScalar();
					return count > 0;
				}
			}
		}
	}
}
