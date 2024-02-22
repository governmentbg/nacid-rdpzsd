using Logs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logs.Dtos.AwNomenclatureDtos
{
	public class AwEducationFeeTypeNomenclatureDto : IActionWorkflowOperation
	{
		public string SearchText => string.Empty;

		public string GetText => string.Empty;

		public string CreateText => string.Empty;

		public string UpdateText => string.Empty;

		public string DeleteText => string.Empty;

		public string ExcelText => "Изтегляне на excel - номенклатури вид на таксата на обучение";
	}
}
