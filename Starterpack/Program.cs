using System.Text;
using Common.Transport;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Starterpack.Auth.Api.Mutations;
using Starterpack.Auth.Domain.Services;
using Starterpack.Auth.Persistence;
using Starterpack.Common.Api;
using Starterpack.Common.Domain.Exceptions;
using Starterpack.User.Api.Mutations;
using Starterpack.User.Api.Queries;
using Starterpack.User.Api.Types;
using Starterpack.User.Domain.Models;
using Starterpack.User.Domain.Services;
using Starterpack.User.Persistance;

// pragma ignore for this file
#pragma warning disable SA1507
#pragma warning disable SA1028
#pragma warning disable SA1122

var builder = WebApplication.CreateBuilder(args);

// Configure everything using extension methods
builder.RegisterApplicationServices().ConfigureGraphQLServer();

var app = builder.Build();

app.MapGraphQL();

// app.UseWebSockets();

app.UseGraphQLVoyager("/graphql-voyager", new GraphQL.Server.Ui.Voyager.VoyagerOptions() { GraphQLEndPoint = "/graphql" });

Console.WriteLine("");
Console.WriteLine("-------------------------------------");
Console.WriteLine("|  WELCOME TO THE Starterpack API  |");
Console.WriteLine("-------------------------------------");
Console.WriteLine("");

var env = builder.Environment.IsDevelopment() ? "DEVELOPMENT" : "PRODUCTION";
Console.WriteLine($" --- The app is running in {env} environment ---");

app.Run();

public partial class Program
{
}

// Extension methods for services and server configuration

public static partial class ServiceInitializer
{
    public static WebApplicationBuilder RegisterApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddPooledDbContextFactory<AppDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("StarterpackDB")));

        builder.Services.AddAuthentication().AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Authentication:Secret")!)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
            };
        });

        builder.Services.AddAuthorization();

        // Define mapper 
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // Add builder.Services
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<IAuthService, AuthService>();
        builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

        // Add repositories (using transient lifecycle to use with pooled dbcontext)
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<IAuthRepository, AuthRepository>();

        // Fluent validation
        builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

        // For util use
        builder.Services.AddHttpContextAccessor();

        // Cors rules
        builder.Services
            .AddCors(o =>
                o.AddDefaultPolicy(b =>
                    b.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()));

        return builder;
    }
}

public static partial class GraphQLServerConfiguration
{
    public static WebApplicationBuilder ConfigureGraphQLServer(this WebApplicationBuilder builder)
    {
        // IRequestExecutorBUilder definition
        builder.Services
        .AddGraphQLServer()

        // This is called on both the IServiceCollection and the IRequestExecutorBuilder
        .AddAuthorization()

        // Parallellization of database access
        .RegisterDbContext<AppDbContext>(DbContextKind.Pooled)

        // For automatic conversion of "[Error...]" to union result types in the GraphQL schema for MUTATION TYPES
        .AddMutationConventions(applyToAllMutations: true)

        // Custom field middleware FOR QUERY TYPES
        .UseField<BaseExceptionMiddleware>()

        // General
        .AddQueryType()
        .AddMutationType()

        // Types
        .AddType<UserType>()


        // Extensions

        // Users
        .AddTypeExtension<UserQueries>()
        .AddTypeExtension<UserMutations>()

        // Auth
        .AddTypeExtension<AuthMutations>()

        // For subscriptions
        // .AddInMemorySubscriptions();

        // Custom http interceptor
        .AddHttpRequestInterceptor<CurrentUserHttpRequestInterceptor>()

        // Custom injection engine kind of vibe
        // Is the one that takes the Lazy<UserModel> from the global state provided by the http interceptor
        // And makes it freely available in the resolvers
        .AddParameterExpressionBuilder(
            ctx => ctx.GetGlobalStateOrDefault<Lazy<UserModel>>(nameof(UserModel)));


        return builder;
    }
}