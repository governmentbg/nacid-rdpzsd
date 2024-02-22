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
	public class PersonStudentPeRecognitionDocumentHistory : RdpzsdAttachedFile
	{
        [Skip]
		public PersonStudentHistory PersonStudentHistory { get; set; }
	}
    public class PersonStudentPeRecognitionDocumentHistoryConfiguration : IEntityTypeConfiguration<PersonStudentPeRecognitionDocumentHistory>
    {
        public void Configure(EntityTypeBuilder<PersonStudentPeRecognitionDocumentHistory> builder)
        {
            builder.HasOne(p => p.PersonStudentHistory)
                   .WithOne(a => a.PeRecognitionDocument)
                   .HasForeignKey<PersonStudentPeRecognitionDocumentHistory>(p => p.Id);
        }
    }
}
