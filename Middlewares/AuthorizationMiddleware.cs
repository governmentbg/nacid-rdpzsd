using Infrastructure.DomainValidation.Models;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Infrastructure.Integrations.RndIntegration;
using Infrastructure.User;
using Infrastructure.User.Enums;
using Logs.Dtos;
using Logs.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Middlewares
{
	public class AuthorizationMiddleware
	{
		private readonly RequestDelegate next;

		public AuthorizationMiddleware(
			RequestDelegate next
			)
		{
			this.next = next;
		}

		public async Task Invoke(HttpContext context, AuthService authService, RndIntegrationService rndIntegrationService)
		{
			var path = context.Request.Path.Value;

			if (path.Contains("/api"))
			{
				if (path.Contains("/token"))
				{
					context.Request.EnableBuffering();
					var actionWorkflowService = context.RequestServices.GetService(typeof(ActionWorkflowService<AwUserDto>)) as ActionWorkflowService<AwUserDto>;

					string requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
					requestBody = requestBody.Substring(requestBody.IndexOf('=') + 1, requestBody.IndexOf('&') - requestBody.IndexOf('=') - 1);

					context.Request.Body.Position = 0;

					var (authorizeStatus, tokenResponse) = await authService.Login(context);
					if (authorizeStatus != HttpStatusCode.OK)
					{
						await actionWorkflowService.LogCustomAction($"Неуспешен опит за вход, потребителско име - {requestBody}");
						context.Response.StatusCode = (int)authorizeStatus;
					}
					else
					{
						await actionWorkflowService.LogCustomAction($"Вход в системата, потребителско име - {requestBody}");

						await HandleAuthentication(authService, rndIntegrationService, context, path, tokenResponse.Access_token);
						var serializedToken = JsonConvert.SerializeObject(tokenResponse,
							new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
						await context.Response.Body.WriteAsync(Encoding.ASCII.GetBytes(serializedToken));
					}
				}
				else
				{
					await HandleAuthentication(authService, rndIntegrationService, context, path);
				}
			}
			else
			{
				await next(context);
			}
		}

		private async Task HandleAuthentication(AuthService authService, RndIntegrationService rndIntegrationService, HttpContext context, string path, string authToken = null)
		{
			var user = await authService.Authenticate(context, path, authToken);

			if (context.Response.StatusCode != (int)HttpStatusCode.Unauthorized
				&& context.Response.StatusCode != (int)HttpStatusCode.BadRequest)
			{
				if (user.UserType == UserType.Unauthorized)
				{
					if (path.Contains("/token"))
					{
						context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
					}
					else
					{
						context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
					}
				}
				else if (!path.Contains("/token"))
				{
					if (user.UserType == UserType.Rsd)
					{
						try
						{
							user.Institution = await rndIntegrationService.GetMainOrganizationByAuthorizedRepresentative(user.UserId);
						}
						catch (Exception)
						{
							user.Institution = null;
						}

						if (user.Institution == null)
						{
							user.UserType = UserType.Unauthorized;
							context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
						}
					}

					user.ConstructUserEducationalQualifications();

					if (user.UserType != UserType.Unauthorized) {
						context.Items[AuthService.UserContextKey] = user;
						await next(context);
					}
				}
			}
		}
	}
}
