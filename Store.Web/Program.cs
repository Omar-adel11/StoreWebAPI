
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistance;
using Persistance.Data.Contexts;
using Persistance.Repositories;
using Services.Mapping.Products;
using AutoMapper;
using ServicesAbstractions;
using Services;
using Services.Products;
using ServicesAbstractions.Products;
using Store.Web.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Shared.Errors;
using Store.Web.Extensions; // Add this using directive at the top of the file

namespace Store.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAllServices(builder.Configuration);

            var app = builder.Build();

            await app.ConfigureMiddlewares();


            app.Run();
        }
    }
}
