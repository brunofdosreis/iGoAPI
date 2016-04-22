using System;
using System.Linq;
using System.Linq.Expressions;

using NHibernate;
using NHibernate.Linq;

using iGO.Repositories.Configuration;

namespace iGO.Repositories
{
	public class BaseRepository<T>
	{
		private ISession _Session { get; set; }

		protected ISession Session 
		{
			private set
			{
				_Session = value;
			}

			get 
			{
				if (_Session == null)
				{
					_Session = NhibernateManager.GetSession();
				}

				return _Session;
			}
		}

		private ITransaction Transaction { get; set; }

		public BaseRepository()
		{
		}

		public BaseRepository(ISession Session)
		{
			this.Session = Session;
		}

		public virtual void BeginTransaction()
		{
			Transaction = Session.BeginTransaction();
		}

		public virtual void CommitTransaction()
		{
			if (Transaction !=null && !Transaction.WasCommitted && !Transaction.WasRolledBack)
			{
				Transaction.Commit();
			}
		}

		public virtual void Rollbackransaction()
		{
			if (Transaction !=null && !Transaction.WasCommitted && !Transaction.WasRolledBack)
			{
				Transaction.Rollback();
			}
		}

		public virtual void Save(T entity)
		{
			Transaction = Session.BeginTransaction();

			try
			{
				Session.SaveOrUpdate(entity);
				Transaction.Commit();
				Session.Flush();
			}
			catch (Exception ex)
			{
				Transaction.Rollback();
				// log exception
				throw;
			}
		}

		public virtual void Save<T>(T entity)
		{
			Transaction = Session.BeginTransaction();

			try
			{
				Session.SaveOrUpdate(entity);
				Transaction.Commit();
				Session.Flush();
			}
			catch (Exception ex)
			{
				Transaction.Rollback();
				// log exception
				throw;
			}
		}

		public virtual void Delete(T entity)
		{
			Transaction = Session.BeginTransaction();

			try
			{
				Session.Delete(entity);
				Transaction.Commit();
				Session.Flush();
			}
			catch (Exception ex)
			{
				Transaction.Rollback();
				// log exception
				throw;
			}
		}

		public virtual void Delete<T>(T entity)
		{
			Transaction = Session.BeginTransaction();

			try
			{
				Session.Delete(entity);
				Transaction.Commit();
				Session.Flush();
			}
			catch (Exception ex)
			{
				Transaction.Rollback();
				// log exception
				throw;
			}
		}

		public virtual T Get(int Id)
		{
			return Session.Get<T>(Id);
		}

		public virtual T Get<T>(int Id)
		{
			return Session.Get<T>(Id);
		}

		public virtual IQueryable<T> List()
		{
			return Session.Query<T>();
		}

		public virtual IQueryable<T> List(Expression<Func<T, bool>> restriction)
		{
			return Session.Query<T>().Where(restriction);
		}
	}
}
	