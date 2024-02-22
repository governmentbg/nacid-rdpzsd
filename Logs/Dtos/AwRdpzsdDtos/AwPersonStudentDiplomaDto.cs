using Logs.Interfaces;

namespace Logs.Dtos
{
	public class AwPersonStudentDiplomaDto : IActionWorkflowOperation
	{
		public string SearchText => string.Empty;

		public string GetText => string.Empty;

		public string CreateText => "Създаване на диплома";

		public string UpdateText => "Редакция на диплома";

		public string DeleteText => string.Empty;

		public string ExcelText => string.Empty;
	}
}
