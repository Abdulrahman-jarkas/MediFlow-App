using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Data;
using MediFlow.ClinicManagement.Domain.Entities;

namespace MediFlow.ClinicManagement.UseCases.Clinics.Commands.Create;


internal class CreateClinicProfileCommandHandler : IRequestHandler<CreateClinicProfileCommand, Result<Clinic>>
{
	private readonly UsersManagementDbContext _context;

	public CreateClinicProfileCommandHandler(UsersManagementDbContext clinicDbContext)
	{
		_context = clinicDbContext;
	}

	public async Task<Result<Clinic>> Handle(CreateClinicProfileCommand request, CancellationToken cancellationToken)
	{
		Result<Clinic> clinic = Clinic.Create(request.UserId, request.Name);

		if (!clinic.Errors.Any())
		{
			await _context.Clinics.AddAsync(clinic, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);
		}

		return clinic;
	}
}
