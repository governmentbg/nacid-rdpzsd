using Infrastructure.User;
using Infrastructure.User.Enums;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Enums.RdpzsdImportFile;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Base;
using Rdpzsd.Models.Models.RdpzsdImports.Base;
using System.Linq;

namespace Rdpzsd.Models.Dtos.RdpzsdImports
{
    public class RdpzsdImportFilterDto<TRdpzsdImport, TFile, TErrorFile, TImportHistory, TImportHistoryFile, TImportHistoryErrorFile> : FilterDto, IWhere<TRdpzsdImport>
        where TRdpzsdImport : RdpzsdImport<TFile, TErrorFile, TImportHistory, TImportHistoryFile, TImportHistoryErrorFile>
        where TFile : RdpzsdAttachedFile
        where TErrorFile : RdpzsdAttachedFile
        where TImportHistory : RdpzsdImportHistory<TImportHistoryFile, TImportHistoryErrorFile>
        where TImportHistoryFile : RdpzsdAttachedFile
        where TImportHistoryErrorFile : RdpzsdAttachedFile
    {
        public ImportState? State { get; set; }
        public int? InstitutionId { get; set; }
        public int? SubordinateId { get; set; }

        public IQueryable<TRdpzsdImport> WhereBuilder(IQueryable<TRdpzsdImport> query, UserContext userContext, RdpzsdDbContext rdpzsdDbContext)
        {
            query = ConstructSearchPermissions(query, userContext);

            if (State.HasValue)
            {
                query = query.Where(e => e.State == State);
            } 
            //else
            //{
            //    query = query.Where(e => e.State != ImportState.Deleted);
            //}

            if (InstitutionId.HasValue)
            {
                query = query.Where(e => e.InstitutionId == InstitutionId);
            }

            if (SubordinateId.HasValue)
            {
                query = query.Where(e => e.SubordinateId == SubordinateId);
            }

            return query;
        }

        private IQueryable<TRdpzsdImport> ConstructSearchPermissions(IQueryable<TRdpzsdImport> query, UserContext userContext)
        {
            if (userContext.UserType == UserType.Rsd)
            {
                if (userContext.Institution.ChildInstitutions.Any())
                {
                    query = query.Where(e => e.SubordinateId.HasValue && userContext.Institution.ChildInstitutions.Select(s => s.Id).ToList().Contains(e.SubordinateId.Value));
                }
                else
                {
                    query = query.Where(e => e.InstitutionId == userContext.Institution.Id);
                }
            }

            return query;
        }
    }
}
