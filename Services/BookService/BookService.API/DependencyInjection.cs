using FluentValidation;
using LibraryWebApp.BookService.Application.Entities;
using LibraryWebApp.BookService.Application.Services;
using LibraryWebApp.BookService.Domain.Interfaces;
using Quartz.Impl;
using Quartz.Spi;
using Quartz;
using LibraryWebApp.BookService.Infrastructure;
using BookService.Application.Profiles;
using LibraryWebApp.BookService.API.Filters;
using LibraryWebApp.BookService.Application.UseCases;
using LibraryWebApp.BookService.Application.DTOs;
using MediatR;
using LibraryWebApp.BookService.Application.Behaviors;

namespace LibraryWebApp.BookService.API
{
    public static class DependencyInjection
    {
        public static void AddDependencyInjectionServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddBookHandler).Assembly));

            services.AddScoped<BookReminderJob>();

            services.AddScoped<IUnitOfWork<ImageDTO>, UnitOfWork>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<QuartzHostedService>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(BookReminderJob),
                cronExpression: "0 0 0 * * ? *"));

            services.AddHostedService<QuartzHostedService>();

            services.AddAutoMapper(typeof(BookToBookProfile).Assembly);

            services.AddValidatorsFromAssembly(typeof(AddBookCommandValidator).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddScoped<EnsureAuthenticatedUserFilter>();
        }
    }
}
