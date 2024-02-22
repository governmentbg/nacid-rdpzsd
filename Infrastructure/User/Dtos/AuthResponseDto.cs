namespace Infrastructure.User.Dtos
{
	public class AuthResponseDto
	{
		public int Id { get; set; }
		public string UserFullname { get; set; }
		public string Permissions { get; set; }
		public string UserType { get; set; }
	}
}
