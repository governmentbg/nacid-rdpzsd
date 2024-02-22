using Logs.Interfaces;

namespace Logs.Dtos
{
	public class AwPersonStudentSemesterDto : IActionWorkflowOperation
	{
		public string SearchText => string.Empty;

		public string GetText => string.Empty;

		public string CreateText => "Създаване на семестър";

		public string UpdateText => "Редакция на семестър";

		public string DeleteText => "Изтриване на семестър";

		public string ExcelText => string.Empty;
	}
}
