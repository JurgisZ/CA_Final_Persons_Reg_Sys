
using CA_Final_Persons_Reg_Sys.Data;
using CA_Final_Persons_Reg_Sys.Repositories;
using CA_Final_Persons_Reg_Sys.Repositories.Interfaces;
using CA_Final_Persons_Reg_Sys.Services;
using CA_Final_Persons_Reg_Sys.Services.Interfaces;
using CA_Final_Persons_Reg_Sys.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace CA_Final_Persons_Reg_Sys
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<UsersDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IUserMapper, UserMapper>();
            builder.Services.AddScoped<IUserPersonalDataMapper, UserPersonalDataMapper>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IPictureRepository, PictureRepository>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPictureService, PictureService>();
            builder.Services.AddScoped<IJwtService, JwtService>();

            //Cors config, see cors middleware below
            builder.Services.AddCors(options =>
            {
                var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

                options.AddDefaultPolicy(policy =>       //"AllowSpecificOrigin", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10 MB
            });

            builder.Services.AddControllers();

            //Jwt Authorisation and authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
                };
            });
            builder.Services.AddAuthorization();    //with Jwt

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Person Reg System API", Version = "v1" });
                //IFormFile as file (binary data)
                c.MapType<IFormFile>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "binary"
                });
                c.OperationFilter<SwaggerFileOperationFilter>();

            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();   //Order is important!

            app.UseCors();                          //use Cors global default policy
            //app.UseCors("AllowSpecificOrigin");   //Cors added. Specific policy used in controller [EnableCors("PolicyName")]

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
