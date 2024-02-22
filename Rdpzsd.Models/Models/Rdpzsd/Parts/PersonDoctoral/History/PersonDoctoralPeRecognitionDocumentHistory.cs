using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.History
{
	public class PersonDoctoralPeRecognitionDocumentHistory : RdpzsdAttachedFile
	{
		[Skip]
		public PersonDoctoralHistory PersonDoctoralHistory { get; set; }
	}
	public class PersonDoctoralPeRecognitionDocumentHistoryConfiguration : IEntityTypeConfiguration<PersonDoctoralPeRecognitionDocumentHistory>
	{
		public void Configure(EntityTypeBuilder<PersonDoctoralPeRecognitionDocumentHistory> builder)
		{
			builder.HasOne(e => e.PersonDoctoralHistory)
				.WithOne(s => s.PeRecognitionDocument)
				.HasForeignKey<PersonDoctoralPeRecognitionDocumentHistory>(p => p.Id);
		}
	}
}
