using System.ComponentModel;

namespace Rdpzsd.Models.Models.Rdpzsd.Migration
{
    public enum IdentitfierChangeAction
    {
		[Description("Смяна типа от егн към лнч")]
		FromUinToForeignNumber = 1, 

		[Description("Смяна типа от егн към идн")]
		FromUinToIdnNumber = 2,

		[Description("Смяна типа от лнч към егн")]
		FromForeignNumbertoUin = 3, 

		[Description("Смяна типа от лнч към идн")]
		FromForeignNumberToIdnNumber = 4,

		[Description("Смяна типа от идн към егн")]
		FromIdnNumberToUin = 5, 

		[Description("Смяна типа от идн към лнч")]
		FromIdnNumberToForeignNumber = 6, 
	}
}
