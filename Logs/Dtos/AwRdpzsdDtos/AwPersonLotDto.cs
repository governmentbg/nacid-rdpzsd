using Logs.Interfaces;

namespace Logs.Dtos
{
	public class AwPersonLotDto : IActionWorkflowOperation
	{
		public string SearchText => string.Empty;

		public string GetText => "Преглед на физическо лице";

		public string CreateText => "Създаване на физическо лице";

		public string UpdateText => string.Empty;

		public string DeleteText => "Изтриване на физическо лице";

		public string ExcelText => string.Empty;
	}
}
