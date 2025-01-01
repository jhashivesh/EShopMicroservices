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
                        config["ClientApp"] + "/signin-callback",
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
                AllowedCorsOrigins = {config["ClientApp"]!},
                RequireClientSecret = false,
                AllowRememberConsent = false,
                RequireConsent = false,
                AccessTokenLifetime = 3600,
                PostLogoutRedirectUris = new List<string>
                {
                    config["ClientApp"] + "/signout-callback",
                },
                ClientSecrets = new List<Secret>
                {
                    new Secret("5c6eb3b4-61a7-4668-ac57-2b4591ec26d2".Sha256())
                },
                AlwaysIncludeUserClaimsInIdToken = true
             }
        };
}
