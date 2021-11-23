using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RdErp;

namespace RdErp.AspNetCore.Common
{
    public static class Configuration
    {
        public static IServiceCollection AddRdErpIdentityServiceAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = configuration.GetValue<string>("Identity:Authority");
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "rd-erp";
                    options.ApiSecret = configuration.GetValue<string>("Identity:Secret");
                });

            return services;
        }

        public static IServiceCollection AddRdErpAppUser(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddHttpContextAccessor();

            services.AddScoped<AppUser>(svc =>
            {
                var user =
                    (svc
                        .GetService<IHttpContextAccessor>()
                       ?.HttpContext
                       ?.User
                       ?.Identity as ClaimsIdentity)
                   ?.Claims
                    .Where(c => c.Type.Equals("sub", StringComparison.OrdinalIgnoreCase))
                    .Where(c => !String.IsNullOrWhiteSpace(c.Value))
                    .Select(c => new AppUser { Id = c.Value })
                    .FirstOrDefault();

                return user;
            });

            return services;

        }
    }
}