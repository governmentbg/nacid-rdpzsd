using Logs.Interfaces;

namespace Logs.Dtos
{
	public class AwPersonStudentDuplicateDiplomaDto : IActionWorkflowOperation
	{
		public string SearchText => string.Empty;

		public string GetText => string.Empty;

		public string CreateText => "Създаване на дубликат на диплома";

		public string UpdateText => "Редакция на дубликат на диплома";

		public string DeleteText => string.Empty;

		public string ExcelText => string.Empty;
	}
}
