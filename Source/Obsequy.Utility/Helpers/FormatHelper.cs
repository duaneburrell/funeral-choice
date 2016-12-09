using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Obsequy.Utility
{
    public static class FormatHelper
    {
        public static string ToCamlCase(string value)
        {
            var result = string.Empty;

            if (!string.IsNullOrEmpty(value))
                result = string.Format("{0}{1}", Char.ToLowerInvariant(value[0]), (value.Length > 1 ? value.Substring(1) : string.Empty));

            return result;
        }

		public static string ToUserReferenceName(string firstName, string lastName)
		{
			if (firstName != null && lastName != null)
				return string.Format("{0} {1}.", firstName, lastName[0]);
			return "???";
		}

		public static string ToJsonString(object data)
		{
			return JsonConvert.SerializeObject(data, Formatting.None, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
		}
    }
}
