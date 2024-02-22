using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.PersonSecondary
{
	public class PersonSecondaryRecognitionDocument : RdpzsdAttachedFile
	{
		[Skip]
		public PersonSecondary PersonSecondary { get; set; }
	}
	public class PersonSecondaryRecognitionDocumentConfiguration : IEntityTypeConfiguration<PersonSecondaryRecognitionDocument>
	{
		public void Configure(EntityTypeBuilder<PersonSecondaryRecognitionDocument> builder)
		{
			builder.HasOne(e => e.PersonSecondary)
					.WithOne(s => s.PersonSecondaryRecognitionDocument)
					.HasForeignKey<PersonSecondaryRecognitionDocument>(p => p.Id);
		}
	}
}
