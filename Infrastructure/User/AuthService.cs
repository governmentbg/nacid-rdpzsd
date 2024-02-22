using Infrastructure.AppSettings;
using Infrastructure.User.Dtos;
using Infrastructure.User.Enums;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infrastructure.User
{
	public class AuthService
	{
		private readonly IHttpClientFactory httpClientFactory;

		public const string UserContextKey = "UserContext";
		private const string AuthHeader = "Authorization";

		public AuthService(
			IHttpClientFactory httpClientFactory
		)
		{
			this.httpClientFactory = httpClientFactory;
		}

		public async Task<UserContext> Authenticate(HttpContext context, string path, string authToken = null)
		{
			var requestMessage = new HttpRequestMessage {
				Method = HttpMethod.Get,
				RequestUri = new Uri($"{AppSettingsProvider.ProxyPaths.Ems}/api/auth")
			};

			if (authToken == null && context.Request.Headers[AuthHeader].FirstOrDefault() == null && !ExcludeTokenRequiredUrls.Urls.Any(e => path.EndsWith(e)))
			{
				return new UserContext { UserType = UserType.Unauthorized };
			}
			else
			{
				var authHeader = authToken != null
					? $"Bearer {authToken}"
					: context.Request.Headers[AuthHeader].FirstOrDefault();

				requestMessage.Headers
						.Add(AuthHeader, authHeader ?? "Bearer");

				var client = httpClientFactory.CreateClient();

				using var responseMessage = await client.SendAsync(requestMessage);
				if (responseMessage.StatusCode != HttpStatusCode.OK)
				{
					if (ExcludeTokenRequiredUrls.Urls.Any(e => path.EndsWith(e)))
					{
						// This is used for services which can be reached without token
						return new UserContext { UserType = UserType.PublicUser };
					}
					else
					{
						return new UserContext { UserType = UserType.Unauthorized };
					}
				}
				else
				{
					var response = await responseMessage.Content.ReadAsStringAsync();
					var result = JsonConvert.DeserializeObject<AuthResponseDto>(response);
					var (userId, userFullname, permissions, userType) = ParseAuthResponse(result);
					return new UserContext(userId, userFullname, permissions, userType);
				}
			}
		}

		public async Task<(HttpStatusCode code, TokenResponseDto response)> Login(HttpContext context)
		{
			var requestMessage = new HttpRequestMessage {
				Content = new StreamContent(context.Request.Body),
				Method = HttpMethod.Post,
				RequestUri = new Uri($"{AppSettingsProvider.ProxyPaths.Ems}/api/token")
			};

			var client = httpClientFactory.CreateClient();

			using var responseMessage = await client.SendAsync(requestMessage);
			if (responseMessage.StatusCode != HttpStatusCode.OK)
			{
				return (responseMessage.StatusCode, null);
			}
			else
			{
				var response = await responseMessage.Content.ReadAsStringAsync();

				var result = JsonConvert.DeserializeObject<TokenResponseDto>(response);
				return (HttpStatusCode.OK, result);
			}
		}

		private (int userId, string userFullname, List<string> permissions, string userType) ParseAuthResponse(AuthResponseDto result)
		{
			var permissions = JsonConvert.DeserializeObject<List<string>>(result.Permissions);

			return (result.Id, result.UserFullname, permissions, result.UserType);
		}
	}
}
