using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts
{
	public class PersonStudentSemesterRelocatedFile : RdpzsdAttachedFile
	{
		[Skip]
		public PersonStudentSemester PersonStudentSemester { get; set; }
	}
    public class PersonStudentSemesterRelocatedFileConfiguration : IEntityTypeConfiguration<PersonStudentSemesterRelocatedFile>
    {
        public void Configure(EntityTypeBuilder<PersonStudentSemesterRelocatedFile> builder)
        {
            builder.HasOne(p => p.PersonStudentSemester)
                   .WithOne(a => a.SemesterRelocatedFile)
                   .HasForeignKey<PersonStudentSemesterRelocatedFile>(p => p.Id);
        }
    }
}
