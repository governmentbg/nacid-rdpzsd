using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Models.Models.Nomenclatures.Others;
using Rdpzsd.Models.Models.Nomenclatures.StudentStatus;
using Rdpzsd.Models.Models.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd.Migration;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Rdpzsd.Parts.Collections;
using Rdpzsd.Models.Models.Rdpzsd.Parts.History;
using Rdpzsd.Models.Models.Rdpzsd.Parts.PersonSecondary;
using Rdpzsd.Models.Models.Rdpzsd.Parts.PersonSecondary.History;
using Rdpzsd.Models.Models.RdpzsdImports;
using Rdpzsd.Models.Models.RdpzsdImports.Collections;
using Rdpzsd.Models.Models.RdpzsdImports.Files;
using Rdpzsd.Models.Models.SchemaVersion;
using System;
using System.Linq;
using System.Reflection;

namespace Rdpzsd.Models
{
	public class RdpzsdDbContext : DbContext
	{
		#region Rdpzsd
		public DbSet<PersonLot> PersonLots { get; set; }
		public DbSet<PersonLotIdNumber> PersonLotIdNumbers { get; set; }
		public DbSet<PersonLotAction> PersonLotActions { get; set; }
        public DbSet<PersonDiplomaCopy> PersonDiplomaCopies { get; set; }
        public DbSet<PersonBasic> PersonBasics { get; set; }
		public DbSet<PersonBasicInfo> PersonBasicInfos { get; set; }
		public DbSet<PersonImage> PersonImages { get; set; }
		public DbSet<PassportCopy> PassportCopies { get; set; }

		public DbSet<PersonBasicHistory> PersonBasicHistories { get; set; }
		public DbSet<PersonBasicHistoryInfo> PersonBasicHistoryInfos { get; set; }
		public DbSet<PersonImageHistory> PersonImageHistories { get; set; }
		public DbSet<PassportCopyHistory> PassportCopyHistories { get; set; }

        public DbSet<PersonSecondary> PersonSecondaries { get; set; }
        public DbSet<PersonSecondaryInfo> PersonSecondaryInfos { get; set; }
		public DbSet<PersonSecondaryRecognitionDocument> PersonSecondaryRecognitionDocuments { get; set; }

		public DbSet<PersonSecondaryHistory> PersonSecondaryHistories { get; set; }
        public DbSet<PersonSecondaryHistoryInfo> PersonSecondaryHistoryInfos{ get; set; }
		public DbSet<PersonSecondaryRecognitionDocumentHistory> PersonSecondaryRecognitionDocumentsHistories { get; set; }

		public DbSet<PersonStudent> PersonStudents { get; set; }
		public DbSet<PersonStudentInfo> PersonStudentInfos { get; set; }
		public DbSet<PersonStudentSemester> PersonStudentSemesters { get; set; }
		public DbSet<PersonStudentDiploma> PersonStudentDiplomas { get; set; }
		public DbSet<PersonStudentDiplomaFile> PersonStudentDiplomaFile { get; set; }
		public DbSet<PersonStudentProtocol> PersonStudentProtocols { get; set; }
		public DbSet<PersonStudentDuplicateDiploma> PersonStudentDuplicateDiplomas { get; set; }
		public DbSet<PersonStudentSemesterRelocatedFile> PersonStudentSemesterRelocatedFiles { get; set; }
        public DbSet<PersonStudentStickerNote> PersonStudentStickerNotes { get; set; }
        public DbSet<PersonStudentHistory> PersonStudentHistories { get; set; }
		public DbSet<PersonStudentHistoryInfo> PersonStudentHistoryInfos { get; set; }
		public DbSet<PersonStudentSemesterHistory> PersonStudentSemesterHistories { get; set; }
		public DbSet<PersonStudentDiplomaHistory> PersonStudentDiplomaHistories { get; set; }
		public DbSet<PersonStudentDiplomaFileHistory> PersonStudentDiplomaFileHistories { get; set; }
		public DbSet<PersonStudentProtocolHistory> PersonStudentProtocolHistories { get; set; }
		public DbSet<PersonStudentDuplicateDiplomaHistory> PersonStudentDuplicateDiplomaHistories { get; set; }
		public DbSet<PersonStudentSemesterRelocatedFileHistory> PersonStudentSemesteryRelocatedFilesHistories { get; set; }


		public DbSet<PersonDoctoral> PersonDoctorals { get; set; }
		public DbSet<PersonDoctoralInfo> PersonDoctoralInfos { get; set; }
		public DbSet<PersonDoctoralSemester> PersonDoctoralSemesters { get; set; }
		public DbSet<PersonDoctoralSemesterRelocatedFile> PersonDoctoralSemesterRelocatedFiles { get; set; }

