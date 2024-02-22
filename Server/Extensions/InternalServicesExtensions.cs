using Infrastructure.AppSettings;
using Infrastructure.FileUpload;
using Infrastructure.User;
using Infrastructure.User.Enums;
using Logs;
using Logs.Services;
using MessageBroker.Consumer;
using MessageBroker.Consumer.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Middlewares;
using ProxyKit;
using Rdpzsd.Import.Jobs;
using Rdpzsd.Models;
using Rdpzsd.Services.Rdpzsd.Parts.Base;
using Server.Handlers;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Server.Extensions
{
    public static class InternalServicesExtensions
    {
        public static void ConfigureDbContextService(this IServiceCollection services)
        {
            services
                .AddDbContext<RdpzsdDbContext>(o =>
                {
                    o.UseNpgsql(AppSettingsProvider.MainDbConnectionString,
                        e => e.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
                })
                .AddDbContext<LogContext>(o =>
                {
                    o.UseNpgsql(AppSettingsProvider.LogDbConnectionString,
                        e => e.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
                });
        }

        public static void ConfigureServices(this IServiceCollection services, IWebHostEnvironment environment)
        {
            services.AddHttpClient();
            services.ConfigureRsoHttpClientServices(environment);

            var assemblyRepositoryNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies()
                .Where(e => e.Name == "Logs"
                    || e.Name == "Infrastructure"
                    || e.Name == "Rdpzsd.Services"
                    || e.Name == "Rdpzsd.Import"
                    || e.Name == "Rdpzsd.Reports"
                    || e.Name == "MessageBroker");

            foreach (var assemblyRepositoryName in assemblyRepositoryNames)
            {
                var repositoryAssemblyTypes = Assembly.Load(assemblyRepositoryName).GetTypes();

                var allServiceClasses = repositoryAssemblyTypes
                    .Where(e => e.Name.EndsWith("Service"))
                    .ToList();

                foreach (var item in allServiceClasses)
                {
                    services.AddScoped(item);
                }
            }

            services.AddHttpContextAccessor();

            services.AddScoped(typeof(UserContext), (provider) =>
            {
                var context = provider.GetRequiredService<IHttpContextAccessor>().HttpContext;

                var key = AuthService.UserContextKey;

                if (context != null && context.Items.ContainsKey(key))
                {
                    var userContext = (UserContext)context.Items[key];

                    return userContext;
                }

                return new UserContext { UserType = UserType.Unauthorized };
            });

            services.AddScoped(typeof(FileUploadService<>));
            services.AddScoped(typeof(BaseHistoryPartService<,,,,>));
            services.AddScoped(typeof(ActionWorkflowService<>));

            if (AppSettingsProvider.MessageBroker.Enable)
            {
                services.AddSingleton<RdpzsdMbConsumer>();
            }
        }

        public static void ConfigureRsoHttpClientServices(this IServiceCollection services, IWebHostEnvironment environment)
        {
            services.AddScoped(typeof(RsoHttpClientCertificateHandler), (provider) =>
            {
                return new RsoHttpClientCertificateHandler(environment);
            });

            services.AddHttpClient(AppSettingsProvider.RsoIntegration.RsoHttpClient, c =>
            {
            }).ConfigurePrimaryHttpMessageHandler(sp => sp.GetRequiredService<RsoHttpClientCertificateHandler>());
        }

        public static void StartJobs(this IServiceCollection services)
        {
            if (AppSettingsProvider.MessageBroker.Enable)
            {
                services.AddHostedService<RndSpecialityUpdateJob>();
                services.AddHostedService<RndNsiUpdateJob>();
                services.AddHostedService<RndOrganizationUpdateJob>();
            }

            if (AppSettingsProvider.ValidationTxtJob.EnableJob)
            {
                services.AddHostedService<ValidationTxtJob>();
            }

            if (AppSettingsProvider.RegistrationTxtJob.EnableJob)
            {
                services.AddHostedService<RegistrationTxtJob>();
            }
        }

        public static void ConfigureMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<RedirectionMiddleware>();
            app.UseMiddleware<AuthorizationMiddleware>();
            app.UseMiddleware<ErrorHandlingMiddleware>();
        }

        public static void ConfigureProxy(this IApplicationBuilder app)
        {
            var options = new RewriteOptions()
                    .AddRewrite(@"api/ems/(.*)", "api/$1", skipRemainingRules: true);

            app.UseWhen(
                context => context.Request.Path.Value.StartsWith("/api/ems"),
                builder => builder
                .UseRewriter(options)
                .RunProxy(context => context
                    .ForwardTo(AppSettingsProvider.ProxyPaths.Ems)
                    .Send()
            ));
        }
    }
}
