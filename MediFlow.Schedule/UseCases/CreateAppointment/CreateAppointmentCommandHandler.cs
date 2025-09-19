using Ardalis.Result;
using MediatR;
using MediFlow.Schedule.Data;
using MediFlow.Schedule.Domain;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Schedule.UseCases.CreateAppointment;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Result<Appointment>>
{
	private readonly ScheduleDbContext _context;

	public CreateAppointmentCommandHandler(ScheduleDbContext context)
	{
		_context = context;
	}

	public async Task<Result<Appointment>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
	{
		var patient = await _context.Patients
			.FirstOrDefaultAsync(res => res.Id == request.PatientId && res.UserId == request.UserId);

		if (patient == null)
			return Result.NotFound("Patient is not exist");

		var clinicDoctor = await _context.ClinicsDoctors
			.FirstOrDefaultAsync(res => res.ClinicId == request.ClinicId && res.DoctorId == request.DoctorId);

		if(clinicDoctor == null)
			return Result.NotFound("Apoointemnt can't reserve");

		if (!clinicDoctor.FreeTime.IsFreeAt(request.DateTime))
			return Result.Invalid(new ValidationError(nameof(request.DateTime), "can't request appointment at this date and time"));


		var requestApoointemntResult = patient.RequestAppointemnt(request.ClinicId, request.DoctorId, request.DateTime);

		if (!requestApoointemntResult.IsSuccess)
			return Result.Invalid(requestApoointemntResult.ValidationErrors);

		_context.Entry(patient).State = EntityState.Modified;
		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success(requestApoointemntResult.Value);
	}
}