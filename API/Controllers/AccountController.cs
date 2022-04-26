using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class AccountController : BaseApiController  

 {
     

        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _sigInManager;
    
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> sigInManager, ITokenService tokenService, IMapper mapper)
        
        {
            _sigInManager = sigInManager;
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
          
        }
        
        [HttpPost("register")]
        public async Task <ActionResult<UserDto>>Register(RegisterDto registerDto)
        {
            if(await UserExists(registerDto.Username)) return BadRequest("Username is Taken");
            var user =_mapper.Map<AppUser>(registerDto);

         
                user.UserName = registerDto.Username.ToLower();
        
           var result = await _userManager.CreateAsync(user,registerDto.Password);

           if(!result.Succeeded) return BadRequest(result.Errors);
           var roleResult = await _userManager.AddToRoleAsync(user, "Member");
           if(!roleResult.Succeeded) return BadRequest(result.Errors);

            return new UserDto
            {
                Username = user.UserName,
                Token =await _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
               Gender =user.Gender
            };
        }

     
        
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>>Login(LoginDto loginDto)
        {
            var user = await _userManager.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == loginDto.username.ToLower());
            if(user == null) return Unauthorized("Invalid Username");

            var result =await _sigInManager
            .CheckPasswordSignInAsync(user, loginDto.password, false);

            if (!result.Succeeded) return Unauthorized();
          
          
           return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender

            };
        }

           private async Task<bool>UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName==username.ToLower());
        }

    }
    
    
}