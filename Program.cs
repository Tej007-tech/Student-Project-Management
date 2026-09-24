using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

using Scalar.AspNetCore;
using StudentProjectAPI.Data;

using FluentValidation;
using StudentProjectAPI.Validators;
using StudentProjectAPI.Services;
using StudentProjectAPI.Repository;

namespace StudentProjectAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // =========================================
            // Controllers
            // =========================================
            builder.Services.AddControllers();

            // =========================================
            // Repositories & Services
            // =========================================
            builder.Services.AddScoped<TokenService>();
            builder.Services.AddScoped<ISPM_UserRepository, SPM_UserRepository>();
            builder.Services.AddScoped<ISPM_UserService, SPM_UserService>();

            // =========================================
            // Database
            // =========================================
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ));

            // =========================================
            // FluentValidation
            // =========================================
            builder.Services.AddValidatorsFromAssemblyContaining<SPM_UserValidator>();

            // =========================================
            // CORS
            // =========================================
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // =========================================
            // JWT Authentication
            // =========================================
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]!
                        )
                    ),

                    RoleClaimType = System.Security.Claims.ClaimTypes.Role,
                    NameClaimType = System.Security.Claims.ClaimTypes.NameIdentifier
                };
            });

            // =========================================
            // OpenAPI / Scalar
            // =========================================
            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer(
                    (document, context, cancellationToken) =>
                    {
                        document.Components ??= new();

                        // Ensure SecuritySchemes dictionary is initialized to avoid NullReferenceException
                        document.Components.SecuritySchemes ??= new System.Collections.Generic.Dictionary<string, IOpenApiSecurityScheme>();

                        document.Components.SecuritySchemes.Add(
                            "Bearer",
                            new OpenApiSecurityScheme
                            {
                                Type = SecuritySchemeType.Http,
                                Scheme = "bearer",
                                BearerFormat = "JWT",
                                In = ParameterLocation.Header,
                                Description =
                                    "Enter your JWT token here"
                            });

                        return Task.CompletedTask;
                    });
            });

            // =========================================
            // Build
            // =========================================
            var app = builder.Build();

            // =========================================
            // HTTP Request Pipeline
            // =========================================
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference("/");
            }

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            // JWT Authentication MUST come before Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}