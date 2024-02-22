using System.ComponentModel;

namespace Infrastructure.FileUpload
{
    [Description("Тип на прикачения файл")]
    public enum FileUploadType
    {
        TxtFile = 1,
        DocxFile = 2,
        XlsxFile = 3
    }
}
