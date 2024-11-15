using LibraryWebApp.AuthorService.Domain.Interfaces;
using LibraryWebApp.AuthorService.Application.Profiles;
using FluentValidation;
using LibraryWebApp.AuthorService.Application.Validators;
using LibraryWebApp.AuthorService.API.Filters;
using LibraryWebApp.AuthorService.Infrastructure;
using LibraryWebApp.AuthorService.Application.UseCases;

namespace LibraryWebApp.AuthorService.API
{
    public static class DependencyInjection
    {
        public static void AddDependencyInjectionServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddAuthorHandler).Assembly));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(typeof(AuthorToAuthorProfile).Assembly);

            services.AddValidatorsFromAssemblyContaining<AuthorDTOValidator>();

            services.AddScoped<ValidateModelAttribute>();
        }
    }
}
