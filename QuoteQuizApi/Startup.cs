using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuoteQuizApi.Interfaces;
using QuoteQuizApi.Repositories;
using QuoteQuizApi.AutoMapper;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace QuoteQuizApi
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
            services.AddCors(); // Make sure you call this previous to AddMvc
            services.AddMvc();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            IConfigurationSection appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            AppSettings appSettings = appSettingsSection.Get<AppSettings>();
            services.AddControllers(setupActin =>
            {
                setupActin.ReturnHttpNotAcceptable = true;
                setupActin.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "QuoteQuizApi", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
       "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement(){
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {Type = ReferenceType.SecurityScheme,Id = "Bearer"},
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
        new List<string>()
    }
});

            });
            var connection = $"Server=tcp:{appSettings.SqlServerHostName},{appSettings.SqlServerPost};Initial Catalog={appSettings.SqlServerCatalog};Persist Security Info=False;User ID={appSettings.SqlServerUser};Password={appSettings.SqlServerPassword};MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False;Connection Timeout=30;";
            services.AddDbContext<DataContext>(options =>
            {
                //options.UseSqlServer("Data Source=(localdb)\\ProjectModels;Initial Catalog=QuoteDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                options.UseSqlServer(connection);
            });
            services.AddAutoMapper(c => c.AddProfile<AutoMapperProfile>(), typeof(Startup));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            var keyBytes = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = "JwtBearer";
                opts.DefaultChallengeScheme = "JwtBearer";
            }).AddJwtBearer("JwtBearer", jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
    
            if (env.IsDevelopment())
            {
           
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "QuoteQuizApi v1");


                });
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "QuoteQuizApi v1");
                    c.RoutePrefix = string.Empty;
                });
            }
       
            app.UseCors(
       optionsx => optionsx.WithOrigins("*").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
            app.UseCors("AllowSpecificOrigin");
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
