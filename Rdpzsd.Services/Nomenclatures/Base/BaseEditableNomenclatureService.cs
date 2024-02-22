using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Infrastructure.ExcelProcessor.Services;
using Infrastructure.User;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Nomenclatures;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Nomenclatures.Base;
using Rdpzsd.Services.EntityServices;
using System.Threading.Tasks;

namespace Rdpzsd.Services.Nomenclatures.Base
{
    public abstract class BaseEditableNomenclatureService<T, TFilter> : BaseNomenclatureService<T, TFilter>
        where T : Nomenclature, IIncludeAll<T>, new()
        where TFilter : NomenclatureFilterDto<T>, new()
    {
        protected readonly DomainValidatorService domainValidatorService;

        protected BaseEditableNomenclatureService(
            RdpzsdDbContext context,
            ExcelProcessorService excelProcessorService,
            UserContext userContext,
            DomainValidatorService domainValidatorService)
            : base(context, excelProcessorService, userContext)
        {
            this.domainValidatorService = domainValidatorService;
        }

        public async virtual Task<T> Create(T nomenclature)
        {
            EntityService.ClearSkipProperties(nomenclature);

            context.Set<T>().Add(nomenclature);
            await context.SaveChangesAsync();

            var createdEntity = await new T()
                .IncludeAll(context.Set<T>().AsNoTracking())
                .SingleOrDefaultAsync(e => e.Id == nomenclature.Id);

            return createdEntity;
        }

        public async virtual Task Delete(int id)
        {
            try
            {
                var entity = await new T()
                .IncludeAll(context.Set<T>())
                .SingleOrDefaultAsync(e => e.Id == id);

                EntityService.Remove(entity, context);
                await context.SaveChangesAsync();
            }

            // Catch error if nomenclature is in use and cannot be deleted
            catch (DbUpdateException)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.Nomenclature_CannotBeDeletedBecauseInUse);
            }
        }

        public async virtual Task<T> Update(T entity)
        {
            var original = await new T() 
                .IncludeAll(context.Set<T>())
                .SingleOrDefaultAsync(e => e.Id == entity.Id);

            if (original != null)
            {
                EntityService.Update(original, entity, context);
                await context.SaveChangesAsync();

                return entity;
            }

            return null;
        }
    }
}
