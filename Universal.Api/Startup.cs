using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using Universal.Api.Data;
using Universal.Api.Data.Repositories;

namespace Universal.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();

            // configure strongly typed settings objects
            var section = Configuration.GetSection("AppSettings");
            var settings = section.Get<Settings>();
            services.Configure<Settings>(section);

            services.AddDbContext<UICContext>(options =>
            {
                options
                    .UseLazyLoadingProxies()
                    .UseSqlServer(settings.SqldbConnString)
                    .ReplaceService<IQueryTranslationPostprocessorFactory, SqlServer2008QueryTranslationPostprocessorFactory>();
            });

            services.AddControllers();

            services.AddSwaggerGen(s => {
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                s.IncludeXmlComments(xmlPath);
                s.CustomOperationIds(apiDescription =>
                {
                    return apiDescription.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
                });
                s.AddServer(new Microsoft.OpenApi.Models.OpenApiServer() { Url = "http://uic-middleware-api.gibsonline.com" });

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });
            });

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.Events = new JwtBearerEvents()
                {
                    OnChallenge = context => {
                        context.HandleResponse();
                        var code = StatusCodes.Status401Unauthorized;

                        var res = new ProblemDetails();
                        res.Status = code;
                        res.Title = "Unauthorized access";
                        res.Type = "https://httpstatuses.com/401";
                        res.Detail = "Invalid or expired access token.";

                        context.Response.StatusCode = code;
                        context.HttpContext.Response.Headers.Append("www-authenticate", "Bearer");

                        return context.Response.WriteAsync(JsonConvert.SerializeObject(res));
                    },
                    OnForbidden = context =>
                    {
                        var code = StatusCodes.Status403Forbidden;

                        var res = new ProblemDetails();
                        res.Status = code;
                        res.Title = "Forbidden request";
                        res.Type = "https://httpstatuses.com/403";
                        res.Detail = "You do not have the permission to access the resource.";

                        context.Response.StatusCode = code;

                        return context.Response.WriteAsync(JsonConvert.SerializeObject(res));
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.JwtSecret))
                };
            });

            services.AddSingleton(settings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Universal.Api V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
