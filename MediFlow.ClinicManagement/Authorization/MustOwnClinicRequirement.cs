
using Microsoft.AspNetCore.Authorization;

namespace MediFlow.ClinicManagement.Authorization;

public class MustOwnClinicRequirement : IAuthorizationRequirement
{
	public MustOwnClinicRequirement()
	{
	}
}

