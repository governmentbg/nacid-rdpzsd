using Logs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logs.Dtos
{
	public class AwSpecialityImportDto : IActionWorkflowOperation
	{
		public string SearchText => "Търсене в импорт на семестриална информация";

		public string GetText => "Преглед на импорт за семестриална информация";

		public string CreateText => "Импортиране на файл за семестриална информация";

		public string UpdateText => "Промяна на импорт файл за семестриална информация";

		public string DeleteText => "Изтриване на импорт файл за семестриална информация";

		public string ExcelText => string.Empty;
	}
}
