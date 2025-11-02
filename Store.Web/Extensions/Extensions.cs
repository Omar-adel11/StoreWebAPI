using System.Text;
using Domain.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistance;
using Persistance.Identity;
using Services;
using Shared;
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
            AddIdentityServices(Services);

            AddSwaggerServices(Services);

            Services.AddInfrastructureServices(Configuration);

            Services.AddApplicationService(Configuration);
            Services.ConfigureJWTServices(Configuration);




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
            app.UseAuthentication();
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
        private static IServiceCollection AddIdentityServices(this IServiceCollection Services)
        {

            Services.AddIdentity<Appuser,IdentityRole>().AddEntityFrameworkStores<StoreIdentityDbContext>();  
            
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

        private static IServiceCollection ConfigureJWTServices(this IServiceCollection Services,IConfiguration Configuration)
        {

            var jwtoptions = Configuration.GetSection("JWTOptions").Get<JWTOptions>();

            Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtoptions.Issuer,
                    ValidAudience = jwtoptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtoptions.Secretkey))


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
            await DbInitializer.InitializeIdentityAsync();
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
