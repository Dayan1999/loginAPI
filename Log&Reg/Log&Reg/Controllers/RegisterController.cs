using Log_Reg.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Log_Reg.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RegisterController : ControllerBase
	{
		private string connectionString = "Data Source=LAPTOP-06GAVR62\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True";

		// POST api/register
		[HttpPost]
		public ActionResult Post(RegistrationModel model)
		{
			// Validate email format and domain name
			if (!IsEmailValid(model.Email) || !IsDomainValid(model.Email))
			{
				return BadRequest("Invalid email address.");
			}

			// Validate password length and confirm password
			if (model.Password.Length < 6 || model.Password != model.ConfirmPassword)
			{
				return BadRequest("Password should be at least 6 characters long and match the confirm password.");
			}

			// Check if email already exists in the database
			if (IsEmailExists(model.Email))
			{
				return BadRequest("Email address already registered.");
			}

			// Set created date time
			model.CreatedDate = DateTime.UtcNow;

			// Insert data into database
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string query = "INSERT INTO UserInf (Email, Password, CreatedDateTime) VALUES (@Email, @Password, @CreatedDateTime)";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@Email", model.Email);
					command.Parameters.AddWithValue("@Password", model.Password);
					command.Parameters.AddWithValue("@CreatedDateTime", model.CreatedDate);
					connection.Open();
					int result = command.ExecuteNonQuery();
					if (result < 0)
					{
						return StatusCode(500, "An error occurred while inserting data into the database.");
					}
				}
			}

			return Ok("Account created successfully.");
		}

		private bool IsEmailValid(string email)
		{
			// TODO: Implement email format validation
			return true;
		}

		private bool IsDomainValid(string email)
		{
			// TODO: Implement email domain validation
			return true;
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
