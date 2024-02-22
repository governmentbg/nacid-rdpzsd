using Infrastructure.AppSettings;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Infrastructure.Integrations.RdDocumentsIntegration.Dtos;
using Infrastructure.Integrations.RsoDocumentsIntegration.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infrastructure.Integrations.RdDocumentsIntegration
{
    public class RsoDocumentsIntegrationService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly DomainValidatorService domainValidatorService;

        public RsoDocumentsIntegrationService(
            IHttpClientFactory httpClientFactory,
            DomainValidatorService domainValidatorService
        )
        {
            this.httpClientFactory = httpClientFactory;
            this.domainValidatorService = domainValidatorService;
        }

        public async Task<List<RsoDocumentDto>> GetRsoDocuments(string studentUinFn)
        {
            if (AppSettingsProvider.EnableRsoIntegration) {
                try
                {
                    var requestMessage = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"{AppSettingsProvider.ProxyPaths.RsoDocuments}/{studentUinFn}")
                    };

                    var client = httpClientFactory.CreateClient(AppSettingsProvider.RsoIntegration.RsoHttpClient);

                    using var response = await client.SendAsync(requestMessage);

                    if (response.IsSuccessStatusCode)
                    {
                        var searchResult = JsonConvert.DeserializeObject<List<RsoDocumentDto>>(await response.Content.ReadAsStringAsync());
                        return searchResult;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<List<RsoImageDto>> GetRsoDocumentsImages(double? intId)
        {
            if (AppSettingsProvider.EnableRsoIntegration)
            {
                try
                {
                    var requestMessage = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"{AppSettingsProvider.ProxyPaths.RsoImages}/{intId}")
                    };

                    var client = httpClientFactory.CreateClient(AppSettingsProvider.RsoIntegration.RsoHttpClient);

                    using var response = await client.SendAsync(requestMessage);

                    if (response.IsSuccessStatusCode)
                    {
                        var searchResult = JsonConvert.DeserializeObject<List<RsoImageDto>>(await response.Content.ReadAsStringAsync());
                        return searchResult;
                    }
                    else
                    {
                        domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_UnableToConnectToRdDocuments);
                        return null;
                    }
                }
                catch (Exception)
                {
                    domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_UnableToConnectToRdDocuments);
                    return null;
                }
            } else
            {
                return null;
            }
        }
    }
}
