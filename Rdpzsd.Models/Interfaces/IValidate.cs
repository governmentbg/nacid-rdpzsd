using Infrastructure.DomainValidation;

namespace Rdpzsd.Models.Interfaces
{
	public interface IValidate
	{
		void ValidateProperties(RdpzsdDbContext context, DomainValidatorService domainValidatorService);
	}
}
