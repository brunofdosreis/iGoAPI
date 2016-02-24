using System.Linq;

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

			// TODO: Comunicação com o Facebook - Obter Nome, Email, Foto de Perfil, Data de Nascimento, Gênero

			User.Save();

			return new PostUserResponse(User);
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

			User.UserPreferences.AgeStart = UserPreferences.AgeStart;
			User.UserPreferences.AgeEnd = UserPreferences.AgeEnd;

			User.UserPreferences.Save();

			return new BaseResponse();
		}
	}
}
