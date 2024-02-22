using Logs.Interfaces;

namespace Logs.Dtos
{
	public class AwPersonDoctoralDto : IActionWorkflowOperation
	{
		public string SearchText => string.Empty;

		public string GetText => "Преглед на докторски програми";

		public string CreateText => "Създаване на докторска програма";

		public string UpdateText => "Редакция на докторска програма";

		public string DeleteText => "Изтриване на докторска програма";

		public string ExcelText => string.Empty;
	}
}
