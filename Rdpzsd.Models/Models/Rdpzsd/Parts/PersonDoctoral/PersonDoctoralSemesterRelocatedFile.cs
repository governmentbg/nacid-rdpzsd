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
	public class PersonDoctoralSemesterRelocatedFile : RdpzsdAttachedFile
	{
		[Skip]
		public PersonDoctoralSemester PersonDoctoralSemester { get; set; }
	}
	public class PersonDoctoralSemesterRelocatedFileConfiguration : IEntityTypeConfiguration<PersonDoctoralSemesterRelocatedFile>
	{
		public void Configure(EntityTypeBuilder<PersonDoctoralSemesterRelocatedFile> builder)
		{
			builder.HasOne(e => e.PersonDoctoralSemester)
					.WithOne(a => a.SemesterRelocatedFile)
					.HasForeignKey<PersonDoctoralSemesterRelocatedFile>(p => p.Id);
		}
	}
}
