using LibraryWebApp.BookService.Infrastructure;
using LibraryWebApp.BookService.Application.Services;
using LibraryWebApp.BookService.Domain.Interfaces;
using LibraryWebApp.BookService.Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LibraryWebApp.BookService.Application.Entities;
using Quartz.Spi;
using Quartz;
using Quartz.Impl;
using AutoMapper;
using LibraryWebApp.BookService.Application.Profiles;
using LibraryWebApp.BookService.Application.Middlewares;
using FluentValidation;
using LibraryWebApp.BookService.Application.Validators;
using Microsoft.OpenApi.Models;

namespace LibraryWebApp.BookService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddServiceDefaults();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "¬ведите JWT токен следующим образом: Bearer {your token}"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var secret = builder.Configuration["Jwt:Secret"];
            if (string.IsNullOrEmpty(secret))
            {
                throw new InvalidOperationException("JWT secret is not configured.");
            }

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"]
                    };
                });
            builder.Services.AddAuthorization();

            builder.Services.AddMemoryCache();

            builder.Services.AddValidatorsFromAssemblyContaining<BookDTOValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<ImageDTOValidator>();

            builder.Services.AddAutoMapper(typeof(MappingProfile));


            builder.Services.AddDbContext<ApplicationContext>(opts =>
                opts.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));

            builder.Services.AddScoped<BookReminderJob>();
            builder.Services.AddScoped<IMapper, Mapper>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IBookService, Application.Services.BookService>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.AddSingleton<IJobFactory, JobFactory>();
            builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            builder.Services.AddSingleton<QuartzHostedService>();
            builder.Services.AddSingleton(new JobSchedule(
                jobType: typeof(BookReminderJob),
                cronExpression: "0 0 0 * * ? *"));

            builder.Services.AddHostedService<QuartzHostedService>();

            var app = builder.Build();

            app.MapDefaultEndpoints();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
