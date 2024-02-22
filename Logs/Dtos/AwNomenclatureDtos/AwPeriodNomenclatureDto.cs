using Logs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logs.Dtos
{
	public class AwPeriodNomenclatureDto : IActionWorkflowOperation
	{
		public string SearchText => string.Empty;

		public string GetText => string.Empty;

		public string CreateText => "Добавяне на номенклатура - учебни семестри";

		public string UpdateText => "Редакция на номенклатура - учебни семестри";

		public string DeleteText => "Изтриване на номенклатура - учебни семестри";

		public string ExcelText => "Изтегляне на excel номенклатури - учебни семестри";
	}
}
