using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Services.EntityServices;
using System.Threading.Tasks;

namespace MessageBroker.Consumer.Services
{
    public class RndNsiUpdateService
    {
        private readonly RdpzsdDbContext context;

        public RndNsiUpdateService(
            RdpzsdDbContext context
            )
        {
            this.context = context;
        }

        public async Task UpdateNsi(NationalStatisticalInstitute nsiForUpdate)
        {
            var nsi = await context.NationalStatisticalInstitutes.SingleOrDefaultAsync(e => e.Id == nsiForUpdate.Id);

            if (nsi != null)
            {
                EntityService.Update(nsi, nsiForUpdate, context);
                await context.SaveChangesAsync();
            }
            else
            {
                EntityService.ClearSkipProperties(nsiForUpdate);
                await context.NationalStatisticalInstitutes.AddAsync(nsiForUpdate);
                await context.SaveChangesAsync();
            }
        }
    }
}
