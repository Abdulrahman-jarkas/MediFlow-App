using MediFlow.ClinicManagement.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.ClinicManagement.Authorization;

public class MustOwnClinicHandler : AuthorizationHandler<MustOwnClinicRequirement>
{
	private readonly UsersManagementDbContext _clinicsDbContext;
	private readonly IHttpContextAccessor _accessor;

	public MustOwnClinicHandler(UsersManagementDbContext clinicsDbContext, IHttpContextAccessor accessor)
	{
		_clinicsDbContext = clinicsDbContext;
		_accessor = accessor;
	}

	protected override async Task HandleRequirementAsync(
		AuthorizationHandlerContext context,
		MustOwnClinicRequirement requirement)
	{
		if (_accessor.HttpContext == null)
		{
			context.Fail();
			return;
		}

		var clinicId = _accessor.HttpContext.GetRouteValue("clinicId");
		if (clinicId == null || !Guid.TryParse(clinicId.ToString(), out var clinicIdAsGuid))
		{
			context.Fail();
			return;
		}

		var clinic = await _clinicsDbContext.Clinics.FirstOrDefaultAsync(c => c.Id == clinicIdAsGuid);
		if (clinic == null)
		{
			return;
		}

		var userId = context.User?.Claims?.FirstOrDefault(c => c.Type == "sub")?.Value;
		if (!Guid.TryParse(userId, out var ownerIdAsGuid))
		{
			context.Fail();
			return;
		}

		if(clinic.UserId != ownerIdAsGuid)
		{
			context.Fail();
			return;
		}

		context.Succeed(requirement);
		return;
	}
}