using Infrastructure.DomainValidation.Models.ErrorCode;

namespace Infrastructure.DomainValidation.Models
{
	public class DomainErrorMessage
	{
		public SystemErrorCode DomainErrorCode { get; set; }

		public const string Type = "danger";

		public string ErrorText { get; set; }
		public int TimeoutSeconds { get; set; } = 15;

		public DomainErrorMessage(SystemErrorCode domainErrorCode, string errorText = null)
		{
			DomainErrorCode = domainErrorCode;
			ErrorText = errorText;
		}
	}
}
