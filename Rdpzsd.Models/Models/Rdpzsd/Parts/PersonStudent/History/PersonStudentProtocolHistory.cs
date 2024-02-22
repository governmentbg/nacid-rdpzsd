using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Base;
using System;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.History
{
    public class PersonStudentProtocolHistory : EntityVersion
    {
        public int PartId { get; set; }
        public StudentProtocolType ProtocolType { get; set; }
        public string ProtocolNumber { get; set; }
        public DateTime ProtocolDate { get; set; }
    }

    public class PersonStudentProtocolHistoryConfiguration : IEntityTypeConfiguration<PersonStudentProtocolHistory>
    {
        public void Configure(EntityTypeBuilder<PersonStudentProtocolHistory> builder)
        {
            builder.Property(e => e.ProtocolNumber)
                .HasMaxLength(25);
        }
    }
}
