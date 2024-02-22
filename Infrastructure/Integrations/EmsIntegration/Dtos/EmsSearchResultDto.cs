using System.Collections.Generic;

namespace Infrastructure.Integrations.EmsIntegration.Dtos
{
	public class EmsSearchResultDto<T> where T : class
	{
		public int TotalCount { get; set; }
		public List<T> Data { get; set; }

		public EmsSearchResultDto()
		{
			Data = new List<T>();
		}
	}
}
