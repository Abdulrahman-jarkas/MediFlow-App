using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace MediFlow.IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
			[ 
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
			new("roles",
				"Your role(s)",
				["role"])];

    public static IEnumerable<ApiScope> ApiScopes =>
		[
			new("clinicmanagementapi.fullaccess")
		];

	public static IEnumerable<ApiResource> ApiResources =>
	 [
		 new("clinicmanagementapi",
				 "Clinic Management API", ["role"])
			 {
				 Scopes = { "clinicmanagementapi.fullaccess"},
				ApiSecrets = { new Secret("clinicmanagementapisecret".Sha256()) }
			 }
	 ];

	public static IEnumerable<Client> Clients =>
		[
				new Client
				{
					ClientId = "mediflowclient",
					ClientName = "MediFlow Client",
					ClientSecrets =
					{
						new Secret("secret".Sha256())
					},
					AllowedGrantTypes = GrantTypes.Code,
					AccessTokenType = AccessTokenType.Jwt,
					UpdateAccessTokenClaimsOnRefresh = true,
					AccessTokenLifetime = 120,
					AllowOfflineAccess = true,
					RefreshTokenUsage = TokenUsage.ReUse,
					RefreshTokenExpiration = TokenExpiration.Absolute,
					AbsoluteRefreshTokenLifetime = 180,
					//SlidingRefreshTokenLifetime = 10,
					UserSsoLifetime = 60,
					IdentityTokenLifetime = 300,
					//DPoPClockSkew = TimeSpan.FromSeconds(0),
					//DPoPValidationMode = DPoPTokenExpirationValidationMode.Iat,
					//RequireDPoP = false,
					AllowedCorsOrigins = {"https://localhost:7119", "https://localhost:4200" },
					AllowedScopes =
					{
					  IdentityServerConstants.StandardScopes.OpenId,
					  IdentityServerConstants.StandardScopes.Profile,
					  IdentityServerConstants.StandardScopes.Email,
					  IdentityServerConstants.StandardScopes.OfflineAccess,
					  "roles",
					  "clinicmanagementapi.fullaccess"
					},
					RedirectUris =
					{
						"https://localhost:7119/signin-oidc"
					},
					PostLogoutRedirectUris =
					{
						"https://localhost:7119/signout-callback-oidc"
					},
				}];
}
