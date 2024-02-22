using System.Collections.Generic;

namespace Infrastructure.Integrations.EmsIntegration.Dtos
{
	public class UserEmsFilterDto
	{
		public int Offset { get; set; } = 0;
		public int Limit { get; set; } = int.MaxValue;
		public bool ReturnAllUsers { get; set; } = true;
		public List<int> UserIds { get; set; } = new List<int>();
	}
}
