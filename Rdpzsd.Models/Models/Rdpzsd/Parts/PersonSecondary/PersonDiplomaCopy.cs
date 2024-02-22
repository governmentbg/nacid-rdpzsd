using Infrastructure.Integrations.RdDocumentsIntegration.Dtos;
using Rdpzsd.Models.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.PersonSecondary
{
    public class PersonDiplomaCopy: EntityVersion
    {
        public int LotId { get; set; }
        public string DocName { get; set; }
        public string SchoolMun { get; set; }
        public string SchoolObl { get; set; }
        public string SchoolTown { get; set; }
        public DateTime? DtOrigRegDate { get; set; }
        public DateTime? DtProtDate { get; set; }
        public DateTime? DtRegDate { get; set; }
        public int? IntCurrVer { get; set; }
        public int IntCurrYear { get; set; }
        public int? IntDocID { get; set; }
        public double? IntDocTemplID { get; set; }
        public double? IntID { get; set; }
        public int? IntIDType { get; set; }
        public decimal? IntMeanMark { get; set; }
        public int? IntSchoolID { get; set; }
        public long IntStudentID { get; set; }
        public int? IntYearGraduated { get; set; }
        public string VcIDNumberText { get; set; }
        public string VcOrigPrnNo { get; set; }
        public string VcOrigPrnSer { get; set; }
        public string VcOrigRegNo1 { get; set; }
        public string VcOrigRegNo2 { get; set; }
        public string VcOrigSchoolName { get; set; }
        public string VcPrnNo { get; set; }
        public string VcPrnSer { get; set; }
        public string VcProtNo { get; set; }
        public string VcRegNo1 { get; set; }
        public string VcRegNo2 { get; set; }
        public string VcSchoolName { get; set; }
        public string VcStudName1 { get; set; }
        public string VcStudName2 { get; set; }
        public string VcStudName3 { get; set; }

        public PersonDiplomaCopy()
        { 
        }

        public PersonDiplomaCopy(RsoDocumentDto rsoDocumentDto, int lotId)
        {
            LotId = lotId;
            DocName = rsoDocumentDto.DocName;
            SchoolMun = rsoDocumentDto.SchoolMun;
            SchoolObl = rsoDocumentDto.SchoolObl;
            SchoolTown = rsoDocumentDto.SchoolTown;
            DtOrigRegDate = rsoDocumentDto.DtOrigRegDate;
            DtProtDate = rsoDocumentDto.DtProtDate;
            DtRegDate = rsoDocumentDto.DtRegDate;
            IntCurrVer = rsoDocumentDto.IntCurrVer;
            IntCurrYear = rsoDocumentDto.IntCurrYear;
            IntDocID = rsoDocumentDto.IntDocID;
            IntDocTemplID = rsoDocumentDto.IntDocTemplID;
            IntID = rsoDocumentDto.IntID;
            IntIDType = rsoDocumentDto.IntIDType;
            IntMeanMark = rsoDocumentDto.IntMeanMark;
            IntSchoolID = rsoDocumentDto.IntSchoolID;
            IntStudentID = rsoDocumentDto.IntStudentID;
            IntYearGraduated = rsoDocumentDto.IntYearGraduated;
            VcIDNumberText = rsoDocumentDto.VcIDNumberText;
            VcOrigPrnNo = rsoDocumentDto.VcOrigPrnNo;
            VcOrigPrnSer = rsoDocumentDto.VcOrigPrnSer;
            VcOrigRegNo1 = rsoDocumentDto.VcOrigRegNo1;
            VcOrigRegNo2 = rsoDocumentDto.VcOrigRegNo2;
            VcOrigSchoolName = rsoDocumentDto.VcOrigSchoolName;
            VcPrnNo = rsoDocumentDto.VcPrnNo;
            VcPrnSer = rsoDocumentDto.VcPrnSer;
            VcProtNo = rsoDocumentDto.VcProtNo;
            VcRegNo1 = rsoDocumentDto.VcRegNo1;
            VcRegNo2 = rsoDocumentDto.VcRegNo2;
            VcSchoolName = rsoDocumentDto.VcSchoolName;
            VcStudName1 = rsoDocumentDto.VcStudName1;
            VcStudName2 = rsoDocumentDto.VcStudName2;
            VcStudName3 = rsoDocumentDto.VcStudName3;
        }
    }

    public static class PersonDiplomaCopyExtensions
    {
        public static List<PersonDiplomaCopy> ToPersonDiplomaCopy(this List<RsoDocumentDto> rsoDocumentDtos, int lotId)
        {
            return rsoDocumentDtos.Select(e => new PersonDiplomaCopy(e, lotId)).ToList();
        }
    }
}
