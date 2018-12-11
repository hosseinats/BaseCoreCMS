using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CMS.DTO;
using CMS.Entities;
using CMS.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signinManager;
        private readonly IConfiguration _configuration;
        private readonly MapperConfiguration _mapperConfiguration;

        public AccountController(UserManager<User> userManager, SignInManager<User> signinManager, IConfiguration configuration, MapperConfiguration mapperConfiguration)
        {
            this.userManager = userManager;
            this.signinManager = signinManager;
            _configuration = configuration;
            _mapperConfiguration = mapperConfiguration;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllUsers()
        {
            var userModel = userManager.Users.ToList();
            var userVM = _mapperConfiguration.CreateMapper().Map<List<UserListDTO>>(userModel);
            return Ok(userVM);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string userName = string.Empty;
            var user = await userManager.FindByNameAsync(loginVM.UserName);
            if (user == null)
            {
                user = await userManager.FindByEmailAsync(loginVM.UserName);
                if (user == null)
                {
                    return Unauthorized();
                }
            }
            userName = user.UserName;
            var result = await signinManager.PasswordSignInAsync(userName, loginVM.Password, loginVM.RememberMe, false);
            if (result.Succeeded)
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var jwtTokenString = new JwtSecurityToken(
                    issuer: _configuration["Tokens:Issuer"],
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: credential,
                    claims: new List<Claim>()
                    {
                       new Claim(ClaimTypes.Name , user.UserName),
                       new Claim( "UserId" , user.Id.ToString()),
                       new Claim( "FullName" , user.FullName.ToString()),
                    });
                var tokenResult = new JwtSecurityTokenHandler().WriteToken(jwtTokenString);
                return Ok(new { token = tokenResult });
            }
            else
            {
                return Unauthorized();
            }

        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {

            if (ModelState.IsValid )
            {

                var isUserNameExist = userManager.FindByNameAsync(registerVM.UserName).Result;
                if (isUserNameExist != null)
                {
                    return Conflict("UserExist");
                }
                var isMailExist = userManager.FindByEmailAsync(registerVM.Mail).Result;
                if (isMailExist != null)
                {
                    return Conflict("MailExist");
                }

                var user = new User
                {
                    FullName = registerVM.FullName,
                    UserName = registerVM.UserName,
                    Email = registerVM.Mail,
                    Gender = registerVM.Gender,
                    CreatedDateTime = DateTime.Now
                };
                var result = await userManager.CreateAsync(user, registerVM.Password);
                if (result.Succeeded)
                {
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                    var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var jwtTokenString = new JwtSecurityToken(
                        issuer: _configuration["Tokens:Issuer"],
                        expires: DateTime.UtcNow.AddMinutes(30),
                        signingCredentials: credential,
                        claims: new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name , user.UserName),
                            new Claim( "UserId" , user.Id.ToString()),
                        });
                    var tokenResult = new JwtSecurityTokenHandler().WriteToken(jwtTokenString);
                    return Ok(new { token = tokenResult });
                }
                else
                {
                    return BadRequest();
                }
            }

            return BadRequest();
        }

    }
}