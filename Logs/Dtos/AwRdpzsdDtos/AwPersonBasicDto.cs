using Logs.Interfaces;

namespace Logs.Dtos
{
    public class AwPersonBasicDto : IActionWorkflowOperation
    {
        public string SearchText => string.Empty;

        public string GetText => "Преглед на физическо лице.";

        public string CreateText => string.Empty;

        public string UpdateText => "Редакция на физическо лице.";

        public string DeleteText => string.Empty;

		public string ExcelText => string.Empty;
    }
}
