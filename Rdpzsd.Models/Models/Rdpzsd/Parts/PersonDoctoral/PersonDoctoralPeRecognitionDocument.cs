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
	public class PersonDoctoralPeRecognitionDocument : RdpzsdAttachedFile
	{
		[Skip]
		public PersonDoctoral PersonDoctoral { get; set; }
	}
	public class PersonDoctoralPeRecognitionDocumentConfiguration : IEntityTypeConfiguration<PersonDoctoralPeRecognitionDocument>
	{
		public void Configure(EntityTypeBuilder<PersonDoctoralPeRecognitionDocument> builder)
		{
			builder.HasOne(e => e.PersonDoctoral)
				.WithOne(s => s.PeRecognitionDocument)
				.HasForeignKey<PersonDoctoralPeRecognitionDocument>(p => p.Id);
		}
	}
}
