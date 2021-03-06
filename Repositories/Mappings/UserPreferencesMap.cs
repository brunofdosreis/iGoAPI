﻿using System;

using FluentNHibernate.Mapping;

using iGO.Domain.Entities;

namespace iGO.Repositories.Mappings
{
	public class UserPreferencesMap : ClassMap<UserPreferences>
	{
		public UserPreferencesMap ()
		{
			Id(x => x.Id);
			Map(x => x.Created);
			Map(x => x.Updated);
			Map(x => x.AgeStart);
			Map(x => x.AgeEnd);
			HasMany(x => x.Gender).Table("UserPreferencesGender").Element("Gender").Cascade.All();

			References(x => x.User).Cascade.All();
		}
	}
}
