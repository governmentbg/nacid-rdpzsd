using Logs.Interfaces;

namespace Logs.Dtos
{
	public class AwPersonStudentProtocolDto : IActionWorkflowOperation
	{
		public string SearchText => string.Empty;

		public string GetText => string.Empty;

		public string CreateText => "Създаване на протокол";

		public string UpdateText => "Редакция на протокол";

		public string DeleteText => "Изтриване на протокол";

		public string ExcelText => string.Empty;
	}
}
