using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Entities;
using API.Data;
using System.Security.Cryptography;
using System.Text;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using API.Services;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{

    [AllowAnonymous]
    public class AccountController(DataContext context, 
            ITokenService tokenService) : BaseApiController
    {
        
       [HttpPost("register")]
       public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
       {
            if(await IsUserExist(registerDto.UserName))
            {
                return BadRequest("Username is taken");
            }
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return new UserDto {
                UserName = user.UserName,
                Token = tokenService.CreateToken(user)
            } ;
       }

       [HttpPost("login")]
       public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
       {
            var user = await context.Users.SingleOrDefaultAsync(x=>
                x.UserName == loginDto.UserName.ToLower()
            ); 

            if(user == null)
            {
                return Unauthorized("Invalid username");
            }

            using var hmac= new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for(int i=0; i<computedHash.Length; i++)
            {
                if(computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid password");
                }
            }
            
            return new UserDto {
                UserName = user.UserName,
                Token = tokenService.CreateToken(user)
            };
       }

       private async Task<bool> IsUserExist(string userName)
       {
            return await context.Users.AnyAsync(x=> x.UserName.ToLower() == userName.ToLower());
       }
    }
}