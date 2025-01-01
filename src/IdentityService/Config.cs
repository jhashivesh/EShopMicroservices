using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace IdentityService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("ecommerceApp", "ECommerce App Full Access"),
        };

    public static IEnumerable<ApiResource> ApiResources =>
        [
            new ApiResource("ECommerceGW", "Gateway.API")
            {
                Scopes = { "ecommerceApp" }
            }
        ];

    public static IEnumerable<Client> Clients(IConfiguration config) =>
        new Client[]
        {
            new Client
            {
                ClientId = "postman",
                ClientName = "Postman Client",
                AllowedScopes = { "openid", "profile", "ecommerceApp" },
                RedirectUris = { "https://www.getpostman.com/oauth2/callback" },
                ClientSecrets = new [] { new Secret("secret".Sha256()) },
                AllowedGrantTypes = { GrantType.ResourceOwnerPassword },
            },

            new Client
            {
                ClientName = "Angular-Client",
                ClientId = "angular-client",
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = new List<string>
                    {
                        "http://localhost:4200/signin-callback",
                        "http://localhost:4200/assets/silent-callback.html",
                    },
                RequirePkce = true,
                AllowAccessTokensViaBrowser = true,
                Enabled = true,
                UpdateAccessTokenClaimsOnRefresh = true,

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "ecommerceApp"
                },
                AllowedCorsOrigins = {"http://localhost:4200", "https://localhost:4200"},
                RequireClientSecret = false,
                AllowRememberConsent = false,
                RequireConsent = false,
                AccessTokenLifetime = 3600,
                PostLogoutRedirectUris = new List<string>
                {
                    "http://localhost:4200/signout-callback",
                },
                ClientSecrets = new List<Secret>
                {
                    new Secret("5c6eb3b4-61a7-4668-ac57-2b4591ec26d2".Sha256())
                },
                AlwaysIncludeUserClaimsInIdToken = true
             }
        };
}
