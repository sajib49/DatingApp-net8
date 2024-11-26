using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Services;
using API.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using API.Data;

namespace API.Extentions
{
    public static class ApplicationServiceExtentions
    {
        public static IServiceCollection AddAccountServices(this IServiceCollection services,
            IConfiguration config)
            {
                services.AddControllers();
                services.AddDbContext<DataContext>(opt => {
                    opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
                });
                services.AddCors();
                services.AddScoped< ITokenService, TokenService>();

                return services;
            }
    }
}