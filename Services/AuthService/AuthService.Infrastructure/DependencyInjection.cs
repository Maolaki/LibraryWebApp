using Microsoft.Extensions.DependencyInjection;
using LibraryWebApp.AuthService.Application.UseCases;
using LibraryWebApp.AuthService.Domain.Interfaces;
using LibraryWebApp.AuthService.Infrastructure.Services;
using LibraryWebApp.AuthService.Application.Profiles;
using LibraryWebApp.AuthService.Application.Entities;
using FluentValidation;
using LibraryWebApp.AuthService.Application.Validators;
using LibraryWebApp.AuthService.Application.Filters;

namespace LibraryWebApp.AuthService.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddDependencyInjectionServices(this IServiceCollection services)
        {
            services.AddScoped<RegisterUserUseCase>();
            services.AddScoped<AuthenticateUserUseCase>();
            services.AddScoped<RefreshTokensUseCase>();
            services.AddScoped<RevokeTokenUseCase>();
            services.AddScoped<GetUserIdUseCase>();
            services.AddScoped<RevokeAllTokensUseCase>();

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
