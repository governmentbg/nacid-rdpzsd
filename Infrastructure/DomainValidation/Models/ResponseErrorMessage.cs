using System.Collections.Generic;

namespace Infrastructure.DomainValidation.Models
{
	public class ResponseErrorMessage
	{
		public string Status { get; set; }

		public List<DomainErrorMessage> ErrorMessages { get; set; } = new List<DomainErrorMessage>();
	}
}
