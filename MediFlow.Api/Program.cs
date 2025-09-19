using MediFlow.Api;
using MediFlow.ClinicManagement;
using MediFlow.DoctorManagement;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using SharedKernel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddUsersManagementServices(builder.Configuration);
builder.Services.AddScheduleServices(builder.Configuration);

builder.Services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		//options.Authority = builder.Configuration["OpenIDConnectSettings:Authority"];
		//options.Audience = builder.Configuration["OpenIDConnectSettings:Audience"];
		options.TokenValidationParameters.RoleClaimType = "role";
		options.TokenValidationParameters.NameClaimType = "given_name";

		options.Events = new JwtBearerEvents
		{
			OnTokenValidated = (context) =>
			{
				return Task.CompletedTask;
			},
			OnAuthenticationFailed = (context) =>
			{
				return Task.CompletedTask;
			},
			OnMessageReceived = (context) =>
			{
				return Task.CompletedTask;
			},
		};
	});

var app = builder.Build();

await app.SeedScheduleDataAsync();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ModelBindingExceptionMiddleware>();

app.MapSliceEndpoints();

app.Run();
