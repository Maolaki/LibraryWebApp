var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.LibraryWebApp_ApiGatewayYARP>("librarywebapp-apigatewayyarp");

builder.AddProject<Projects.LibraryWebApp_AuthService>("librarywebapp-authservice");

builder.AddProject<Projects.LibraryWebApp_BookService>("librarywebapp-bookservice");

builder.AddProject<Projects.LibraryWebApp_AuthorService>("librarywebapp-authorservice");

builder.Build().Run();
