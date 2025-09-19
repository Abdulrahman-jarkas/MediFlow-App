using Duende.IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace BffOpenIddict.Server.Controllers;

[Route("api/[controller]")]
public class AccountController : ControllerBase
{
	private readonly IHttpClientFactory httpClientFactory;
	private readonly IConfiguration configuration;

	public AccountController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
	{
		this.httpClientFactory = httpClientFactory;
		this.configuration = configuration;
	}

	[HttpGet("Login")]
    public ActionResult Login(string? returnUrl, string? claimsChallenge)
    {
        var properties = GetAuthProperties(returnUrl);

        if (claimsChallenge != null)
        {
            string jsonString = claimsChallenge.Replace("\\", "").Trim('"');

            properties.Items["claims"] = jsonString;
        }

        return Challenge(properties, OpenIdConnectDefaults.AuthenticationScheme);
    }

    // [ValidateAntiForgeryToken] // not needed explicitly due the the Auto global definition.
    [IgnoreAntiforgeryToken] // need to apply this to the form post request
    [Authorize]
    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        var client = httpClientFactory.CreateClient("IDP");

        var discoveryDocumentResponse = await client.GetDiscoveryDocumentAsync();

        if (discoveryDocumentResponse.IsError)
        {
            throw new Exception(discoveryDocumentResponse.Error);
        }

        var revokeAccessTokenResponse = await client.RevokeTokenAsync(
            new TokenRevocationRequest
            {
                Address = discoveryDocumentResponse.RevocationEndpoint,
                ClientId = configuration["OpenIDConnectSettings:ClientId"]!,
                ClientSecret = configuration["OpenIDConnectSettings:ClientSecret"]!,
                Token = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken)!
            }
        );

        if (revokeAccessTokenResponse.IsError)
        {
            throw new Exception(revokeAccessTokenResponse.Error);
        }

		var revokeRefreshTokenResponse = await client.RevokeTokenAsync(
		         new TokenRevocationRequest
		         {
			         Address = discoveryDocumentResponse.RevocationEndpoint,
					 ClientId = configuration["OpenIDConnectSettings:ClientId"]!,
					 ClientSecret = configuration["OpenIDConnectSettings:ClientSecret"]!,
					 Token = (await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken))!
		         }
        );

		if (revokeAccessTokenResponse.IsError)
		{
			throw new Exception(revokeAccessTokenResponse.Error);
		}


        return SignOut(
            new AuthenticationProperties { RedirectUri = "/" },
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme);
    }

    /// <summary>
    /// Original src:
    /// https://github.com/dotnet/blazor-samples/blob/main/8.0/BlazorWebOidc/BlazorWebOidc/LoginLogoutEndpointRouteBuilderExtensions.cs
    /// </summary>
    private static AuthenticationProperties GetAuthProperties(string? returnUrl)
    {
        const string pathBase = "/";

        // Prevent open redirects.
        if (string.IsNullOrEmpty(returnUrl))
        {
            returnUrl = pathBase;
        }
        else if (!Uri.IsWellFormedUriString(returnUrl, UriKind.Relative))
        {
            returnUrl = new Uri(returnUrl, UriKind.Absolute).PathAndQuery;
        }
        else if (returnUrl[0] != '/')
        {
            returnUrl = $"{pathBase}{returnUrl}";
        }

        return new AuthenticationProperties { RedirectUri = returnUrl };
    }
}
