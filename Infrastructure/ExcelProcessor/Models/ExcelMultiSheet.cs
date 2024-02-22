using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.ExcelProcessor.Models
{
	public class ExcelMultiSheet<TResult>
	{
		public string SheetName { get; set; }
		public List<object> Items { get; set; }
		public List<Expression<Func<object, TResult>>> Expressions { get; set; }
	}
}
