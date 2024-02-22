using System;

namespace Infrastructure.Integrations.RdDocumentsIntegration.Dtos
{
    public class RsoDocumentDto
    {
        // Наименование на вида документ
        public string DocName { get; set; }
        // Община на училището
        public string SchoolMun { get; set; }
        // Област на училището
        public string SchoolObl { get; set; }
        //Населено място на училището
        public string SchoolTown { get; set; }
        // Дата на оригиналния документ(в случаи на дубликат)
        public DateTime? DtOrigRegDate { get; set; }
        // Дата на протокола
        public DateTime? DtProtDate { get; set; }
        // Дата на документа
        public DateTime? DtRegDate { get; set; }
        // Служебно поле
        public int? IntCurrVer { get; set; }
        // Учебна година, през която е регистриран документа
        public int IntCurrYear { get; set; }
        // Код на вида документ
        public int? IntDocID { get; set; }
        // Служебно поле
        public double? IntDocTemplID { get; set; }
        // Уникален идентификатор на документа
        public double? IntID { get; set; }
        // 0 - ЕГН, 1 - ЛНЧ, 2 - ИДН
        public int? IntIDType { get; set; }
        // Среден успех
        public decimal? IntMeanMark { get; set; }
        // Код на училището, издало документа
        public int? IntSchoolID { get; set; }
        // Идентификатор на лицето
        public long IntStudentID { get; set; }
        // Година на завършване на образованието
        public int? IntYearGraduated { get; set; }
        // Личен идентификатор в случаите на ID Type = 2
        public string VcIDNumberText { get; set; }
        // Фабричен номер на бланката на оригиналния документ(в случаи на дубликат)
        public string VcOrigPrnNo { get; set; }
        // Серия на бланката на оригиналния документ(в случаи на дубликат)
        public string VcOrigPrnSer { get; set; }
        // Регистрационен номер (1 част) на оригиналния документ(в случаи на дубликат)
        public string VcOrigRegNo1 { get; set; }
        // Регистрационен номер (2 част) на оригиналния документ(в случаи на дубликат)
        public string VcOrigRegNo2 { get; set; }
        // Наименование на училището, издало оригиналния документ(в случаи на дубликат)
        public string VcOrigSchoolName { get; set; }
        // Фабричен номер на бланката
        public string VcPrnNo { get; set; }
        // Серия на бланката
        public string VcPrnSer { get; set; }
        // Фабричен номер на бланката
        public string VcProtNo { get; set; }
        // Регистрационен номер (1 част)
        public string VcRegNo1 { get; set; }
        // Регистрационен номер (2 част)
        public string VcRegNo2 { get; set; }
        //Наименование на училището, издало документа
        public string VcSchoolName { get; set; }
        // Име
        public string VcStudName1 { get; set; }
        // Презиме
        public string VcStudName2 { get; set; }
        // Фамилия
        public string VcStudName3 { get; set; }
    }
}
