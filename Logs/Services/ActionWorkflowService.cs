using Infrastructure.User;
using Infrastructure.User.Enums;
using Logs.Enums;
using Logs.Interfaces;
using Logs.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Logs.Services
{
    public class ActionWorkflowService<TActionWorkflowOperation>
		where TActionWorkflowOperation : IActionWorkflowOperation, new()
	{
		private readonly LogContext context;
		private readonly IHttpContextAccessor httpContextAccessor;
		private readonly UserContext userContext;
		private readonly TActionWorkflowOperation actionWorkflowOperation = new();

		public ActionWorkflowService(
			LogContext context,
			IHttpContextAccessor httpContextAccessor,
			UserContext userContext
		)
		{
			this.context = context;
			this.httpContextAccessor = httpContextAccessor;
			this.userContext = userContext;
		}

		public async Task LogSearchAction(string customMessage = null)
        {
			var actionWorkflow = ConstructRequestInformation();
			actionWorkflow.Message = !string.IsNullOrWhiteSpace(customMessage) ? $"{actionWorkflowOperation.SearchText} \nДопълнителна информация - {customMessage}" : actionWorkflowOperation.SearchText;
			await context.ActionWorkflows.AddAsync(actionWorkflow);
			await context.SaveChangesAsync();
		}

		public async Task LogGetAction(string customMessage = null)
		{
			var actionWorkflow = ConstructRequestInformation();
			actionWorkflow.Message = !string.IsNullOrWhiteSpace(customMessage) ? $"{actionWorkflowOperation.GetText} \nДопълнителна информация - {customMessage}" : actionWorkflowOperation.GetText;
			await context.ActionWorkflows.AddAsync(actionWorkflow);
			await context.SaveChangesAsync();
		}

		public async Task LogCreateAction(string customMessage = null)
		{
			var actionWorkflow = ConstructRequestInformation();
			actionWorkflow.Message = !string.IsNullOrWhiteSpace(customMessage) ? $"{actionWorkflowOperation.CreateText} \nДопълнителна информация - {customMessage}" : actionWorkflowOperation.CreateText;
			await context.ActionWorkflows.AddAsync(actionWorkflow);
			await context.SaveChangesAsync();
		}

		public async Task LogUpdateAction(string customMessage = null)
		{
			var actionWorkflow = ConstructRequestInformation();
			actionWorkflow.Message = !string.IsNullOrWhiteSpace(customMessage) ? $"{actionWorkflowOperation.UpdateText} \nДопълнителна информация - {customMessage}" : actionWorkflowOperation.UpdateText;
			await context.ActionWorkflows.AddAsync(actionWorkflow);
			await context.SaveChangesAsync();
		}

		public async Task LogDeleteAction(string customMessage = null)
		{
			var actionWorkflow = ConstructRequestInformation();
			actionWorkflow.Message = !string.IsNullOrWhiteSpace(customMessage) ? $"{actionWorkflowOperation.DeleteText} \nДопълнителна информация - {customMessage}" : actionWorkflowOperation.DeleteText;
			await context.ActionWorkflows.AddAsync(actionWorkflow);
			await context.SaveChangesAsync();
		}
		public async Task LogExcelAction(string customMessage = null)
		{
			var actionWorkflow = ConstructRequestInformation();
			actionWorkflow.Message = !string.IsNullOrWhiteSpace(customMessage) ? $"{actionWorkflowOperation.ExcelText} \nДопълнителна информация - {customMessage}" : actionWorkflowOperation.ExcelText ;
			await context.ActionWorkflows.AddAsync(actionWorkflow);
			await context.SaveChangesAsync();
		}
		public async Task LogCustomAction(string customMessage)
        {
			var actionWorkflow = ConstructRequestInformation();
			actionWorkflow.Message = customMessage;
			await context.ActionWorkflows.AddAsync(actionWorkflow);
			await context.SaveChangesAsync();
		}

		private ActionWorkflow ConstructRequestInformation()
		{
			var actionWorkflow = new ActionWorkflow();
			actionWorkflow.EventDate = DateTime.Now;

			if (httpContextAccessor.HttpContext != null && httpContextAccessor.HttpContext.Request != null)
			{
				actionWorkflow.Ip = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
				actionWorkflow.Url = httpContextAccessor.HttpContext.Request.Path.ToUriComponent() + httpContextAccessor.HttpContext.Request.QueryString.ToUriComponent();
				actionWorkflow.Verb = httpContextAccessor.HttpContext.Request.Method;
				actionWorkflow.UserAgent = httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
			}

            if (userContext?.UserType == UserType.Ems || userContext?.UserType == UserType.Rsd)
            {
				actionWorkflow.UserId = userContext.UserId;
				actionWorkflow.Username = userContext.UserFullname;
            }

			actionWorkflow.SystemType = userContext?.UserType == UserType.PublicUser ? SystemType.Public : SystemType.Register;

			return actionWorkflow;
		}
	}
}
