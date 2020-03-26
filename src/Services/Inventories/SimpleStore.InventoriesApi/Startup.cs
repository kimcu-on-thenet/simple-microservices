using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using HotChocolate.Execution.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SimpleStore.Infrastructure.Common.Extensions;
using SimpleStore.Inventories.Infrastructure.EfCore;
using SimpleStore.Inventories.Infrastructure.EfCore.Options;
using SimpleStore.InventoriesApi.GraphQL.Objects;
using System.Threading.Tasks;

namespace SimpleStore.InventoriesApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services
                .AddSingleton(this.Configuration)
                .AddCustomInfrastructure(this.Configuration);

            

            services
                .AddGraphQL(sp => Schema.Create(cfg =>
                {
                    cfg.RegisterServiceProvider(sp);
                    cfg.RegisterQueryType<QueryInventories>();
                    cfg.RegisterMutationType<InventoryMutation>();
                }), new QueryExecutionOptions
                {
                    IncludeExceptionDetails = true,
                    TracingPreference = TracingPreference.Always
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptionsMonitor<ServiceOptions> optionsAccessor)
        {
            app.Listen(optionsAccessor.CurrentValue.InventoriesApi);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //In order to run our server we now just have to add the middleware.
            app.UseGraphQL("/graphql");

            //In order to write queries and execute them it would be practical if our server also serves up Playground
            app.UsePlayground(new PlaygroundOptions
            {
                QueryPath = "/graphql",
                Path = "/playground",
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", context =>
                {
                    context.Response.Redirect("/playground");
                    return Task.CompletedTask;
                });
                endpoints.MapControllers();
            });
        }
    }
}
