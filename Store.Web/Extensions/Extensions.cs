using Domain.Contracts;
using Microsoft.AspNetCore.Mvc;
using Persistance;
using Services;
using Shared.Errors;
using Store.Web.Middlewares;

namespace Store.Web.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddAllServices(this IServiceCollection Services,IConfiguration Configuration)
        {

            // Add services to the container.
            AddBuiltinServices(Services);


            AddSwaggerServices(Services);

            Services.AddInfrastructureServices(Configuration);

            Services.AddApplicationService(Configuration);

            ConfigureServices(Services);




            return Services;
        }

        public static async Task<WebApplication> ConfigureMiddlewares(this WebApplication app)
        {

            await app.InitializeDBAsync();

            app.UseErrorHandlingMiddleware();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers();

            return app;
        }

        private static IServiceCollection AddBuiltinServices(this IServiceCollection Services)
        {

            // Add services to the container.

            Services.AddControllers();
            
            return Services;
        }

        private static IServiceCollection AddSwaggerServices(this IServiceCollection Services)
        {

            // Add services to the container.

            Services.AddEndpointsApiExplorer();
            Services.AddSwaggerGen();


            return Services;
        }

        private static IServiceCollection ConfigureServices(this IServiceCollection Services)
        {

            Services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(m => m.Value.Errors.Any())
                                                            .Select(m => new ValidationError()
                                                            {
                                                                Field = m.Key,
                                                                Errors = m.Value.Errors.Select(errors => errors.ErrorMessage)
                                                            });

                    var response = new ValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            return Services;
        }
        private static async Task<WebApplication> InitializeDBAsync(this WebApplication app)
        {
            #region Initialize DB
            using var scope = app.Services.CreateScope();
            var DbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await DbInitializer.InitializeAsync();
            #endregion
            return app;
        }

        private static  WebApplication UseErrorHandlingMiddleware(this WebApplication app)
        {
            app.UseMiddleware<GlobalErrorHandlingMiddlewares>();
            return app;
        }

    }
}
