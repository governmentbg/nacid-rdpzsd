using Infrastructure.AppSettings;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Infrastructure.Integrations.RndIntegration.Dtos;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infrastructure.Integrations.RndIntegration
{
	public class RndIntegrationService
	{
		private readonly IHttpClientFactory httpClientFactory;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly DomainValidatorService domainValidatorService;

        public RndIntegrationService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            DomainValidatorService domainValidatorService
        )
        {
            this.httpClientFactory = httpClientFactory;
            this.httpContextAccessor = httpContextAccessor;
            this.domainValidatorService = domainValidatorService;
        }

        public async Task<InstitutionDto> GetMainOrganizationByAuthorizedRepresentative(int? authorizedRepresentativeId = null)
        {
            var requestMessage = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{AppSettingsProvider.ProxyPaths.Rnd}/api/AcadOrganization/MainOrganizationAr{(authorizedRepresentativeId.HasValue ? "?authorizedRepresentativeId=" + authorizedRepresentativeId : "")}")
            };

            if (!authorizedRepresentativeId.HasValue)
            {
                requestMessage.Headers.Add("Authorization", (string)httpContextAccessor.HttpContext.Request.Headers["Authorization"]);
            }

            var client = httpClientFactory.CreateClient();

            using var response = await client.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var searchResult = JsonConvert.DeserializeObject<InstitutionDto>(await response.Content.ReadAsStringAsync());
                return searchResult;
            }
            else
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_UnableToGetRndOrganizationPermissions);
                return null;
            }
        }
    }
}
