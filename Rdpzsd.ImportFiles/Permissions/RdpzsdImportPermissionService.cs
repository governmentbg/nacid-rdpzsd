using Infrastructure.Constants;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Infrastructure.User;
using Infrastructure.User.Enums;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Enums.RdpzsdImportFile;
using System.Linq;
using System.Threading.Tasks;

namespace Rdpzsd.Import.Permissions
{
    public class RdpzsdImportPermissionService
    {
        private readonly UserContext userContext;
        private readonly DomainValidatorService domainValidatorService;
        private readonly RdpzsdDbContext context;

        public RdpzsdImportPermissionService(
            UserContext userContext,
            DomainValidatorService domainValidatorService,
            RdpzsdDbContext context
        )
        {
            this.userContext = userContext;
            this.domainValidatorService = domainValidatorService;
            this.context = context;
        }

        public void VerifyReadPermissions(int rdpzsdImportInstitutionId, int? rdpzsdImportSubordinateId)
        {
            if (userContext.UserType != UserType.Rsd && (userContext.UserType != UserType.Ems || !userContext.Permissions.Contains(PermissionConstants.RdpzsdRead)))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_NotEnoughPermissions);
            }

            if (userContext.UserType == UserType.Rsd)
            {
                if (rdpzsdImportSubordinateId.HasValue
                    && !userContext.Institution.ChildInstitutions.Select(s => s.Id).ToList().Contains(rdpzsdImportSubordinateId.Value))
                {
                    domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_NotEnoughPermissions);
                }
                else if (userContext.Institution.Id != rdpzsdImportInstitutionId)
                {
                    domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_NotEnoughPermissions);
                }
            }
        }

        public void VerifyEditPermissions(int userId)
        {
            if (userContext.UserType != UserType.Rsd || userId != userContext.UserId)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_NotEnoughPermissions);
            }
        }

        public void VerifyState(ImportState currentState, params ImportState[] states)
        {
            if (!states.Contains(currentState))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.RdpzsdImport_WrongState);
            }
        }

        public async Task VerifyUniqueSpecialityImportInstitution()
        {
            var institutionId = userContext.Institution?.Id;

            if (userContext.Institution?.ChildInstitutions?.Count == 1)
            {
                var subordinateId = userContext.Institution.ChildInstitutions.First().Id;

                if (await context.SpecialityImports
                    .AsNoTracking()
                    .AnyAsync(e => e.InstitutionId == institutionId 
                        && e.SubordinateId == subordinateId
                        && e.State != ImportState.Deleted 
                        && e.State != ImportState.Registered))
                {
                    domainValidatorService.ThrowErrorMessage(SystemErrorCode.RdpzsdImport_SpecialityImportInProgress);
                }
            } 
            else
            {
                if (await context.SpecialityImports
                    .AsNoTracking()
                    .AnyAsync(e => e.InstitutionId == institutionId
                        && e.SubordinateId == null
                        && e.State != ImportState.Deleted 
                        && e.State != ImportState.Registered))
                {
                    domainValidatorService.ThrowErrorMessage(SystemErrorCode.RdpzsdImport_SpecialityImportInProgress);
                }
            }
        }
    }
}
