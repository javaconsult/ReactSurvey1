using AutoMapper;
using ClassLibrary1.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ClassLibrary1.DTOs;
using WebApi.Services;

namespace WebApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AdminUser> userManager;
        private readonly SignInManager<AdminUser> signInManager;
        private readonly TokenService tokenService;
        private readonly IMapper mapper;

        public AccountService(UserManager<AdminUser> userManager, SignInManager<AdminUser> signInManager, TokenService tokenService, IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            //check if email exists
            AdminUser adminUser = await userManager.FindByEmailAsync(loginDto.Email);
            if (adminUser == null)
                return new UserDto { LoginStatus = LoginStatus.InvalidEmail };

            //attempt to sign in with supplied password
            SignInResult result = await signInManager.CheckPasswordSignInAsync(adminUser, loginDto.Password, false);
            if (result.Succeeded == false)
                return new UserDto { LoginStatus = LoginStatus.InvalidPassword };

            UserDto userDto = mapper.Map<UserDto>(adminUser); //map AdminUser to UserDto
            userDto.Token = tokenService.CreateToken(adminUser);
            return userDto;
        }


        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await userManager.Users.AnyAsync(adminUser => adminUser.Email == registerDto.Email))
                return new UserDto { LoginStatus = LoginStatus.InvalidEmail };

            if (await userManager.Users.AnyAsync(u => u.UserName == registerDto.UserName))
                return new UserDto { LoginStatus = LoginStatus.InvalidUsername };
            
            //map RegisterDto to AdminUser
            AdminUser adminUser = mapper.Map<AdminUser>(registerDto); 
            //create a new AdminUser
            IdentityResult result = await userManager.CreateAsync(adminUser, registerDto.Password);

            if (result.Succeeded == false)
                return new UserDto { LoginStatus = LoginStatus.Fail };

            UserDto userDto = mapper.Map<UserDto>(adminUser); //map AdminUser to UserDto
            userDto.Token = tokenService.CreateToken(adminUser);
            return userDto;
        }

        public async Task<UserDto> GetCurrentUserAsync(string email)
        {    
            AdminUser adminUser = await userManager.FindByEmailAsync(email);
            UserDto userDto = mapper.Map<UserDto>(adminUser); //map AdminUser to UserDto
            userDto.Token = tokenService.CreateToken(adminUser);
            return userDto;
        }
    }
}
