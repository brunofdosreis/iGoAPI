using System;
using System.Configuration;

namespace iGO.API.Authorization
{
	public class ClientSection : ConfigurationSection
	{
		[ConfigurationProperty("clients")]
		public ClientElementCollection Clients
		{
			get { return (ClientElementCollection)base["clients"]; }
		}

		public class ClientElement : ConfigurationElement
		{
			[ConfigurationProperty("apiKey", IsRequired = true)]
			public string ApiKey
			{
				get { return (string)base["apiKey"]; }
				set { base["apiKey"] = value; }
			}

			[ConfigurationProperty("name", IsRequired = true)]
			public string Name
			{
				get { return (string)base["name"]; }
				set { base["name"] = value; }
			}
		}

		public class ClientElementCollection : ConfigurationElementCollection
		{
			public override ConfigurationElementCollectionType CollectionType
			{
				get { return ConfigurationElementCollectionType.BasicMap; }
			}

			public ClientElement this[object key]
			{
				get { return this.BaseGet(key) as ClientElement; }
			}

			protected override ConfigurationElement CreateNewElement()
			{
				return new ClientElement();
			}

			protected override object GetElementKey(ConfigurationElement element)
			{
				return ((ClientElement)element).Name;
			}

			protected override bool IsElementName(string elementName)
			{
				return !string.IsNullOrEmpty(elementName) && elementName.Equals("client");
			}
		}
	}
}
