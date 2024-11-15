using LibraryWebApp.AuthService.Domain.Interfaces;
using LibraryWebApp.AuthService.Infrastructure.Services;
using LibraryWebApp.AuthService.Application.Profiles;
using FluentValidation;
using LibraryWebApp.AuthService.Application.Validators;
using LibraryWebApp.AuthService.API.Filters;
using LibraryWebApp.AuthService.Infrastructure.Entities;
using LibraryWebApp.AuthService.Application.UseCases;

namespace LibraryWebApp.AuthService.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddDependencyInjectionServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AuthenticateUserHandler).Assembly));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddValidatorsFromAssemblyContaining<UserDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<LoginDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<AuthenticatedDTOValidator>();

            services.AddScoped<ValidateModelAttribute>();
            services.AddScoped<EnsureAuthenticatedUserFilter>();
        }
    }
}
