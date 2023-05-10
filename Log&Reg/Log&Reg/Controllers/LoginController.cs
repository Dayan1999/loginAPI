using Log_Reg.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;
using System.Text;

namespace Log_Reg.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		private string connectionString = "Data Source=LAPTOP-06GAVR62\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True";

		// POST api/login
		[HttpPost]
		public ActionResult Post(LoginModel model)
		{
			// Check if email exists in the database
			if (!IsEmailExists(model.Email))
			{
				return BadRequest("Invalid email address.");
			}

			// Retrieve the password from the database
			string passwordHash = "";
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string query = "SELECT Password FROM UserInf WHERE Email=@Email";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@Email", model.Email);
					connection.Open();
					var result = command.ExecuteScalar();
					if (result != null)
					{
						passwordHash = result.ToString();
					}
				}
			}

			// Decrypt the password
			string password = "";
			if (!string.IsNullOrEmpty(passwordHash))
			{
				byte[] passwordBytes = Convert.FromBase64String(passwordHash);
				password = Encoding.UTF8.GetString(passwordBytes);
			}

			// Check if the password matches
			if (password != model.Password)
			{
				return BadRequest("Invalid password.");
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
	}
}
