using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Yarp.ReverseProxy.Transforms;
using Microsoft.IdentityModel.JsonWebTokens;
using BffOpenIddict.Server.Services;
using System.Net.Http.Headers;


var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddRazorPages();

services.AddHttpClient("IDP", client => {
	client.BaseAddress = new Uri(configuration["OpenIDConnectSettings:Authority"]!);
});

builder.Services
	.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never;
	});

JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();


services.AddAuthentication(options =>
{
	options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
	{
		options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
		options.Authority = configuration["OpenIDConnectSettings:Authority"];
		options.ClientId = configuration["OpenIDConnectSettings:ClientId"];
		options.ClientSecret = configuration["OpenIDConnectSettings:ClientSecret"];
		options.ResponseType = "code";
		
		options.Scope.Add("email");
		options.Scope.Add("offline_access");
		options.Scope.Add("roles");
		options.Scope.Add("clinicmanagementapi.fullaccess");

		options.ClaimActions.MapJsonKey("role", "role");
		options.ClaimActions.MapUniqueJsonKey("email", "email");


		options.SaveTokens = true;
		options.GetClaimsFromUserInfoEndpoint = true;
		options.TokenValidationParameters = new()
		{
			NameClaimType = "given_name",
			RoleClaimType = "role",
			ClockSkew = TimeSpan.Zero
		};
	});

builder.Services.AddOpenIdConnectAccessTokenManagement();


services.AddReverseProxy()
   .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
   .AddTransforms(builderContext =>
   {
	   builderContext.AddRequestTransform(async transformContext =>
	   {

	   var token = await transformContext.HttpContext.GetUserAccessTokenAsync();

	   Console.WriteLine("Management: ");
	   Console.WriteLine($"Access token: {token.AccessToken}");
	   Console.WriteLine($"Refresh token: {token.RefreshToken}");
	   Console.WriteLine($"Expired At: {token.Expiration.ToLocalTime() }");

	   transformContext.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
	   });
   });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseNoUnauthorizedRedirect("/api");
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapNotFound("/api/{**segment}");

app.MapReverseProxy();

app.MapFallbackToPage("/_Host");

app.Run();