using System;
using System.Collections.Generic;

namespace Infrastructure.DomainValidation.Models
{
	public class DomainErrorException : Exception
	{
		public List<DomainErrorMessage> ErrorMessages { get; set; } = new List<DomainErrorMessage>();

		public DomainErrorException(DomainErrorMessage errorMessage)
		{
			ErrorMessages.Add(errorMessage);
		}
	}
}
