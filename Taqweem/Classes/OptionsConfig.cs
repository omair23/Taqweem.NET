using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Taqweem.Classes
{
    public static class OptionConfigExtentions
    {
        public static IServiceCollection AddOptions(this IServiceCollection services, IConfigurationRoot Configuration)
        {
            services.Configure<AuthMessageSenderOptions>(options =>
            {
                options.SendGridUser = Configuration["Credentials:SendGrid:User"];
                options.SendGridPassword = Configuration["Credentials:SendGrid:Password"];

                bool IsDebug = false;
                bool.TryParse(Configuration["Email:IsDebug"], out IsDebug);

                options.IsDebug = IsDebug;
                options.DebugEmail = Configuration["Email:DebugEmail"];
            });
            
            var SecretKey = Configuration["Credentials:Token:Key"];
            SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromDays(2);
            });

            return services;
        }
    }
}