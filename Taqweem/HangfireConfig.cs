using Hangfire;
using Microsoft.AspNetCore.Builder;
using Taqweem.Services;

namespace Taqweem
{
    public static class HangfireConfigExtentions
    {
        public static IApplicationBuilder UseHangfire(this IApplicationBuilder app)
        {
            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                //The Queue name must consist of lowercase letters, digits and underscore characters only.
                Queues = new[] { "default" }
            });

            return app;
        }

        public static IApplicationBuilder ScheduleHangfireTasks(this IApplicationBuilder app, IEmailSender _emailService)
        {
            //BackgroundJob.Enqueue(
            //        () => _emailService.SendReport());

            RecurringJob.AddOrUpdate(
                () => _emailService.SendReport(),
                    Cron.Daily(16));

            return app;
        }
    }
}