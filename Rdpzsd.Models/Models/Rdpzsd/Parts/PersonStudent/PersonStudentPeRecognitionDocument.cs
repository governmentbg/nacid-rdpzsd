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
	public class PersonStudentPeRecognitionDocument : RdpzsdAttachedFile
	{
		[Skip]
		public PersonStudent PersonStudent { get; set; }
	}
	public class PersonStudentPeRecognitionDocumentConfiguration : IEntityTypeConfiguration<PersonStudentPeRecognitionDocument>
	{
		public void Configure(EntityTypeBuilder<PersonStudentPeRecognitionDocument> builder)
		{
			builder.HasOne(e => e.PersonStudent)
				.WithOne(s => s.PeRecognitionDocument)
				.HasForeignKey<PersonStudentPeRecognitionDocument>(p => p.Id);
		}
	}
}
