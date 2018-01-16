using Fulcrum.Infrastructure.Settings;
using Fulcrum.UI.Web.Core.Helpers;
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
            services.Configure<AzureSettings>(options =>
            {
                options.AzureStorageConnectionString = Configuration["ConnectionStrings:BlobConnection"];
            });

            services.Configure<AuthMessageSenderOptions>(options =>
            {
                options.SendGridUser = Configuration["Credentials:SendGrid:User"];
                options.SendGridPassword = Configuration["Credentials:SendGrid:Password"];

                bool IsDebug = false;
                bool.TryParse(Configuration["Email:IsDebug"], out IsDebug);

                options.IsDebug = IsDebug;
                options.DebugEmail = Configuration["Email:DebugEmail"];
            });

            services.Configure<XEROOptions>(options =>
            {
                options.BaseUrl = Configuration["XERO:BaseUrl"];
                options.ConsumerKey = Configuration["XERO:ConsumerKey"];
                options.ConsumerSecret = Configuration["XERO:ConsumerSecret"];
                options.CallbackUrl = Configuration["XERO:CallbackUrl"];
                options.PrivateKeyFile = Configuration["XERO:PrivateKeyFile"];
                options.SigningCertificate = Configuration["XERO:SigningCertificate"];
                options.SigningCertificatePassword = Configuration["XERO:SigningCertificatePassword"];
            });

            services.Configure<PowerBIOptions>(options =>
            {
                options.AccessKey = Configuration["PowerBI:AccessKey"];
                options.ApiUrl = Configuration["PowerBI:ApiUrl"];
                options.WorkspaceCollection = Configuration["PowerBI:WorkspaceCollection"];
                options.WorkspaceId = Configuration["PowerBI:WorkspaceId"];
            });

            var SecretKey = Configuration["Credentials:Token:Key"];
            SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];

                var days = 4;
                int.TryParse(jwtAppSettingOptions[nameof(JwtIssuerOptions.ValidFor)], out days);
                options.ValidFor = TimeSpan.FromDays(days);

                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromDays(2);
            });

            services.Configure<IntercomHubSettings>(options =>
            {
                options.Endpoint = Configuration["NotificationHub:Intercom:Endpoint"];
            });

            return services;
        }
    }
}