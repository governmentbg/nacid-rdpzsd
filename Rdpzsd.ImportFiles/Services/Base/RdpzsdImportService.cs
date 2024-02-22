using FileStorageNetCore.Api;
using Infrastructure.DomainValidation;
using Infrastructure.ExcelProcessor.Services;
using Infrastructure.Integrations.EmsIntegration;
using Infrastructure.User;
using Infrastructure.User.Enums;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Enums.RdpzsdImportFile;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Base;
using Rdpzsd.Models.Models.RdpzsdImports.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rdpzsd.Import.Services.Base
{
    public abstract class RdpzsdImportService<TRdpzsdImport, TFile, TErrorFile, TImportHistory, TImportHistoryFile, TImportHistoryErrorFile>
        where TRdpzsdImport : RdpzsdImport<TFile, TErrorFile, TImportHistory, TImportHistoryFile, TImportHistoryErrorFile>, IIncludeAll<TRdpzsdImport>, new()
        where TFile : RdpzsdAttachedFile
        where TErrorFile : RdpzsdAttachedFile, new()
        where TImportHistory : RdpzsdImportHistory<TImportHistoryFile, TImportHistoryErrorFile>, new()
        where TImportHistoryFile : RdpzsdAttachedFile, new()
        where TImportHistoryErrorFile : RdpzsdAttachedFile, new()
    {
        protected readonly RdpzsdDbContext context;
        protected readonly UserContext userContext;
        protected readonly DomainValidatorService domainValidatorService;
        protected readonly BlobStorageService blobStorageService;
        protected readonly EmsIntegrationService emsIntegrationService;
        protected readonly ExcelProcessorService excelProcessorService;

        public RdpzsdImportService(
           RdpzsdDbContext context,
           UserContext userContext,
           DomainValidatorService domainValidatorService,
           BlobStorageService blobStorageService,
           EmsIntegrationService emsIntegrationService,
           ExcelProcessorService excelProcessorService
        )
        {
            this.context = context;
            this.userContext = userContext;
            this.domainValidatorService = domainValidatorService;
            this.blobStorageService = blobStorageService;
            this.emsIntegrationService = emsIntegrationService;
            this.excelProcessorService = excelProcessorService;
        }

        public async virtual Task<TRdpzsdImport> Get(int id)
        {
            var result = await new TRdpzsdImport().IncludeAll(context.Set<TRdpzsdImport>().AsQueryable())
                       .SingleAsync(e => e.Id == id);

            result.ImportHistories = result.ImportHistories
                .OrderByDescending(e => e.CreateDate)
                .ToList();

            return result;
        }

        public async virtual Task<TRdpzsdImport> GetFirstByState(ImportState importState)
        {
            var result = await new TRdpzsdImport().IncludeAll(context.Set<TRdpzsdImport>().AsQueryable())
                       .FirstOrDefaultAsync(e => e.State == importState);

            return result;
        }

        public async virtual Task<bool> HasEntityWithState(ImportState importState)
        {
            var hasEntity = await context.Set<TRdpzsdImport>()
                .AsNoTracking()
                .AnyAsync(e => e.State == importState);

            return hasEntity;
        }

        public async virtual Task<TRdpzsdImport> GetWithoutIncludes(int id)
        {
            var result = await context.Set<TRdpzsdImport>().AsQueryable()
                       .SingleAsync(e => e.Id == id);

            return result;
        }

        public async Task<TRdpzsdImport> Create(TRdpzsdImport rdpzsdImport)
        {
            rdpzsdImport.ValidateProperties(context, domainValidatorService);

            var emsUsers = await emsIntegrationService.GetUsersInfo(new List<int> { userContext.UserId.Value });
            var currentUser = emsUsers.Single(e => e.Id == userContext.UserId.Value);

            var newRdpzsdImport = new TRdpzsdImport
            {
                CreateDate = DateTime.Now,
                ImportFile = rdpzsdImport.ImportFile,
                InstitutionId = userContext.Institution.Id,
                State = ImportState.Draft,
                SubordinateId = userContext.UserType == UserType.Rsd && userContext.Institution.ChildInstitutions.Count == 1
                    ? userContext.Institution.ChildInstitutions.First().Id
                    : null,
                UserId = userContext.UserId.Value,
                UserFullname = userContext.UserFullname,
                UserEmail = currentUser.Email
            };

            await context.Set<TRdpzsdImport>().AddAsync(newRdpzsdImport);
            await context.SaveChangesAsync();

            return await Get(newRdpzsdImport.Id);
        }

        public async Task<TRdpzsdImport> ChangeImportFile(TFile importFile, TRdpzsdImport rdpzsdImport)
        {
            var importHistories = new TImportHistory
            {
                CreateDate = rdpzsdImport.CreateDate,
                ErrorFile = rdpzsdImport.ErrorFile != null ? new TImportHistoryErrorFile
                {
                    DbId = rdpzsdImport.ErrorFile.DbId,
                    Hash = rdpzsdImport.ErrorFile.Hash,
                    Key = rdpzsdImport.ErrorFile.Key,
                    MimeType = rdpzsdImport.ErrorFile.MimeType,
                    Name = rdpzsdImport.ErrorFile.Name,
                    Size = rdpzsdImport.ErrorFile.Size
                } : null,
                ImportFile = new TImportHistoryFile
                {
                    DbId = rdpzsdImport.ImportFile.DbId,
                    Hash = rdpzsdImport.ImportFile.Hash,
                    Key = rdpzsdImport.ImportFile.Key,
                    MimeType = rdpzsdImport.ImportFile.MimeType,
                    Name = rdpzsdImport.ImportFile.Name,
                    Size = rdpzsdImport.ImportFile.Size
                },
                State = rdpzsdImport.State
            };

            rdpzsdImport.CreateDate = DateTime.Now;
            rdpzsdImport.ErrorFile = null;
            rdpzsdImport.ImportFile = importFile;
            rdpzsdImport.ImportHistories.Add(importHistories);

            await ChangeState(rdpzsdImport, ImportState.Draft, null, null, null, null, null, null);

            return rdpzsdImport;
        }

        public async Task<TRdpzsdImport> DeleteImport(TRdpzsdImport rdpzsdImport)
        {
            await ChangeState(rdpzsdImport, ImportState.Deleted, 
                rdpzsdImport.EntitiesCount, rdpzsdImport.EntitiesAcceptedCount,
                rdpzsdImport.FirstCriteriaCount, rdpzsdImport.FirstCriteriaAcceptedCount,
                rdpzsdImport.SecondCriteriaCount, rdpzsdImport.SecondCriteriaAcceptedCount);
            return await Get(rdpzsdImport.Id);
        }

        public async Task CreateErrorFile(MemoryStream memoryStream, TRdpzsdImport rdpzsdImport, 
            int entitiesCount, int entitiesAcceptedCount,
            int? firstCriteriaCount, int? firstCriteriaAcceptedCount,
            int? secondCriteriaCount, int? secondCriteriaAcceptedCount)
        {
            string fileName = $"Грешка_{rdpzsdImport.ImportFile?.Name?.Replace(".txt", "")}.xlsx";
            var errorAttachedFile = await blobStorageService.Post(memoryStream, fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            rdpzsdImport.ErrorFile = new TErrorFile
            {
                DbId = errorAttachedFile.DbId,
                Hash = errorAttachedFile.Hash,
                Key = errorAttachedFile.Key,
                MimeType = errorAttachedFile.MimeType,
                Name = errorAttachedFile.Name,
                Size = errorAttachedFile.Size
            };

            await ChangeState(rdpzsdImport, ImportState.Error, entitiesCount, entitiesAcceptedCount, firstCriteriaCount, firstCriteriaAcceptedCount, secondCriteriaCount, secondCriteriaAcceptedCount);
        }

        public async Task ChangeState(TRdpzsdImport rdpzsdImport, ImportState state, 
            int? entitiesCount, int? entitiesAcceptedCount,
            int? firstCriteriaCount, int? firstCriteriaAcceptedCount,
            int? secondCriteriaCount, int? secondCriteriaAcceptedCount,
            DateTime? finishDate = null)
        {
            rdpzsdImport.State = state;
            rdpzsdImport.EntitiesCount = entitiesCount;
            rdpzsdImport.EntitiesAcceptedCount = entitiesAcceptedCount;
            rdpzsdImport.FirstCriteriaCount = firstCriteriaCount;
            rdpzsdImport.FirstCriteriaAcceptedCount = firstCriteriaAcceptedCount;
            rdpzsdImport.SecondCriteriaCount = secondCriteriaCount;
            rdpzsdImport.SecondCriteriaAcceptedCount = secondCriteriaAcceptedCount;
            rdpzsdImport.FinishDate = finishDate;

            await context.SaveChangesAsync();
        }

        public async Task<byte[]> GetFileBytes<TRdpzsdFile>(int id)
            where TRdpzsdFile : RdpzsdAttachedFile
        {
            var rdpzsdFile = await context.Set<TRdpzsdFile>().AsQueryable()
                       .AsNoTracking()
                       .SingleAsync(e => e.Id == id);

            var fileBytes = await blobStorageService.GetBytes(rdpzsdFile.Key, rdpzsdFile.DbId);

            return fileBytes;
        }
    }
}
