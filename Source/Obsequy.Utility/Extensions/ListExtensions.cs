using System;
using System.Collections.Generic;

namespace Obsequy.Utility
{
	public static class ListExtension
	{
		public static bool ContainsPropertyValue<T>(this List<T> list, string id, string propertyName)
		{
			if (!string.IsNullOrEmpty(id))
				foreach (var item in list)
					if (id.Equals(item.GetType().GetProperty(propertyName).GetValue(item, null)))
						return true;
			
			return false;
		}
	}
}
