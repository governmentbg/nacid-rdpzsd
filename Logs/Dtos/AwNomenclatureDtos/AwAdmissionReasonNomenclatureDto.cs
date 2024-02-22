using Logs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logs.Dtos
{
	public class AwAdmissionReasonNomenclatureDto : IActionWorkflowOperation
	{
		public string SearchText => string.Empty;

		public string GetText => string.Empty;

		public string CreateText => "Добавяне на номенклатура - основание за прием";

		public string UpdateText => "Редакция на номенклатура - основание за прием";

		public string DeleteText => "Изтриване на номенклатура - основание за прием";

		public string ExcelText => "Изтегляне на excel номенклатури - основание за прием";
	}
}
