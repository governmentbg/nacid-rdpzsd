using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Enums.RdpzsdImportFile;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Base;
using Rdpzsd.Models.Models.Nomenclatures;
using System;
using System.Collections.Generic;

namespace Rdpzsd.Models.Models.RdpzsdImports.Base
{
    public abstract class RdpzsdImport<TFile, TErrorFile, TImportHistory, TImportHistoryFile, TImportHistoryErrorFile> : EntityVersion, IValidate
        where TFile : RdpzsdAttachedFile
        where TErrorFile : RdpzsdAttachedFile
        where TImportHistory : RdpzsdImportHistory<TImportHistoryFile, TImportHistoryErrorFile>
        where TImportHistoryFile : RdpzsdAttachedFile
        where TImportHistoryErrorFile : RdpzsdAttachedFile
    {
        public ImportState State { get; set; }

        public int UserId { get; set; }
        public string UserFullname { get; set; }
        public string UserEmail { get; set; }

        public int InstitutionId { get; set; }
        [Skip]
        public Institution Institution { get; set; }
        public int? SubordinateId { get; set; }
        [Skip]
        public Institution Subordinate { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? FinishDate { get; set; }

        // All items
        public int? EntitiesAcceptedCount { get; set; }
        public int? EntitiesCount { get; set; }
        // Custom count items
        public int? FirstCriteriaAcceptedCount { get; set; }
        public int? FirstCriteriaCount { get; set; }
        public int? SecondCriteriaAcceptedCount { get; set; }
        public int? SecondCriteriaCount { get; set; }

        public TFile ImportFile { get; set; }
        public TErrorFile ErrorFile { get; set; }

        public List<TImportHistory> ImportHistories { get; set; } = new List<TImportHistory>();

        public void ValidateProperties(RdpzsdDbContext context, DomainValidatorService domainValidatorService)
        {
            if (ImportFile == null || string.IsNullOrWhiteSpace(ImportFile.Name))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.FileUpload_PleaseAttachFile);
            }
        }
    }
}
