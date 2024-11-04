using LibraryWebApp.BookService.API.Middlewares;

namespace LibraryWebApp.BookService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddServiceDefaults();

            builder.Services.AddApplicationConfigurations(builder.Configuration);

            var app = builder.Build();

            app.MapDefaultEndpoints();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();
            app.MapControllers();

            app.Run();
        }
    }
}
