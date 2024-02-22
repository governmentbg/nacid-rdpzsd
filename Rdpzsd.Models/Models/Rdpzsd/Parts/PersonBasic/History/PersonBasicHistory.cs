using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Models.Rdpzsd.Interfaces;
using Rdpzsd.Models.Models.Rdpzsd.Parts.Base;
using System.Linq;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.History
{
    public class PersonBasicHistory : BasePersonBasic<PersonBasicHistoryInfo, PassportCopyHistory, PersonImageHistory>, IHistoryPart<PersonBasicHistory>
    {
        public int PartId { get; set; }

        public IQueryable<PersonBasicHistory> IncludeAll(IQueryable<PersonBasicHistory> query)
        {
            return query
                .Include(e => e.BirthCountry)
                .Include(e => e.BirthDistrict)
                .Include(e => e.BirthMunicipality)
                .Include(e => e.BirthSettlement)
                .Include(e => e.Citizenship)
                .Include(e => e.SecondCitizenship)
                .Include(e => e.ResidenceCountry)
                .Include(e => e.ResidenceDistrict)
                .Include(e => e.ResidenceMunicipality)
                .Include(e => e.ResidenceSettlement)
                .Include(e => e.PersonImage)
                .Include(e => e.PassportCopy);
        }
    }

    public class PersonBasicHistoryConfiguration : IEntityTypeConfiguration<PersonBasicHistory>
    {
        public void Configure(EntityTypeBuilder<PersonBasicHistory> builder)
        {
            builder.Property(e => e.Uin)
                .HasMaxLength(10)
                .IsFixedLength();

            builder.Property(e => e.ForeignerNumber)
                .HasMaxLength(10)
                .IsFixedLength();

            builder.Property(e => e.FirstName)
                .HasMaxLength(50);

            builder.Property(e => e.MiddleName)
                .HasMaxLength(50);

            builder.Property(e => e.LastName)
                .HasMaxLength(50);

            builder.Property(e => e.FirstNameAlt)
                .HasMaxLength(50);

            builder.Property(e => e.MiddleNameAlt)
                .HasMaxLength(50);

            builder.Property(e => e.LastNameAlt)
                .HasMaxLength(50);

            builder.Property(e => e.OtherNames)
                .HasMaxLength(100);

            builder.Property(e => e.OtherNamesAlt)
                .HasMaxLength(100);

            builder.Property(e => e.ForeignerBirthSettlement)
                .HasMaxLength(255);

            builder.Property(e => e.Email)
                .HasMaxLength(50);

            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(18);

            builder.Property(e => e.ResidenceAddress)
                .HasMaxLength(255);

            builder.Property(e => e.IdnNumber)
                .HasMaxLength(50);

            builder.Property(e => e.PostCode)
                .HasMaxLength(4);
        }
    }
}
