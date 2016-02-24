using System.Linq;

using ServiceStack.ServiceInterface;

using iGO.API.Models;
using iGO.Domain.Entities;
using iGO.Repositories;
using iGO.Repositories.Extensions;

namespace iGO.API.Services
{
	[CustomAuthenticateToken]
	public class EventService : BaseService
	{
		public object Get(GetEventsRequest Request)
		{
			User User = base.GetAuthenticatedUser();

			// TODO: Comunicação com o Facebook - Sincronizar Eventos do Usuário e verificar outros Usuários também

			return new GetEventsResponse(User.Event);
		}
	}
}
