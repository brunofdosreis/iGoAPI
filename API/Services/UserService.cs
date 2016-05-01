using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;

using Facebook;

using ServiceStack;
using ServiceStack.ServiceInterface;

using iGO.API.Models;
using iGO.Domain.Entities;
using iGO.Repositories;
using iGO.Repositories.Extensions;

namespace iGO.API.Services
{
	public class UserService : BaseService
	{
		public object Post(PostUserRequest Request)
		{
			User User = Request.GetEntity(((NHibernate.ISession)base.Request.Items["hibernateSession"]));

			// TODO: Salvar a foto do perfil localmente

			try
			{
				string accessToken = User.FacebookToken;

				FacebookClient client = new FacebookClient(accessToken);

				dynamic me = client.Get("me", new { fields = new[] { "id", "email", "name", "birthday", "picture.width(480).height(480)", "gender" }});

				ulong facebookId = ulong.Parse(me.id);

				User = new BaseRepository<User>(((NHibernate.ISession)base.Request.Items["hibernateSession"]))
					.List(x => x.FacebookId == facebookId).FirstOrDefault();

				if (User == null)
				{
					User = new User();

					User.FacebookId = facebookId;

					User.FacebookToken = accessToken;

					User.Email = me.email;

					User.Name = me.name;

					User.Birthday = DateTime.ParseExact(me.birthday, "MM/dd/yyyy", null);

					User.Gender = me.gender;

					List<UserPictures> UserPictures = new List<UserPictures>();

					UserPictures.Add(new UserPictures {
						Picture = me.picture.data.url,
						IsDefault = true,
						User = User
					});

					User.UserPictures = UserPictures;

				}
				else
				{
					User.FacebookToken = accessToken;
				}

			}
			catch (FacebookOAuthException)
			{
				throw new HttpError(HttpStatusCode.Unauthorized);
			}

			User.Save(((NHibernate.ISession)base.Request.Items["hibernateSession"]));

			return new BaseResponse();
		}

		[CustomAuthenticateToken]
		public object Get(GetUserRequest Request)
		{
			User User = Request.GetEntity(((NHibernate.ISession)base.Request.Items["hibernateSession"]));

			if (User == null)
			{
				User = base.GetAuthenticatedUser(((NHibernate.ISession)base.Request.Items["hibernateSession"]));
			}

			return new GetUserResponse(User);
		}

		[CustomAuthenticateToken]
		public object Get(GetUserPreferencesRequest Request)
		{
			User User = base.GetAuthenticatedUser(((NHibernate.ISession)base.Request.Items["hibernateSession"]));

			return new GetUserPreferencesResponse(User.UserPreferences);
		}

		[CustomAuthenticateToken]
		public object Put(PutUserPreferencesRequest Request)
		{
			UserPreferences UserPreferences = Request.GetEntity(((NHibernate.ISession)base.Request.Items["hibernateSession"]));

			User User = base.GetAuthenticatedUser(((NHibernate.ISession)base.Request.Items["hibernateSession"]));

			if (User.UserPreferences == null)
			{
				User.UserPreferences = new UserPreferences() { User = User };
			}

			User.UserPreferences.AgeStart = UserPreferences.AgeStart;
			User.UserPreferences.AgeEnd = UserPreferences.AgeEnd;
			User.UserPreferences.Gender = UserPreferences.Gender;

			User.UserPreferences.Save(((NHibernate.ISession)base.Request.Items["hibernateSession"]));

			return new BaseResponse();
		}

		[CustomAuthenticateToken]
		public object Get(GetUserPicturesRequest Request)
		{
			User User = GetAuthenticatedUser(((NHibernate.ISession)base.Request.Items["hibernateSession"]));

			List<string> pictures = new List<string>();

			try
			{
				string accessToken = User.FacebookToken;

				FacebookClient client = new FacebookClient(accessToken);

				dynamic me = client.Get("me", new { fields = new[] { "albums.limit(1000){type,id}" }});

				string facebookId = "";

				if (me.albums != null && me.albums.data != null)
				{
					foreach(dynamic album in me.albums.data)
					{
						if (album.type == "profile")
						{
							facebookId = album.id;

							break;
						}
					}
				}

				if (facebookId != "")
				{
					dynamic album = client.Get(facebookId, new { fields = new[] { "photos.limit(1000){source}" } } );

					if (album.photos != null && album.photos.data != null)
					{
						foreach(dynamic photo in album.photos.data)
						{
							pictures.Add(photo.source);
						}
					}
				}
			}
			catch (FacebookOAuthException)
			{
				throw new HttpError(HttpStatusCode.Unauthorized);
			}

			return new GetUserPicturesResponse(pictures.ToArray());
		}

		[CustomAuthenticateToken]
		public object Put(PutUserPicturesRequest Request)
		{
			User User = GetAuthenticatedUser(((NHibernate.ISession)base.Request.Items["hibernateSession"]));

			List<UserPictures> UserPictures = new List<UserPictures>();

			foreach (PutUserPicturesRequest.Object picture in Request.pictures)
			{
				UserPictures.Add (new UserPictures()
					{
						//Id = picture.ID,
						Picture = picture.picture,
						IsDefault = picture.isDefault,
						User = User
					}
				);
			}

			User.UserPictures = UserPictures;

			User.Save(((NHibernate.ISession)base.Request.Items["hibernateSession"]));

			return new BaseResponse();
		}
	}
}
