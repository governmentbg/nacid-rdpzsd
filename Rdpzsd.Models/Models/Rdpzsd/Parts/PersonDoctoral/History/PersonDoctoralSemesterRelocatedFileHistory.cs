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
	public class PersonDoctoralSemesterRelocatedFileHistory : RdpzsdAttachedFile
	{
		[Skip]
		public PersonDoctoralSemesterHistory PersonDoctoralSemesterHistory { get; set; }
	}

	public class PersonDoctoralSemesterRelocatedFileHistoryConfiguration : IEntityTypeConfiguration<PersonDoctoralSemesterRelocatedFileHistory>
	{
		public void Configure(EntityTypeBuilder<PersonDoctoralSemesterRelocatedFileHistory> builder)
		{
			builder.HasOne(e => e.PersonDoctoralSemesterHistory)
					.WithOne(a => a.SemesterRelocatedFile)
					.HasForeignKey<PersonDoctoralSemesterRelocatedFileHistory>(p => p.Id);
		}
	}
}
