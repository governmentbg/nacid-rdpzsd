using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;
using Rdpzsd.Models.Models.Rdpzsd.Parts.History;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts
{
	public class PersonStudentSemesterRelocatedFileHistory : RdpzsdAttachedFile
    {
        [Skip]
        public PersonStudentSemesterHistory PersonStudentSemesterHistory { get; set; }
    }


    public class PersonStudentSemesterRelocatedFileHistoryConfiguration : IEntityTypeConfiguration<PersonStudentSemesterRelocatedFileHistory>
    {
        public void Configure(EntityTypeBuilder<PersonStudentSemesterRelocatedFileHistory> builder)
        {
            builder.HasOne(p => p.PersonStudentSemesterHistory)
                   .WithOne(a => a.SemesterRelocatedFile)
                   .HasForeignKey<PersonStudentSemesterRelocatedFileHistory>(p => p.Id);
        }
    }
}
