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
			User User = Request.GetEntity();

			// TODO: Salvar a foto do perfil localmente

			try {

				string accessToken = User.FacebookToken;

				FacebookClient client = new FacebookClient(accessToken);

				dynamic me = client.Get("me", new { fields = new[] { "id", "email", "name", "birthday", "picture", "gender" }});

				ulong facebookId = ulong.Parse(me.id);

				User = new BaseRepository<User>().List(x => x.FacebookId == facebookId).FirstOrDefault();

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

				} else {

					User.FacebookToken = accessToken;
				}

			} catch (FacebookOAuthException) {

				throw new HttpError(HttpStatusCode.Unauthorized);
			}

			User.Save();

			return new BaseResponse();
		}

		[CustomAuthenticateToken]
		public object Put(PutUserRequest Request)
		{
			UserPictures DefaultPictureNew = Request.GetEntity();

			UserPictures DefaultPictureOld = new BaseRepository<UserPictures>().List(x => x.IsDefault).FirstOrDefault();

			DefaultPictureOld.IsDefault = false;
			DefaultPictureOld.Save();

			DefaultPictureNew.IsDefault = true;
			DefaultPictureNew.Save();

			return new BaseResponse();
		}

		[CustomAuthenticateToken]
		public object Get(GetUserRequest Request)
		{
			User User = Request.GetEntity();

			if (User == null)
			{
				User = base.GetAuthenticatedUser();
			}

			return new GetUserResponse(User);
		}

		[CustomAuthenticateToken]
		public object Get(GetUserPreferencesRequest Request)
		{
			User User = base.GetAuthenticatedUser();

			return new GetUserPreferencesResponse(User.UserPreferences);
		}

		[CustomAuthenticateToken]
		public object Put(PutUserPreferencesRequest Request)
		{
			UserPreferences UserPreferences = Request.GetEntity();

			User User = base.GetAuthenticatedUser();

			if (User.UserPreferences == null)
			{
				User.UserPreferences = new UserPreferences() { User = User };
			}

			User.UserPreferences.AgeStart = UserPreferences.AgeStart;
			User.UserPreferences.AgeEnd = UserPreferences.AgeEnd;
			User.UserPreferences.Gender = UserPreferences.Gender;

			User.UserPreferences.Save();

			return new BaseResponse();
		}
	}
}
