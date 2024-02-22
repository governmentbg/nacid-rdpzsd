using System.Collections.Generic;

namespace Rdpzsd.Models.Dtos.Base
{
	public class SearchResultDto<T> where T : class
	{
		public int TotalCount { get; set; }
		public List<T> Result { get; set; }

		public SearchResultDto()
		{
			Result = new List<T>();
		}
	}
}
