using System;
using System.Reflection;

namespace Rdpzsd.Models.Attributes
{
    public class SkipUpdateAttribute : Attribute
	{
		public static bool IsDeclared(PropertyInfo propertyInfo)
			=> propertyInfo.GetCustomAttribute(typeof(SkipUpdateAttribute)) != null;
	}
}
