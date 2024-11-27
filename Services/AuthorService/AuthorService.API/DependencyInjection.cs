using LibraryWebApp.AuthorService.Domain.Interfaces;
using LibraryWebApp.AuthorService.Application.Profiles;
using FluentValidation;
using LibraryWebApp.AuthorService.Infrastructure;
using LibraryWebApp.AuthorService.Application.UseCases;
using LibraryWebApp.AuthorService.Application.Behaviors;
using MediatR;

namespace LibraryWebApp.AuthorService.API
{
    public static class DependencyInjection
    {
        public static void AddDependencyInjectionServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddAuthorHandler).Assembly));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(typeof(AuthorToAuthorProfile).Assembly);

            services.AddValidatorsFromAssembly(typeof(AddAuthorCommandValidator).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }
    }
}
