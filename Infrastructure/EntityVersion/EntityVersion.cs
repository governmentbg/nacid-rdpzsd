using Rdpzsd.Models.Interfaces;

namespace Rdpzsd.Models.Models.Base
{
	public abstract class EntityVersion : IEntityVersion
	{
		public int Id { get; set; }
		public int Version { get; set; }
	}
}
