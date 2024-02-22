using Logs.Interfaces;

namespace Logs.Dtos
{
	public class AwPersonImportDto : IActionWorkflowOperation
	{
		public string SearchText => "Търсене в импорт на физически лица";

		public string GetText => "Преглед на импорт на физически лица";

		public string CreateText => "Импортиране на файл за физически лица";

		public string UpdateText => "Промяна на импорт файл за физически лица";

		public string DeleteText => "Изтриване на импорт файл на физически лица";

		public string ExcelText => string.Empty;
	}
}
