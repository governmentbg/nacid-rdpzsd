using Logs.Enums;
using Logs.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Logs.Services
{
	public class LogService
	{
		private readonly LogContext logContext;

		public LogService(LogContext logContext)
		{
			this.logContext = logContext;
		}

		public async Task LogServerException(Exception exception, int? userId, LogType logType, HttpRequest request = null, string customText = null)
		{
			var message = $"CustomText: {customText} \n\nType: {exception.GetType().FullName} \n\nMessage: {exception.Message} \n\nStackTrace: {exception.StackTrace}";

			var log = new Log {
				Type = logType,
				LogDate = DateTime.Now,
				Ip = request != null ? request.HttpContext.Connection.RemoteIpAddress.ToString() : string.Empty,
				Verb = request != null ? request.Method : string.Empty,
				Url = request != null ? request.Path.ToUriComponent() : string.Empty,
				UserAgent = request != null ? request.Headers["User-Agent"].ToString() : string.Empty,
				Message = message,
				UserId = userId
			};

			await SaveLog(log);
		}

		private async Task SaveLog(Log log)
		{
			await logContext.Logs.AddAsync(log);
			await logContext.SaveChangesAsync();
		}
	}
}
