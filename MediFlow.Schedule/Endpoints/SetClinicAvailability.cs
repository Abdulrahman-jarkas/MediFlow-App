using Ardalis.Result;
using MediatR;
using MediFlow.Schedule.Requests;
using MediFlow.Schedule.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernal;

namespace MediFlow.Schedule.Endpoints;

public sealed partial class SetClinicAvailability : ISlice
{
	public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
	{
		endpointRouteBuilder.MapPut("api/clinics/{id:guid}/availability",
			async ([FromBody] SetAvailabilityRequest data, [FromRoute] Guid id, IMediator mediator, HttpContext httpContext) =>
			{
				var result = await mediator.Send(new SetClinicAvailabilityCommand()
				{ ClinicId = id, UserId = httpContext.GetUserId(), AvailabilityItems = data.Availabilities.ToCommand() });

				return result.Map(res => res.ToDto()).ToMinimalApiResult();
			})
			.RequireAuthorization(new AuthorizeAttribute { Roles = "clinic" });
	}
}

