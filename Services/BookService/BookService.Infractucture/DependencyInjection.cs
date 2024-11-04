using FluentValidation;
using LibraryWebApp.BookService.Application.Entities;
using LibraryWebApp.BookService.Application.Profiles;
using LibraryWebApp.BookService.Application.Services;
using LibraryWebApp.BookService.Application.UseCases;
using LibraryWebApp.BookService.Application.Validators;
using LibraryWebApp.BookService.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Impl;
using Quartz.Spi;
using Quartz;
using LibraryWebApp.BookService.Application.Interfaces;
using LibraryWebApp.BookService.Application.Filters;

namespace LibraryWebApp.BookService.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddDependencyInjectionServices(this IServiceCollection services)
        {
            services.AddScoped<GetAllBooksUseCase>();
            services.AddScoped<GetAllBooksWithFiltersUseCase>();
            services.AddScoped<GetBookByIdUseCase>();
            services.AddScoped<GetBooksByISBNUseCase>();
            services.AddScoped<GetAvailableCopiesUseCase>();
            services.AddScoped<AddBookUseCase>();
            services.AddScoped<UpdateBookUseCase>();
            services.AddScoped<DeleteBookUseCase>();
            services.AddScoped<ReturnBookUseCase>();
            services.AddScoped<CheckoutBookUseCase>();
            services.AddScoped<AddBookImageUseCase>();
            services.AddScoped<GetBookImageUseCase>();
            services.AddScoped<NotifyCheckoutDateUseCase>();

            services.AddScoped<BookReminderJob>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<QuartzHostedService>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(BookReminderJob),
                cronExpression: "0 0 0 * * ? *"));

            services.AddHostedService<QuartzHostedService>();

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddValidatorsFromAssemblyContaining<BookDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<ImageDTOValidator>();

            services.AddScoped<ValidateModelAttribute>();
            services.AddScoped<EnsureAuthenticatedUserFilter>();
        }
    }
}
