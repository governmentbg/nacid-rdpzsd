using Infrastructure.AppSettings;
using Infrastructure.DomainValidation.Models;
using Infrastructure.DomainValidation.Models.ErrorCode;

namespace Infrastructure.DomainValidation
{
	public class DomainValidatorService
	{
		public void ThrowErrorMessage(SystemErrorCode errorCode, string errorText = null)
		{
			throw new DomainErrorException(new DomainErrorMessage(errorCode, errorText));
		}

		public void ThrowFunctionalityNotSupportedError()
		{
			if (!AppSettingsProvider.EnableFullFunctionality) {
				throw new DomainErrorException(new DomainErrorMessage(SystemErrorCode.System_FunctionalityNotSupported, null));
			}
		}
	}
}
