using MediFlow.ClinicManagement.Domain.ValueObjects;
using MediFlow.Schedule.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace MediFlow.Schedule.Data;

internal class ClinicDoctorConfiguration : IEntityTypeConfiguration<ClinicDoctor>
{
	public void Configure(EntityTypeBuilder<ClinicDoctor> builder)
	{
		var dictionaryComparer = new ValueComparer<Availability>(
				(a, b) => JsonSerializer.Serialize(a.Data, (JsonSerializerOptions?)null) ==
						  JsonSerializer.Serialize(b.Data, (JsonSerializerOptions?)null),

				a => JsonSerializer.Serialize(a.Data, (JsonSerializerOptions?)null).GetHashCode(),

				a => Availability.CloneAvailabilty(a)
			);

		builder
		   .Property(p => p.FreeTime)
		   .HasColumnName("AvailabilityData")
		   .HasConversion(
			   v => JsonSerializer.Serialize(v.Data, (JsonSerializerOptions?)null),
			   v => Availability.InitAvailabilty(JsonSerializer.Deserialize<Dictionary<DayOfWeek, List<TimeRange>>>(
						v, (JsonSerializerOptions?)null)!)
		   )
		   .Metadata.SetValueComparer(dictionaryComparer);
	}
}