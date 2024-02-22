using Infrastructure.Constants;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Infrastructure.Integrations.RdDocumentsIntegration.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Models.Models.Nomenclatures.Others;
using Rdpzsd.Models.Models.Rdpzsd.Interfaces;
using Rdpzsd.Models.Models.Rdpzsd.Parts.PersonSecondary.Base;
using Rdpzsd.Models.Models.Rdpzsd.Parts.PersonSecondary.History;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.PersonSecondary
{
    public class PersonSecondary : BasePersonSecondary<PersonSecondaryInfo>,
        ISinglePart<PersonSecondary, PersonLot, PersonSecondaryHistory>
    {
        [Skip]
        public PersonLot Lot { get; set; }
        public bool FromRso { get; set; }

        //This is used for calling diploma images
        public double? RsoIntId { get; set; }
        [Skip]
        public List<PersonSecondaryHistory> Histories { get; set; } = new List<PersonSecondaryHistory>();

		public PersonSecondaryRecognitionDocument PersonSecondaryRecognitionDocument { get; set; }
		public PersonSecondary() { }

        public PersonSecondary(RsoDocumentDto rdDocumentDto, School school, Country country)
        {
            GraduationYear = rdDocumentDto.IntYearGraduated ?? 1900;
            CountryId = country.Id;
            Country = country;
            SchoolId = country.Code == CountryConstants.BulgariaCode ? school.Id : null;
            School = country.Code == CountryConstants.BulgariaCode ? school : null;
            ForeignSchoolName = country.Code == CountryConstants.Missing ? rdDocumentDto.VcSchoolName : null;
            DiplomaDate = rdDocumentDto.DtRegDate;
            DiplomaNumber = $"{rdDocumentDto.VcPrnSer} {rdDocumentDto.VcPrnNo} {rdDocumentDto.VcRegNo1}{(!string.IsNullOrWhiteSpace(rdDocumentDto.VcRegNo2) ? $"-{rdDocumentDto.VcRegNo2}" : string.Empty)}";
            FromRso = true;
            RsoIntId = rdDocumentDto.IntID;
        }

        public void ValidateProperties(RdpzsdDbContext context, DomainValidatorService domainValidatorService)
        {
            if (GraduationYear < 1960 || GraduationYear > DateTime.Now.Year)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonSecondary_InvalidGraduationYear);
            }

            if (!SchoolId.HasValue && string.IsNullOrWhiteSpace(ForeignSchoolName) && string.IsNullOrWhiteSpace(MissingSchoolName))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonSecondary_InvalidSchool);
            }
            if (Country?.Code != "BG" && string.IsNullOrWhiteSpace(RecognitionNumber))
			{
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonSecondary_InvalidRecognitionNumber);
            }
            if (Country?.Code != "BG" && RecognitionDate?.Year < 1960 && RecognitionDate?.Year > DateTime.Now.Year)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonSecondary_InvalidRecognitionDate);
            }
            if (Country?.Code != "BG" && PersonSecondaryRecognitionDocument == null)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonSecondary_InvalidRecognitionDocument);
            }
            if(Country?.Code == "BG")
			{
                RecognitionDate = null;
                RecognitionNumber = null;
                PersonSecondaryRecognitionDocument = null;
			}
        }

        public IQueryable<PersonSecondary> IncludeAll(IQueryable<PersonSecondary> query)
        {
            return query
                .Include(e => e.Country)
                .Include(e => e.School.Settlement)
                .Include(e => e.PersonSecondaryRecognitionDocument)
                .Include(e => e.MissingSchoolSettlement);
        }

        public class PersonSecondaryConfiguration : IEntityTypeConfiguration<PersonSecondary>
        {
            public void Configure(EntityTypeBuilder<PersonSecondary> builder)
            {
                builder.HasMany(e => e.Histories)
                        .WithOne()
                        .HasForeignKey(e => e.PartId);

                builder.Property(e => e.DiplomaNumber)
                       .HasMaxLength(100);

                builder.Property(e => e.ForeignSchoolName)
                       .HasMaxLength(180);

                builder.Property(e => e.Profession)
                       .HasMaxLength(200);

                builder.Property(e => e.RecognitionNumber)
                       .HasMaxLength(30);

                builder.Property(e => e.MissingSchoolName)
                       .HasMaxLength(180);
            }
        }
    }
}
