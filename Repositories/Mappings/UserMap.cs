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
			Map(x => x.Updated);
			Map(x => x.FacebookId);
			Map(x => x.FacebookToken);
			Map(x => x.Email);
			Map(x => x.Name);
			Map(x => x.Birthday);
			Map(x => x.Gender);

			References(x => x.UserPreferences).Cascade.All();

			HasMany(x => x.UserPictures).Cascade.All();

			HasMany(x => x.DeviceToken).Cascade.All();

			HasManyToMany(x => x.Event).Table("UserEvent").Cascade.All();

			HasMany(x => x.FirstUserMatch)
				.KeyColumns.Add("FirstUser_id");

			HasMany(x => x.SecondUserMatch)
				.KeyColumns.Add("SecondUser_id");
		}
	}
}
