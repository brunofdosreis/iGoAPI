using System.Reflection;
using System.Configuration;

using NHibernate;
using NHibernate.Tool.hbm2ddl;

using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;

namespace iGO.Repositories.Configuration
{
	public class NhibernateManager
	{
		private static ISession Session { get; set; }

		public static void CreateDatabase()
		{
			if (Session != null)
			{
				//Session.Close();

				Session = null;
			}

			Fluently.Configure()
				.Database(
					MySQLConfiguration.Standard.ConnectionString(
						ConfigurationManager.ConnectionStrings["DataConnectionSting"].ConnectionString
					)
				)
				.Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
				.ExposeConfiguration(c => new SchemaExport(c).Create(true, true))
				.BuildSessionFactory()
				.OpenSession();
				//.Close();
		}

		public static ISession GetSession()
		{
			if (Session == null || !Session.IsOpen)
			{
				Session = Fluently.Configure()
					.Database(
						MySQLConfiguration.Standard.ConnectionString(
							ConfigurationManager.ConnectionStrings["DataConnectionSting"].ConnectionString
						)
					)
					.Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
					.BuildSessionFactory()
					.OpenSession();
			}

			return Session;
		}
	}
}
