using System.ComponentModel;

namespace Rdpzsd.Models.Enums.Rdpzsd
{
    [Description("Статус на партиди очакващи одобрение")]
	public enum ApprovalState
	{
		[Description("Чакащи")]
		Pending = 1,

		[Description("Отказани")]
		Canceled = 2,

		[Description("Липсва документ за самоличност")]
		MissingPassportCopy = 3
	}
}
