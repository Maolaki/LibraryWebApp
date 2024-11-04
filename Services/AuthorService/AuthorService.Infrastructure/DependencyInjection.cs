using Microsoft.Extensions.DependencyInjection;
using LibraryWebApp.AuthorService.Domain.Interfaces;
using LibraryWebApp.AuthorService.Application.Profiles;
using LibraryWebApp.AuthorService.Application.UseCases;
using FluentValidation;
using LibraryWebApp.AuthorService.Application.Validators;
using AuthorService.API.Filters;

namespace LibraryWebApp.AuthorService.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddDependencyInjectionServices(this IServiceCollection services)
        {
            services.AddScoped<GetAuthorIdUseCase>();
            services.AddScoped<GetAllAuthorsUseCase>();
            services.AddScoped<AddAuthorUseCase>();
            services.AddScoped<GetAuthorUseCase>();
            services.AddScoped<UpdateAuthorUseCase>();
            services.AddScoped<DeleteAuthorUseCase>();
            services.AddScoped<GetAllBooksByAuthorUseCase>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddValidatorsFromAssemblyContaining<AuthorDTOValidator>();

            services.AddScoped<ValidateModelAttribute>();
        }
    }
}
