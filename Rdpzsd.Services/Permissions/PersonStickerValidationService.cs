using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using System;
using System.Linq;

namespace Rdpzsd.Services.Permissions
{
    public class PersonStickerValidationService
    {
        private readonly DomainValidatorService domainValidatorService;

        public PersonStickerValidationService(
            DomainValidatorService domainValidatorService
        )
        {
            this.domainValidatorService = domainValidatorService;
        }

        public void VerifyStickerState(StudentStickerState stickerState, SystemErrorCode stickerError, params StudentStickerState[] acceptedStickerStates)
        {
            if (!acceptedStickerStates.Any(e => e == stickerState))
            {
                domainValidatorService.ThrowErrorMessage(stickerError);
            }
        }

        public void VerifyStickerYear(int? stickerYear)
        {
            if (!stickerYear.HasValue || stickerYear < 2009 || stickerYear > DateTime.Now.Year)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentSticker_InvalidYear);
            }
        }
    }
}