		public DbSet<PersonDoctoralHistory> PersonDoctoralHistories { get; set; }
		public DbSet<PersonDoctoralHistoryInfo> PersonDoctoralHistoryInfos { get; set; }
		public DbSet<PersonStudentSemesterHistory> PersonDoctoralSemesterHistories { get; set; }
		public DbSet<PersonDoctoralSemesterRelocatedFileHistory> PersonDoctoralSemesterRelocatedFilesHistories { get; set; }
		#endregion

		#region RdpzsdImports
		public DbSet<PersonImport> PersonImports { get; set; }
		public DbSet<PersonImportFile> PersonImportFiles { get; set; }
		public DbSet<PersonImportErrorFile> PersonImportErrorFiles { get; set; }
        public DbSet<PersonImportUan> PersonImportUans { get; set; }

		public DbSet<SpecialityImport> SpecialityImports { get; set; }
		public DbSet<SpecialityImportFile> SpecialityImportFiles { get; set; }
		public DbSet<SpecialityImportErrorFile> SpecialityImportErrorFiles { get; set; }
		#endregion

		#region Nomenclatures
		public DbSet<Country> Countries { get; set; }
		public DbSet<District> Districts { get; set; }
		public DbSet<Municipality> Municipalities { get; set; }
		public DbSet<Settlement> Settlements { get; set; }

		public DbSet<Institution> Institutions { get; set; }
		public DbSet<InstitutionSpeciality> InstitutionSpecialities { get; set; }
		public DbSet<Speciality> Specialities { get; set; }
		public DbSet<ResearchArea> ResearchAreas { get; set; }
		public DbSet<EducationalForm> EducationalForms { get; set; }
		public DbSet<EducationalQualification> EducationalQualifications { get; set; }
		public DbSet<InstitutionSpecialityLanguage> InstitutionSpecialityLanguages { get; set; }
		public DbSet<InstitutionSpecialityJointSpeciality> InstitutionSpecialityJointSpecialities { get; set; }
		public DbSet<Language> Languages { get; set; }
		public DbSet<NationalStatisticalInstitute> NationalStatisticalInstitutes { get; set; }

		public DbSet<Period> Periods { get; set; }
		public DbSet<School> Schools { get; set; }
		public DbSet<AdmissionReason> AdmissionReasons { get; set; }
		public DbSet<EducationFeeType> EducationFeeTypes { get; set; }
		public DbSet<AdmissionReasonEducationFee> AdmissionReasonEducationFees { get; set; }
		public DbSet<AdmissionReasonHistory> AdmissionReasonHistories { get; set; }
		public DbSet<AdmissionReasonEducationFeeHistory> AdmissionReasonEducationFeeHistories { get; set; }

		public DbSet<StudentStatus> StudentStatuses { get; set; }
		public DbSet<StudentEvent> StudentEvents { get; set; }
		public DbSet<StudentEventQualification> StudentEventQualifications { get; set; }
		#endregion

		#region SchemaVersion
		public DbSet<SchemaVersion> SchemaVersions { get; set; }
		#endregion

		public RdpzsdDbContext(DbContextOptions<RdpzsdDbContext> options)
			: base(options)
		{
		}

		public IDbContextTransaction BeginTransaction()
		{
			return Database.BeginTransaction();
		}

		public void SetValues(IEntityVersion original, IEntityVersion entity)
		{
			entity.Id = original.Id;

			Entry(original).OriginalValues["Version"] = entity.Version;
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			ApplyConfigurations(modelBuilder);
			DisableCascadeDelete(modelBuilder);
			ConfigurePgSqlNameMappings(modelBuilder);

			modelBuilder.Entity<SchemaVersion>()
				.ToTable("schemaversions");
			//ConfigureOptimisticConcurrencyToken(modelBuilder);
		}

		protected void ApplyConfigurations(ModelBuilder modelBuilder)
		{
			var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
				   .Where(t => t.GetInterfaces().Any(gi =>
					   gi.IsGenericType
					   && gi.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
				   .ToList();

			foreach (var type in typesToRegister)
			{
				dynamic configurationInstance = Activator.CreateInstance(type);
				modelBuilder.ApplyConfiguration(configurationInstance);
			}
		}

		protected void DisableCascadeDelete(ModelBuilder modelBuilder)
		{
			modelBuilder.Model.GetEntityTypes()
				.SelectMany(t => t.GetForeignKeys())
				.Where(fk => !fk.IsOwnership
					&& fk.DeleteBehavior == DeleteBehavior.Cascade)
				.ToList()
				.ForEach(e => e.DeleteBehavior = DeleteBehavior.Restrict);
		}

		protected void ConfigurePgSqlNameMappings(ModelBuilder modelBuilder)
		{
			foreach (var entity in modelBuilder.Model.GetEntityTypes())
			{
				// Configure pgsql table names convention.
				entity.SetTableName(entity.ClrType.Name.ToLower());

				// Configure pgsql column names convention.
				foreach (var property in entity.GetProperties())
					property.SetColumnName(property.Name.ToLower());
			}
		}
	}
}
