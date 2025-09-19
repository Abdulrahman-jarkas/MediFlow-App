using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Domain.ValueObjects;
using MediFlow.Schedule.Requests;
using MediFlow.Schedule.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernal;

namespace MediFlow.Schedule.Endpoints;

public sealed partial class SetDoctorAvailability : ISlice
{
	public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
	{
		endpointRouteBuilder.MapPut("api/doctors/{id:guid}/availability",
			async ([FromBody] SetAvailabilityRequest data, [FromRoute] Guid id, IMediator mediator, HttpContext httpContext) =>
			{
				var result = await mediator.Send(new SetDoctorAvailabilityCommand()
				{ DoctorId = id, UserId = httpContext.GetUserId(), AvailabilityItems = data.Availabilities.ToCommand() });

				return result.Map(result => result.ToDto()).ToMinimalApiResult();
			})
			.RequireAuthorization(new AuthorizeAttribute { Roles = "doctor" });
	}
}
