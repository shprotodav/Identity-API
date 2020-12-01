using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TestTask.Identity.BL.Services;
using TestTask.Identity.Common;

namespace TestTask.Identity.Services
{
    public static class DependencyResolver
	{
		public static void Resolve(IServiceCollection services, IConfiguration configuration)
		{
			var appSettingSection = configuration.GetSection("AppSettings");

			services.Configure<AppSettings>(appSettingSection);

			services.AddHttpContextAccessor();

			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IJwtTokenService, JwtTokenService>();
		}
	}
}
