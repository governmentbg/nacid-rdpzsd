using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Models.Base;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Models.Models.Rdpzsd.Migration;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Rdpzsd.Parts.PersonSecondary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rdpzsd.Models.Models.Rdpzsd
{
    public class PersonLot : EntityVersion
    {
        public string Uan { get; set; }
        public LotState State { get; set; }

        public int CreateUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public int? CreateInstitutionId { get; set; }
        [Skip]
        public Institution CreateInstitution { get; set; }
        public int? CreateSubordinateId { get; set; }
        [Skip]
        public Institution CreateSubordinate { get; set; }

        public PersonBasic PersonBasic { get; set; }
        public PersonSecondary PersonSecondary { get; set; }
        public List<PersonStudent> PersonStudents { get; set; } = new List<PersonStudent>();
        public List<PersonDoctoral> PersonDoctorals { get; set; } = new List<PersonDoctoral>();
        public List<PersonDiplomaCopy> PersonDiplomaCopies { get; set; } = new List<PersonDiplomaCopy>();
        public List<PersonLotAction> PersonLotActions { get; set; } = new List<PersonLotAction>();

        // MigrationIds from DataUni_ME
        public List<PersonLotIdNumber> PersonLotIdNumbers { get; set; } = new List<PersonLotIdNumber>();

        public void GenerateUan(RdpzsdDbContext context, HashSet<string> personUans = null)
        {
            var random = new Random();

            if (personUans == null) {
                personUans = context.PersonLots
                    .AsNoTracking()
                    .Select(e => e.Uan)
                    .ToHashSet();
            }

            string alphabetChars = "ABCDEFGHKLMNPQRSTUVWXYZ";
            string numberChars = "0123456789";

            while (string.IsNullOrWhiteSpace(Uan) || personUans.Contains(Uan))
            {
                string alphabets = new string(Enumerable.Repeat(alphabetChars, 2).Select(s => s[random.Next(s.Length)]).ToArray());
                string numbers = new string(Enumerable.Repeat(numberChars, 5).Select(s => s[random.Next(s.Length)]).ToArray());
                Uan = alphabets + numbers;
            }
        }
    }

    public class PersonLotConfiguration : IEntityTypeConfiguration<PersonLot>
    {
        public void Configure(EntityTypeBuilder<PersonLot> builder)
        {
            builder.HasIndex(e => e.Uan)
                .IsUnique();

            builder.HasOne(e => e.PersonBasic)
                .WithOne(c => c.Lot)
                .HasForeignKey<PersonBasic>();

            builder.HasOne(e => e.PersonSecondary)
               .WithOne(c => c.Lot)
               .HasForeignKey<PersonSecondary>();

            builder.HasMany(e => e.PersonStudents)
                .WithOne(c => c.Lot)
                .HasForeignKey(e => e.LotId);

            builder.HasMany(e => e.PersonDoctorals)
                .WithOne(c => c.Lot)
                .HasForeignKey(e => e.LotId);

            builder.HasMany(e => e.PersonLotActions)
                .WithOne()
                .HasForeignKey(e => e.LotId);

            builder.HasMany(e => e.PersonDiplomaCopies)
                .WithOne()
                .HasForeignKey(e => e.LotId);
        }
    }
}
