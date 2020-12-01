using TestTask.Identity.DAL.Entities;

namespace TestTask.Identity.BL.DTOs
{
    public class LoginResultDTO
	{
		public bool Status { get; set; }

		public User User { get; set; }
		public string Token { get; set; }

		public string Error { get; set; }
		public string ErrorDescription { get; set; }
	}
}
