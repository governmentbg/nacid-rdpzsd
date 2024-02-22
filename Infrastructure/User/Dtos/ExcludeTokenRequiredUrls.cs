using System.Collections.Generic;

namespace Infrastructure.User.Dtos
{
	public static class ExcludeTokenRequiredUrls
	{
		public readonly static List<string> Urls = new List<string> {
			"/activate", "/recoverPassword", "/Nomenclature/Period/Latest"
		};
	}
}
