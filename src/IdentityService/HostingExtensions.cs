using IdentityService.Data;
using IdentityService.Models;
using IdentityService.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityService;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
                        .WriteTo.Console()
                        .CreateBootstrapLogger();

        Log.Information("Starting up");


        builder.Services.AddRazorPages();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();


        builder.Services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                Log.Information("IssuerUri:" + builder.Configuration["IssuerUri"]);
                options.IssuerUri = builder.Configuration["IssuerUri"];

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                //options.EmitStaticAudienceClaim = true;
            })
            .AddInMemoryClients(Config.Clients(builder.Configuration))
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiResources(Config.ApiResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddAspNetIdentity<ApplicationUser>()
            .AddProfileService<CustomProfileService>()
            .AddDeveloperSigningCredential();
        ;

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.SameSite = SameSiteMode.Lax;
        });

        builder.Services.ConfigureExternalCookie(options =>
        {
            options.Cookie.SameSite = SameSiteMode.Lax;
        });

        builder.Services.AddAuthentication();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        var forwardedHeaderOptions = new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        };
        forwardedHeaderOptions.KnownNetworks.Clear();
        forwardedHeaderOptions.KnownProxies.Clear();
        app.UseForwardedHeaders(forwardedHeaderOptions);

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();

        app.MapRazorPages()
            .RequireAuthorization();

        return app;
    }
}