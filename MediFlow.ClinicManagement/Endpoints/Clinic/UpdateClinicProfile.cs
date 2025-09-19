using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Requests.Profile;
using MediFlow.ClinicManagement.Respones;
using MediFlow.ClinicManagement.UseCases.Clinics.Commands.UpdateClinic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernal;

namespace MediFlow.ClinicManagement.Endpoints.Clinic;

public sealed partial class UpdateClinicProfile : ISlice
{
	public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
	{
		endpointRouteBuilder.MapPut("api/clinics/{clinicId:guid}",
			async (
				[FromBody] UpdateClinicProfileRequest data,
				[FromRoute] Guid clinicId,
				HttpContext httpContext,
				IMediator mediator) =>
			{
				var result = (await mediator
								.Send(new UpdateClinicProfileCommand() { Id = clinicId, Name = data.Name, UserId = httpContext.GetUserId() }))
								.Map(c => new ClinicProfileDto(c.Id, c.Name));

				return result.ToMinimalApiResult();
			})
			.RequireAuthorization(new AuthorizeAttribute { Roles = "clinic" });
	}
}
