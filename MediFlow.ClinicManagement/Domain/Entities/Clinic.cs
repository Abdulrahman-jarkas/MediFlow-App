using Ardalis.Result;
using MediFlow.ClinicManagement.Domain.Entities.Events;
using SharedKernal;

namespace MediFlow.ClinicManagement.Domain.Entities;

internal class Clinic : Entity
{
	public string Name { get; private set; } = string.Empty;
	public Guid UserId { get; private init; }

	private readonly List<Room> _rooms = new List<Room>();
	public IReadOnlyList<Room> Rooms => _rooms.AsReadOnly();

	private readonly List<Equipment> _equipments = new List<Equipment>();
	public IReadOnlyList<Equipment> Equipments => _equipments.AsReadOnly();

	private readonly List<Medicine> _medicines = new List<Medicine>();
	public IReadOnlyList<Medicine> Medicines => _medicines.AsReadOnly();

	private readonly List<Doctor> _doctors = new();
	public IReadOnlyList<Doctor> Doctors => _doctors.AsReadOnly();


	private Clinic(Guid userId, string name) : this()
	{
		UserId = userId;

		UpdateName(name);
	}

	public static Result<Clinic> Create(
		Guid userId,
		string name)
	{
		if(userId == Guid.Empty)
			return Result
				.Unauthorized("Access denied. You are not authorized.");

		return Result.Created(new Clinic(userId, name));
	}

	public Result<Clinic> UpdateName(string name)
	{
		if (string.IsNullOrEmpty(name.Trim()))
			return Result
					.Invalid(new ValidationError(nameof(Name), "The name of clinic must not be empty"));

		Name = name;

		return Result.Success(this);
	}

	public void AddDoctor(Doctor doctor)
	{
		_doctors.Add(doctor);

		var addDoctorEvent = new DoctorAddedToClinicEvent(this, doctor);
		RegisterDomainEvent(addDoctorEvent);
	}

	public Doctor? GetDoctor(Guid id)
	{
		return _doctors.FirstOrDefault(d => d.Id == id);
	}

	public void AddMedicine(Medicine medicine)
	{
		_medicines.Add(medicine);
	}

	public void AddEquipment(Equipment equipment)
	{
		_equipments.Add(equipment);
	}

	public void AddRoom(Room room)
	{
		_rooms.Add(room);
	}

	public Medicine? GetMedicine(Guid id)
	{
		return _medicines.FirstOrDefault(m => m.Id == id);
	}

	public Equipment? GetEquipment(Guid id)
	{
		return _equipments.FirstOrDefault(eq => eq.Id == id);
	}

	public Room? GetRoom(Guid id)
	{
		return _rooms.FirstOrDefault(room => room.Id == id);
	}

	public Clinic() { }
}