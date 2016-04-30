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
			Map(x => x.Updated);
			Map(x => x.Text);

			References(x => x.Match).Cascade.All();
			References(x => x.FromUser).Cascade.All();
			References(x => x.ToUser).Cascade.All();
		}
	}
}
