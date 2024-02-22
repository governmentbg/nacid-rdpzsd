using Infrastructure.AppSettings;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Infrastructure.Integrations.EmsIntegration.Dtos;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Integrations.EmsIntegration
{
	public class EmsIntegrationService
	{
		private readonly IHttpClientFactory httpClientFactory;
		private readonly IHttpContextAccessor httpContextAccessor;
		private readonly DomainValidatorService domainValidatorService;

		public EmsIntegrationService(
			IHttpClientFactory httpClientFactory,
			IHttpContextAccessor httpContextAccessor,
			DomainValidatorService domainValidatorService
		)
		{
			this.httpClientFactory = httpClientFactory;
			this.httpContextAccessor = httpContextAccessor;
			this.domainValidatorService = domainValidatorService;
		}

		public async Task<List<UserEmsDto>> GetUsersInfo(List<int> UserIds)
		{
			var userEmsFilter = new UserEmsFilterDto {
				UserIds = UserIds,
				ReturnAllUsers = true,
				Offset = 0,
				Limit = int.MaxValue
			};

			var requestMessage = new HttpRequestMessage {
				Method = HttpMethod.Post,
				RequestUri = new Uri($"{AppSettingsProvider.ProxyPaths.Ems}/api/users/filtered"),
				Content = new StringContent(JsonConvert.SerializeObject(userEmsFilter), Encoding.UTF8, "application/json")
			};

			requestMessage.Headers.Add("Authorization", (string)httpContextAccessor.HttpContext.Request.Headers["Authorization"]);

			var client = httpClientFactory.CreateClient();

			using var response = await client.SendAsync(requestMessage);

			if (response.IsSuccessStatusCode)
			{
				var emsSearchResult = JsonConvert.DeserializeObject<EmsSearchResultDto<UserEmsDto>>(await response.Content.ReadAsStringAsync());
				return emsSearchResult.Data;
			}
			else
			{
				domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_UnableToGetEmsUsers);
				return null;
			}
		}
	}
}
