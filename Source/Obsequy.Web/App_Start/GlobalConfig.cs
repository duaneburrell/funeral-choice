using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNet.SignalR.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;
using System.Web.Http;

namespace Obsequy.Web
{
	public static class GlobalConfig
	{
		public class SignalRContractResolver : IContractResolver
		{
			private readonly Assembly _assembly;
			private readonly IContractResolver _camelCaseContractResolver;
			private readonly IContractResolver _defaultContractSerializer;

			public SignalRContractResolver()
			{
				_defaultContractSerializer = new DefaultContractResolver();
				_camelCaseContractResolver = new CamelCasePropertyNamesContractResolver();
				_assembly = typeof(Connection).Assembly;
			}

			public JsonContract ResolveContract(Type type)
			{
				if (type.Assembly.Equals(_assembly))
				{
					return _defaultContractSerializer.ResolveContract(type);
				}

				return _camelCaseContractResolver.ResolveContract(type);
			}
		}

		public static void CustomizedConfig(HttpConfiguration config)
		{
			config.Formatters.Remove(config.Formatters.XmlFormatter);

			var json = config.Formatters.JsonFormatter;
			json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			// for SignalR 2.0
			var jsonSerializer = new JsonSerializer()
			{
				ContractResolver = new SignalRContractResolver()
			};

			GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => jsonSerializer);
		}
	}
}