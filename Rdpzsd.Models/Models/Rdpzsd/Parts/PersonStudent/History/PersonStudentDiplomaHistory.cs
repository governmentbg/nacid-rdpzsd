using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;
using System;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.History
{
    public class PersonStudentDiplomaHistory : EntityVersion
    {
        [Skip]
        public PersonStudentHistory PersonStudentHistory { get; set; }

        public string DiplomaNumber { get; set; }
        public string RegistrationDiplomaNumber { get; set; }
        public DateTime? DiplomaDate { get; set; }

        public PersonStudentDiplomaFileHistory File { get; set; }

        public bool IsValid { get; set; }
    }

    public class PersonStudentDiplomaHistoryConfiguration : IEntityTypeConfiguration<PersonStudentDiplomaHistory>
    {
        public void Configure(EntityTypeBuilder<PersonStudentDiplomaHistory> builder)
        {
            builder.HasOne(p => p.PersonStudentHistory)
                   .WithOne(a => a.Diploma)
                   .HasForeignKey<PersonStudentDiplomaHistory>(p => p.Id);

            builder.Property(e => e.DiplomaNumber)
                .HasMaxLength(25);

            builder.Property(e => e.RegistrationDiplomaNumber)
                .HasMaxLength(25);
        }
    }
}
