using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Domain.Entities;

namespace MediFlow.UsersManagement.UseCases.Clinics.Commands.AddDoctor;

internal class AddDoctorToClinicCommand : IRequest<Result<Clinic>>
{
	public Guid DoctorId { get; set; }
	public Guid ClinicId { get; set; }
	public Guid UserId { get; set; }
}