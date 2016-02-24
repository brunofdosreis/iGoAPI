using System;

using FluentNHibernate.Mapping;

using iGO.Domain.Entities;

namespace iGO.Repositories.Mappings
{
	public class UserMap : ClassMap<User>
	{
		public UserMap ()
		{
			Id(x => x.Id);
			Map(x => x.Created);
			Map(x => x.FacebookToken);
			Map(x => x.Email);
			Map(x => x.Name);
			Map(x => x.Birthday);

			References(x => x.UserPreferences);

			HasMany(x => x.UserPictures);

			HasManyToMany(x => x.Event)
				.Table("UserEvent");

			HasMany(x => x.Match);
		}
	}
}
