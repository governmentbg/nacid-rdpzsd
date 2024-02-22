using Logs.Interfaces;

namespace Logs.Dtos
{
    public class AwPersonSecondaryDto : IActionWorkflowOperation
    {
        public string SearchText => string.Empty;

        public string GetText => "Преглед на средно образование.";

        public string CreateText => "Създаване на средно образование.";

        public string UpdateText => "Редакция на средно образование.";

        public string DeleteText => string.Empty;

		public string ExcelText => string.Empty;
    }
}
