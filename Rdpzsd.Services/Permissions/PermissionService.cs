using Infrastructure.Constants;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Infrastructure.User;
using Infrastructure.User.Enums;
using System.Linq;

namespace Rdpzsd.Services.Permissions
{
    public class PermissionService
    {
		private readonly UserContext userContext;
		private readonly DomainValidatorService domainValidatorService;

		public PermissionService(
			UserContext userContext,
			DomainValidatorService domainValidatorService
		)
		{
			this.userContext = userContext;
			this.domainValidatorService = domainValidatorService;
		}


		public void VerifyInternalUser(params string[] permission)
		{
			if (userContext.UserType != UserType.Ems || !userContext.Permissions.Intersect(permission).Any())
			{
				domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_NotEnoughPermissions);
			}
		}

		public void VerifyEditPermissions()
        {
			if (userContext.UserType != UserType.Rsd && (userContext.UserType != UserType.Ems || !userContext.Permissions.Contains(PermissionConstants.RdpzsdEdit)))
			{
				domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_NotEnoughPermissions);
			}
		}

		public void VerifyRsdUser()
		{
			if (userContext.UserType != UserType.Rsd)
			{
				domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_NotEnoughPermissions);
			}
		}

		public void VerifyInternalRsdUserPermissions(params string[] permission)
        {
			if (userContext.UserType != UserType.Rsd && !userContext.Permissions.Intersect(permission).Any())
			{
				domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_NotEnoughPermissions);
			}
		}
	}
}
