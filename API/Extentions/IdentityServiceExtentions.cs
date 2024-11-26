using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Extentions
{
    public static class IdentityServiceExtentions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services,
            IConfiguration config)
            {
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    var tokenKey = config["TokenKey"] ?? throw new Exception("Token key not found");
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                }); 

                return services;
            }
    }
}