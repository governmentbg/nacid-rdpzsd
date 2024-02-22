using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Rdpzsd.Parts.Base;
using System;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.History
{
    public class PersonDoctoralSemesterHistory : BasePersonSemester
    {
        public DateTime ProtocolDate { get; set; }
        public string ProtocolNumber { get; set; }
        public YearType YearType { get; set; }
        public AttestationType? AttestationType { get; set; }
        [Skip]
        public PersonDoctoral RelocatedFromPart { get; set; }
        public string SemesterRelocatedNumber { get; set; }
        public DateTime? SemesterRelocatedDate { get; set; }
        public PersonDoctoralSemesterRelocatedFileHistory SemesterRelocatedFile { get; set; }
	}

    public class PersonDoctoralSemesterHistoryConfiguration : IEntityTypeConfiguration<PersonDoctoralSemesterHistory>
    {
        public void Configure(EntityTypeBuilder<PersonDoctoralSemesterHistory> builder)
        {
            builder.Property(e => e.Note)
                .HasMaxLength(500);
        }
    }
}
