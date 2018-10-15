using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Taqweem.Data;
using Taqweem.Models;
using Taqweem.Services;
using Taqweem.Classes;
using AutoMapper;
using Taqweem.ViewModels.ManageViewModels;
using Hangfire;
using System;

namespace Taqweem
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }
        public IHostingEnvironment env { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddScoped<TaqweemService>();

            services.AddScoped<WorldService>();

            services.AddScoped<RazorViewToStringRenderer>();

            services.AddScoped<RazorHelperService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MasjidEditViewModel, Masjid>().ForMember(x => x.UID, opt => opt.Ignore());

                cfg.CreateMap<SalaahTimeViewModel, SalaahTime>().ForMember(x => x.UID, opt => opt.Ignore());
            });

            var mapper = config.CreateMapper();

            services.AddSingleton(mapper);

            services.AddMvc();

            services.AddHangfire(c =>
            {
                c.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration configuration, IEmailSender emailSender)
        {
            app.UseHangfire();
            app.UseHangfireDashboard();
            app.ScheduleHangfireTasks(emailSender);

            //if (env.IsProduction() || env.IsStaging())
            //{

            //}

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
