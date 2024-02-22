using System;
using System.Reflection;

namespace Rdpzsd.Models.Attributes
{
	public class SkipAttribute : Attribute
	{
		public static bool IsDeclared(PropertyInfo propertyInfo)
			=> propertyInfo.GetCustomAttribute(typeof(SkipAttribute)) != null;
	}
}
