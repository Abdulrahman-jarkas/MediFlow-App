using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Requests.Equipments;
using MediFlow.ClinicManagement.UseCases.Doctors.Commands;
using MediFlow.DoctorManagement.Respones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernal;

namespace MediFlow.ClinicManagement.Endpoints.Doctor;

public sealed partial class CreateClinicProfile : ISlice
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
