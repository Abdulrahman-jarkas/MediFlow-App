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

public sealed partial class CreateDoctorProfile : ISlice
{
	public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
	{
		endpointRouteBuilder.MapPost("api/doctors",
			async ([FromBody] CreateDoctorProfileRequest request, IMediator mediator, HttpContext httpContext) =>
			{

				var userId = httpContext.GetUserId();

				var result = (await mediator
				.Send(new CreateDoctorProfileCommand() { Name = request.Name, UserId = userId }))
				.Map(c => new DoctorProfileDto(c.Id, c.Name));

				var location = !result.IsSuccess ? "" : $"/api/doctors/{result.Value.Id}";

				return result.ToMinimalApiResult(location);
			})
			.RequireAuthorization(new AuthorizeAttribute { Roles = "doctor" });
	}
}
