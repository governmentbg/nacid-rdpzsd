using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.PersonSecondary.History
{
	public class PersonSecondaryRecognitionDocumentHistory : RdpzsdAttachedFile
	{
		[Skip]
		public PersonSecondaryHistory PersonSecondaryHistory { get; set; }
	}
	public class PersonSecondaryRecognitionDocumentHistoryConfiguration : IEntityTypeConfiguration<PersonSecondaryRecognitionDocumentHistory>
	{
		public void Configure(EntityTypeBuilder<PersonSecondaryRecognitionDocumentHistory> builder)
		{
			builder.HasOne(e => e.PersonSecondaryHistory)
				.WithOne(s => s.PersonSecondaryRecognitionDocument)
				.HasForeignKey<PersonSecondaryRecognitionDocumentHistory>(p => p.Id);
				
		}
	}
}
