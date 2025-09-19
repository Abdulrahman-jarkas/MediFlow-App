using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Requests.Profile;
using MediFlow.UsersManagement.Respones;
using MediFlow.UsersManagement.UseCases.Clinics.Commands.AddDoctor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernal;

namespace MediFlow.ClinicManagement.Endpoints.Clinic;

public sealed partial class AddDoctorToClinic : ISlice
{
	public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
	{
		endpointRouteBuilder.MapPost("api/clinics/{clinicId:guid}/doctors",
			async ([FromBody] AddDoctorToClinicRequest request,
			ISender sender,
			[FromRoute] Guid clinicId,
			HttpContext httpContext) =>
			{

				var command = new AddDoctorToClinicCommand()
				{ DoctorId = request.DoctorId, ClinicId = clinicId, UserId = httpContext.GetUserId() };

				var result = await sender.Send(command);

				return result.Map(result => result.ToDto()).ToMinimalApiResult();
			});
	}
}