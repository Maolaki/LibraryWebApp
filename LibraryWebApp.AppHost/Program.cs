var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.LibraryWebApp_ApiGatewayYARP>("librarywebapp-apigatewayyarp");

builder.AddProject<Projects.AuthService_API>("librarywebapp-authservice");

builder.AddProject<Projects.BookService_API>("librarywebapp-bookservice");

builder.AddProject<Projects.AuthorService_API>("librarywebapp-authorservice");

builder.Build().Run();
