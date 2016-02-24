using System;
using System.Configuration;
using System.Linq;

using iGO.API.Authorization;

namespace iGO.API.Authorization
{
	public static class Clients
	{
		private static Lazy<ClientSection> section = new Lazy<ClientSection>(() =>
			(ClientSection)ConfigurationManager.GetSection("apiClients"));

		public static bool VerifyKey(string apiKey)
		{
			return section.Value.Clients.Cast<ClientSection.ClientElement>()
				.Any(ce => ce.ApiKey == apiKey);
		}
	}
}
