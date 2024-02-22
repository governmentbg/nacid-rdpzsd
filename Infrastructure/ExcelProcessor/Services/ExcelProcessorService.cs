using Infrastructure.ExcelProcessor.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure.ExcelProcessor.Services
{
	public class ExcelProcessorService
	{
		readonly EnumUtilityService utility;

		public ExcelProcessorService(EnumUtilityService utility)
		{
			this.utility = utility;
		}

		public MemoryStream ExportMultiSheet<TResult>(IEnumerable<ExcelMultiSheet<TResult>> exportSheets)
		{
			using ExcelPackage package = new ExcelPackage();
			foreach (var exportSheet in exportSheets)
			{
				ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(exportSheet.SheetName);

				ConstructSheet(worksheet, exportSheet);
			}

			var stream = new MemoryStream(package.GetAsByteArray());
			return stream;
		}

		private void ConstructSheet<TResult>(ExcelWorksheet worksheet, ExcelMultiSheet<TResult> exportSheet)
		{
			var headers = new List<string>();
			var memberExpressions = new List<MemberExpression>();
			GetHeadersAndMembers(ref headers, ref memberExpressions, exportSheet.Expressions.ToArray());

			bool[] isFormatedMaxCols = new bool[headers.Count];

			int col = 1, row = 1;

			for (int i = 0; i < headers.Count; i++)
			{

				worksheet.Cells[row, col].Value = headers[i];
				worksheet.Cells[row, col].Style.Font.Bold = true;

				col++;
			}

			foreach (var item in exportSheet.Items)
			{
				col = 1;
				row++;
				foreach (var memberExpression in memberExpressions)
				{
					object value = null;
					if (memberExpression.Expression.Type == item.GetType() && memberExpression.Expression.GetType() == typeof(UnaryExpression))
					{
						value = item.GetType().GetProperty(memberExpression.Member.Name).GetValue(item, null);
					}
					else
					{
						var resultValue = GetNestedProperties(item, memberExpression.Expression.ToString().Substring(memberExpression.Expression.ToString().IndexOf(".", StringComparison.Ordinal) + 1));
						if (resultValue == null)
						{
							value = null;
						}
						else
						{
							value = resultValue.GetType().GetProperty(memberExpression.Member.Name).GetValue(resultValue, null);
						}
					}

					if (value != null
						&& value.GetType().BaseType == typeof(Enum))
					{
						worksheet.Cells[row, col].Value = utility.GetDescription(value);
					}
					else
					{
						worksheet.Cells[row, col].Value = value;
					}

					var fieldType = memberExpression.Type;
					if (fieldType == typeof(bool)
						|| fieldType == typeof(bool?))
					{
						var obj = (bool)worksheet.Cells[row, col].Value;
						string boolValue = "Не";
						if (obj)
						{
							boolValue = "Да";
						}
						worksheet.Cells[row, col].Value = boolValue;
					}

					worksheet.Cells[row, col].Style.Numberformat.Format = GetCellFormatting(fieldType);

					if (!isFormatedMaxCols[col - 1]
						&& worksheet.Cells[row, col].Value != null)
					{
						int cellSize = worksheet.Cells[row, col].Value.ToString().Length;
						if (cellSize > 80)
						{
							worksheet.Column(col).Width = 80;
							worksheet.Column(col).Style.WrapText = true;
							isFormatedMaxCols[col - 1] = true;
						}
					}
					col++;
				}
			}

			for (int i = 0; i <= headers.Count - 1; i++)
			{
				if (!isFormatedMaxCols[i])
				{
					worksheet.Column(i + 1).AutoFit();
				}
			}
		}

		private string GetCellFormatting(Type fieldType)
		{
			if (fieldType == typeof(DateTime)
				|| fieldType == typeof(DateTime?))
			{
				return "dd-mm-yyyy";
			}
			else if (fieldType == typeof(double)
				|| fieldType == typeof(double?)
				|| fieldType == typeof(decimal)
				|| fieldType == typeof(decimal?))
			{
				return "0.00";
			}

			return null;
		}

		private object GetNestedProperties(object original, string properties)
		{
			string[] namesOfProperties = properties.Split('.');
			int size = namesOfProperties.Length - 1;

			PropertyInfo property = original.GetType().GetProperty(namesOfProperties[0]);
			object propValue = property.GetValue(original, null);

			for (int i = 1; i <= size; i++)
			{
				property = propValue.GetType().GetProperty(namesOfProperties[i]);
				propValue = property.GetValue(propValue, null);
			}

			return propValue;
		}

		private void GetHeadersAndMembers<T, TResult>(ref List<string> headers, ref List<MemberExpression> memberExpressions, params Expression<Func<T, TResult>>[] expressions)
		{
			foreach (var item in expressions)
			{
				var expression = item.Body as MemberInitExpression;
				var bindings = expression.Bindings;

				foreach (var binding in bindings)
				{
					dynamic obj = binding;

					var member = obj.Expression as MemberExpression;
					var unary = obj.Expression as UnaryExpression;
					var result = member ?? (unary != null ? unary.Operand as MemberExpression : null);

					if (result == null)
					{
						headers.Add(obj.Expression.Value);
					}
					else
					{
						memberExpressions.Add(result);
					}
				}
			}
		}
	}
}
