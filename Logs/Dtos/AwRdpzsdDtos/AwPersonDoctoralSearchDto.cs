using Logs.Interfaces;

namespace Logs.Dtos
{
	public class AwPersonDoctoralSearchDto : IActionWorkflowOperation
	{
		public string SearchText => "Търсене на докторанти";

		public string GetText => string.Empty;

		public string CreateText => string.Empty;

		public string UpdateText => string.Empty;

		public string DeleteText => string.Empty;

		public string ExcelText => string.Empty;
	}
}
