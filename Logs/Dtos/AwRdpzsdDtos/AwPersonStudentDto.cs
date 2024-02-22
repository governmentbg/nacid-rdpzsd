using Logs.Interfaces;

namespace Logs.Dtos
{
	public class AwPersonStudentDto : IActionWorkflowOperation
	{
		public string SearchText => string.Empty;

		public string GetText => "Преглед на специалности";

		public string CreateText => "Създаване на специалност";

		public string UpdateText => "Редакция на специалност";

		public string DeleteText => "Изтриване на специалност";

		public string ExcelText => string.Empty;
	}
}
