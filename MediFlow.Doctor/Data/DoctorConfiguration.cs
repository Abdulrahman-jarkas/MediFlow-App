using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MediFlow.ClinicManagement.Domain.ValueObjects;
using DoctorManagement.Domain.Entities;

namespace MediFlow.DoctorManagement.Data;

internal class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
	public void Configure(EntityTypeBuilder<Doctor> builder)
	{
	}
}

