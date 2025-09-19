using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Marvin.IDP.Services;

namespace MediFlow.IdentityServer.Services
{
	public class LocalUserProfileService : IProfileService
	{
		private readonly ILocalUserService localUserService;

		public LocalUserProfileService(ILocalUserService localUserService)
		{
			this.localUserService = localUserService;
		}

		public async Task GetProfileDataAsync(ProfileDataRequestContext context)
		{
			var subjectId = context.Subject.GetSubjectId();

			var userClaims = await localUserService.GetUserClaimsBySubjectAsync(subjectId);

			context.AddRequestedClaims(
				userClaims.Select(c => new System.Security.Claims.Claim(c.Type, c.Value))
				);
		}

		public async Task IsActiveAsync(IsActiveContext context)
		{
			context.IsActive =  await localUserService.IsUserActive(context.Subject.GetSubjectId());
		}
	}
}
