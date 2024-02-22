namespace Rdpzsd.Models.Models.Rdpzsd.Migration
{
    // This is used because in DataUni_ME they have same persons with different ID Number
    public class PersonLotIdNumber
    {
        public int Id { get; set; }
        // Fill them when migrating from old system
        // Student number
        public double? MigrationIdNumber { get; set; }
        // University which added this student number
        public int? MigrationUniId { get; set; }
        public int PersonLotId { get; set; }
        public PersonLot PersonLot { get; set; }
        public int? InstitutionLotId { get; set; }
        public int? SubordinateId { get; set; }
        public IdentitfierChangeAction? IdentifierTypeAction { get; set; } = null;
        public double? ApplicationId { get; set; } = null;
    }
}
