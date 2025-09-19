using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Requests.Profile;
using MediFlow.ClinicManagement.Respones;
using MediFlow.ClinicManagement.UseCases.Clinics.Commands.Create;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernal;

namespace MediFlow.ClinicManagement.Endpoints.Clinic;

public sealed partial class CreateClinicProfile : ISlice
{
	public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
	{
		endpointRouteBuilder.MapPost("api/clinics",
			async ([FromBody] CreateClinicProfileRequest request, IMediator mediator, HttpContext httpContext) =>
			{

				var userId = httpContext.GetUserId();

				var result = (await mediator
				.Send(new CreateClinicProfileCommand() { Name = request.Name, UserId = userId }))
				.Map(c => new ClinicProfileDto(c.Id, c.Name));

				var location = !result.IsSuccess ? "" : $"/api/clinics/{result.Value.Id}";

				return result.ToMinimalApiResult(location);
			})
			.RequireAuthorization(new AuthorizeAttribute { Roles = "clinic" });
	}
}