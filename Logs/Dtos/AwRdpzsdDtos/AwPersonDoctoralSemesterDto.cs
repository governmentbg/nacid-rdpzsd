using Logs.Interfaces;

namespace Logs.Dtos
{
	public class AwPersonDoctoralSemesterDto : IActionWorkflowOperation
	{
		public string SearchText => string.Empty;

		public string GetText => string.Empty;

		public string CreateText => "Създаване на атестация";

		public string UpdateText => "Редакция на атестация";

		public string DeleteText => "Изтриване на атестация";

		public string ExcelText => string.Empty;
	}
}
