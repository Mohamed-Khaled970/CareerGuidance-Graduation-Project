/*
 * File Name: Program.cs
 * Author Information: Mohamed Khaled , Abdelrahman Rezk
 * Date of creation: 2024-08-09
 * Versions Information: v1.0
 * Dependencies: 
 *      - using Microsoft.Extensions.DependencyInjection;
 *      - using Microsoft.EntityFrameworkCore;
 *      - using Microsoft.AspNetCore.Identity;
 *      - using FluentValidation;
 * Contributors: Mohamed Khaled
 * Last Modified Date: 2024-10-27
 */

using CareerGuidance.Api;
using CareerGuidance.Api.Services;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NSwag.Generation.Processors.Security;
using NSwag;

var builder = WebApplication.CreateBuilder(args); // Initialize web application with default settings

// Add services to the container
builder.Services.AddControllers(); // Add controller services

// Configure CORS policy to allow any origin, method, and header
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(builder =>
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
        )
);
// Retrieve the connection string from the configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    // Throw an error if the connection string is not found
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Add DbContext service and configure it to use SQL Server with the connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add Identity services for user management and configure it to use the DbContext
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders(); ;

// Add AuthService to provide authentication services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IAllRoadmapsInsertedService, allRoadmapsInsertedService>();
builder.Services.AddScoped<IEndPointHomePageService, EndPointHomePageService>();
builder.Services.AddScoped<IEndPointStartHereService, EndPointStartHereService>();
builder.Services.AddScoped<IuserProfileServices, userProfileServices>();
builder.Services.AddScoped<IprogressBarServices, progressBarServices>();
builder.Services.AddScoped<IInterviewService, InterviewService>();
builder.Services.AddScoped<IFileService, FileService>();




// Configure Fluent Validation for automatic validation of data
builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Register JwtProvider for JWT-related functionality
builder.Services.AddSingleton<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IEmailSender, EmailService>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
builder.Services.AddHttpContextAccessor();


// Mapster
var mappingConfig = TypeAdapterConfig.GlobalSettings;
mappingConfig.Scan(Assembly.GetExecutingAssembly());

builder.Services.AddSingleton<IMapper>(new Mapper(mappingConfig));
////////////////////////////////////////////////////////////
// Configure JWT authentication for the API
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    o.SaveToken = true; // Save the token in HttpContext
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // Validate the signing key
        ValidateIssuer = true, // Validate the issuer
        ValidateAudience = true, // Validate the audience
        ValidateLifetime = true, // Validate token expiration
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("6ksVC2PjX5wkenicens4ydUmGHKitRiT")),
        ValidIssuer = "CareerGuidanceApp", // Issuer
        ValidAudience = "CareerGuidanceApp users" // Audience
    };
});

// Add Google Configuration 
builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<GoogleAuthConfig>>().Value);
builder.Services.Configure<GoogleAuthConfig>(builder.Configuration.GetSection("Google"));

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 8;
    // options.SignIn.RequireConfirmedEmail = true;
    options.User.RequireUniqueEmail = true;
});

// Configure Identity options
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default user settings
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_@.";
    options.User.RequireUniqueEmail = true; // Require unique email for each user
});

// Enable Swagger for API documentation in development mode
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Nswag Service
builder.Services.AddOpenApiDocument(option =>
{
    option.AddSecurity("Bearer", new NSwag.OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.Http,
        Scheme = "bearer", 
        BearerFormat = "JWT",
        In = OpenApiSecurityApiKeyLocation.Header,
        Name = "Authorization",
        Description = "Enter 'Bearer' followed by a space and the JWT token."
    });

    option.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors(); // Enable CORS middleware to use the configured CORS policy

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run(); // Run the application
