using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RdErp.Identity.Service
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Config = configuration
                ??
                throw new ArgumentNullException(nameof(configuration));
        }

        private IConfiguration Config { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var secrets = new List<Secret>
                {
                    new Secret(Config.GetValue<string>("Identity:Secret").Sha256())
                };

            services.AddMvc();

            services
                .AddIdentityServer(config =>
                {
                    config.IssuerUri = Config.GetValue<string>("Identity:IssuerUri");
                    config.PublicOrigin = Config.GetValue<string>("Identity:PublicOrigin");
                })
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                .AddInMemoryClients(new []
                {
                    new Client
                    {
                        AccessTokenType = AccessTokenType.Jwt,
                            AllowAccessTokensViaBrowser = true,
                            AllowedScopes = {
                                "rd-erp",
                                IdentityServerConstants.StandardScopes.OpenId
                            },
                            AllowedGrantTypes = {
                                GrantType.ResourceOwnerPassword,
                                GrantType.Implicit
                            },
                            ClientId = "rd",
                            ClientName = "rd",
                            AllowOfflineAccess = true,
                            ClientSecrets = secrets,
                            RedirectUris = (Config.GetValue<string>("Identity:RedirectUris") ?? "").Split(","),
                            RequireConsent = false
                    }
                })
                .AddInMemoryApiResources(
                    new []
                    {
                        new ApiResource("rd-erp", "RD ERP Api")
                    })
                .AddTestUsers(new List<TestUser>()
                {
                    new TestUser
                    {
                        SubjectId = "serg@rd-erp.io",
                            Username = "serg",
                            Password = "root",
                            IsActive = true,
                    }
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(cors =>
            {
                cors
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowCredentials();
            });
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }
    }
}