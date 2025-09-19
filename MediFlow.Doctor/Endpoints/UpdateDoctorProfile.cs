using Ardalis.Result;
using MediatR;
using MediFlow.DoctorManagement.Requests;
using MediFlow.DoctorManagement.Respones;
using MediFlow.DoctorManagement.UseCases.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernal;

namespace MediFlow.DoctorManagement.Endpoints;

public sealed partial class UpdateDoctorProfile : ISlice
{
	public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
	{
		endpointRouteBuilder.MapPut("api/doctors/{doctorId:guid}",
			async (
				[FromBody] UpdateDoctorProfileRequest data,
				[FromRoute] Guid doctorId,
				HttpContext httpContext,
				IMediator mediator) =>
			{
				var result = (await mediator
								.Send(new UpdateDoctorProfileCommand() { Id = doctorId, Name = data.Name, UserId = httpContext.GetUserId() }))
								.Map(c => new DoctorProfileDto(c.Id, c.Name));

				return result.ToMinimalApiResult();
			})
			.RequireAuthorization(new AuthorizeAttribute { Roles = "doctor" });
	}
}
