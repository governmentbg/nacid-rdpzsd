using Logs.Enums;
using System;

namespace Logs.Models
{
	public class ActionWorkflow
	{
		public int Id { get; set; }

		public DateTime EventDate { get; set; }
		public string Ip { get; set; }
		public string Verb { get; set; }
		public string Url { get; set; }
		public string UserAgent { get; set; }

		public SystemType SystemType { get; set; }
		public int? UserId { get; set; }
		public string Username { get; set; }
		public string Message { get; set; }
	}
}
