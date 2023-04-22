using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using AdeshDatingApp.Data;
using AdeshDatingApp.DTOs;
using AdeshDatingApp.Entities;
using AdeshDatingApp.Interface;
using AdeshDatingApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AdeshDatingApp.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly DataContext c;
        private readonly ITokenService tokenService;
        public AccountController(DataContext c, ITokenService tokenService)
        {
            this.c = c;
            this.tokenService=tokenService;
        }

        [HttpPost("register")]     // POST : adeshdatingapp/account/register?uname=sam&pass=password
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {

            if(await UserExists(registerDTO.Username)) return BadRequest("UserName Already Exists.....");

            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = registerDTO.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                PasswordSalt = hmac.Key
            };
            c.Users.Add(user);
            await c.SaveChangesAsync();

            return new UserDTO
            {
                UserName=user.UserName,
                Token=tokenService.CreateToken(user)
            };
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await c.Users.SingleOrDefaultAsync(x => x.UserName == loginDTO.Username);
            if(user == null) return Unauthorized("Invalid Username");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
            
            for(int i=0; i<computedHash.Length; i++)
            {
                if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return new UserDTO
            {
                UserName=user.UserName,
                Token=tokenService.CreateToken(user)
            };

        }

        private async Task<bool> UserExists(string uname)
        {
            return await c.Users.AnyAsync(x => x.UserName == uname.ToLower());
        }
    }
}