using System;

using FluentNHibernate.Mapping;

using iGO.Domain.Entities;

namespace iGO.Repositories.Mappings
{
	public class MessageMap : ClassMap<Message>
	{
		public MessageMap ()
		{
			Id(x => x.Id);
			Map(x => x.Created);
			Map(x => x.Text);

			References(x => x.Match);
			References(x => x.FromUser);
			References(x => x.ToUser);
		}
	}
}
