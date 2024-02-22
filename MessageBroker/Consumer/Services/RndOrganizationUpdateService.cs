using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Enums;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Services.EntityServices;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBroker.Consumer.Services
{
    public class RndOrganizationUpdateService
    {
        private readonly RdpzsdDbContext context;

        public RndOrganizationUpdateService(
            RdpzsdDbContext context
            )
        {
            this.context = context;
        }

        public async Task UpdateOrganization(Institution institutionForUpdate)
        {
            if (Enum.IsDefined(typeof(OrganizationType), institutionForUpdate.OrganizationType) && institutionForUpdate.Level < Level.Third) {
                var institution = await context.Institutions
                    .Include(e => e.InstitutionSpecialities)
                        .ThenInclude(s => s.OrganizationSpecialityLanguages)
                    .Include(e => e.InstitutionSpecialities)
                        .ThenInclude(s => s.InstitutionSpecialityJointSpecialities)
                    .SingleOrDefaultAsync(e => e.Id == institutionForUpdate.Id);

                EntityService.ClearSkipProperties(institutionForUpdate);

                if (institution != null)
                {
                    EntityService.Update(institution, institutionForUpdate, context);
                    await context.SaveChangesAsync();
                }
                else
                {
                    await context.Institutions.AddAsync(institutionForUpdate);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
