using Infrastructure.DomainValidation.Models;
using Infrastructure.User;
using Logs.Enums;
using Logs.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Middlewares
{
	public class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate next;

		public ErrorHandlingMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task Invoke(HttpContext context, LogService logService, UserContext userContext)
		{
			try
			{
				await next(context);
			}
			catch (Exception exception)
			{
				while (exception.InnerException != null)
				{ exception = exception.InnerException; }

				if (exception is DomainErrorException domainErrorException)
				{
					var responseErrorMessage = new ResponseErrorMessage {
						ErrorMessages = domainErrorException.ErrorMessages
					};

					await HandleException(context, HttpStatusCode.UnprocessableEntity, responseErrorMessage);
				}
				else
				{
					await logService.LogServerException(exception, userContext?.UserId, LogType.ServerExeptionLog, context.Request);
					await HandleException(context, HttpStatusCode.InternalServerError);
				}
			}
		}

		private static Task HandleException(HttpContext context, HttpStatusCode status, ResponseErrorMessage responseErrorMessage = null)
		{
			context.Response.Clear();
			context.Response.StatusCode = (int)status;
			context.Response.ContentType = "application/json";
			context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
			return context.Response.WriteAsync(JsonConvert.SerializeObject(responseErrorMessage, new JsonSerializerSettings {
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = new List<JsonConverter>() { new Newtonsoft.Json.Converters.StringEnumConverter() }
			}));
		}
	}
}
