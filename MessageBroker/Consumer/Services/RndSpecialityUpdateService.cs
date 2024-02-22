using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Services.EntityServices;
using System.Threading.Tasks;

namespace MessageBroker.Consumer.Services
{
    public class RndSpecialityUpdateService
    {
        private readonly RdpzsdDbContext context;

        public RndSpecialityUpdateService(
            RdpzsdDbContext context
            )
        {
            this.context = context;
        }

        public async Task UpdateSpeciality(Speciality specialityForUpdate)
        {
            var speciality = await context.Specialities.SingleOrDefaultAsync(e => e.Id == specialityForUpdate.Id);

            if (speciality != null)
            {
                EntityService.Update(speciality, specialityForUpdate, context);
                await context.SaveChangesAsync();
            }
            else
            {
                EntityService.ClearSkipProperties(specialityForUpdate);
                await context.Specialities.AddAsync(specialityForUpdate);
                await context.SaveChangesAsync();
            }
        }
    }
}
