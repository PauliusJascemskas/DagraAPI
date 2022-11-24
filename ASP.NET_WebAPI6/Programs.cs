using ASP.NET_WebAPI6.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;









//var builder = WebApplication.CreateBuilder(args);
//IConfiguration _configuration[];
//// Add services to the container.
////builder.Services.AddEntityFrameworkMySQL()
////                .AddDbContext<DBContext>(options =>
////                {
////                    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
////                });

//builder.Services.AddIdentity<RestUser, IdentityRole>()
//                .AddEntityFrameworkStores<DBContext>()
//                .AddDefaultTokenProviders();

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//                .AddJwtBearer(options =>
//                {
//                    options.TokenValidationParameters.ValidAudience = _configuration["JWT:ValidAudience"];
//                    options.TokenValidationParameters.ValidIssuer = _configuration["JWT:ValidIssuer"];
//                    options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
//                });

//builder.Services.AddControllers();

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

////Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
