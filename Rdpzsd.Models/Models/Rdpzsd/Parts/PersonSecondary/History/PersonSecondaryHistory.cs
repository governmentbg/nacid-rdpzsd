using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Models.Rdpzsd.Interfaces;
using Rdpzsd.Models.Models.Rdpzsd.Parts.PersonSecondary.Base;
using System;
using System.Linq;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.PersonSecondary.History
{
    public class PersonSecondaryHistory : BasePersonSecondary<PersonSecondaryHistoryInfo>, IHistoryPart<PersonSecondaryHistory>
    {
        public int PartId { get; set; }
		public PersonSecondaryRecognitionDocumentHistory PersonSecondaryRecognitionDocument { get; set; }
		public IQueryable<PersonSecondaryHistory> IncludeAll(IQueryable<PersonSecondaryHistory> query)
        {
            return query
                .Include(e => e.Country)
                .Include(e => e.School.Settlement)
                .Include(e => e.PersonSecondaryRecognitionDocument);
        }

        public class PersonSecondaryHistoryConfiguration : IEntityTypeConfiguration<PersonSecondaryHistory>
        {
            public void Configure(EntityTypeBuilder<PersonSecondaryHistory> builder)
            {
                builder.Property(e => e.DiplomaNumber)
                     .HasMaxLength(100);

                builder.Property(e => e.ForeignSchoolName)
                       .HasMaxLength(100);

                builder.Property(e => e.Profession)
                       .HasMaxLength(100);
            }
        }
    }
}
