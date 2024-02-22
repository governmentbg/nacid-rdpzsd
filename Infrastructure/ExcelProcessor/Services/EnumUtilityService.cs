using System;
using System.ComponentModel;
using System.Reflection;

namespace Infrastructure.ExcelProcessor.Services
{
	public class EnumUtilityService
	{
		public string GetDescription(object value)
		{
			FieldInfo fieldInfo = value.GetType()
				.GetField(value.ToString());

			DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo
				.GetCustomAttributes(typeof(DescriptionAttribute), false);

			if (attributes != null
				&& attributes.Length > 0)
			{
				return attributes[0].Description;
			}
			else
			{
				return value.ToString();
			}
		}

		public static string GetDescriptionSt(object value)
		{
			FieldInfo fieldInfo = value.GetType()
				.GetField(value.ToString());

			DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo
				.GetCustomAttributes(typeof(DescriptionAttribute), false);

			if (attributes != null
				&& attributes.Length > 0)
			{
				return attributes[0].Description;
			}
			else
			{
				return value.ToString();
			}
		}

		public static T ParseEnum<T>(string value)
		{
			return (T)Enum.Parse(typeof(T), value, true);
		}
	}
}
