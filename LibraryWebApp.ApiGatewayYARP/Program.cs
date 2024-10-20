namespace LibraryWebApp.ApiGatewayYARP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddServiceDefaults();

            builder.Services.AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            var app = builder.Build();

            app.MapDefaultEndpoints();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway");

                    c.SwaggerEndpoint("/auth-service/swagger/v1/swagger.json", "Auth Service");
                    c.SwaggerEndpoint("/author-service/swagger/v1/swagger.json", "Author Service");
                    c.SwaggerEndpoint("/book-service/swagger/v1/swagger.json", "Book Service");
                });
            }

            app.UseCors();

            app.UseHttpsRedirection();

            app.MapReverseProxy();

            app.Run();
        }
    }
}