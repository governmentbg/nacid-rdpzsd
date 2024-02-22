using Logs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logs.Dtos
{
	public class AwSchoolNomenclatureDto : IActionWorkflowOperation
	{
		public string SearchText => string.Empty;

		public string GetText => string.Empty;

		public string CreateText => "Добавяне на нова номенклатура - средно училище";

		public string UpdateText => "Редакция на номенклатура - средно училище";

		public string DeleteText => "Изтриване на номенклатура - средно училище";

		public string ExcelText => "Изтегляне на excel номенклатури - средно училище";
	}
}
