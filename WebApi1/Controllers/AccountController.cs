using ClassLibrary1.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ClassLibrary1.DTOs;
using WebApi.Services;

namespace WebApi.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
//Register and Login
public class AccountController : ControllerBase
{
    private IAccountService accountService;

    public AccountController(IAccountService accountService)
    {
        this.accountService = accountService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        UserDto userDto = await accountService.LoginAsync(loginDto);
        switch (userDto.LoginStatus)
        {
            case LoginStatus.InvalidEmail:
                return Unauthorized("Email is not registered");
            case LoginStatus.InvalidPassword:
                return Unauthorized("Incorrect password");
        }
        return userDto;
    }

    //user passes RegisterDto and on registration is returned a UserDto
    //which contains the JWT
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        UserDto userDto = await accountService.RegisterAsync(registerDto);
        switch (userDto.LoginStatus)
        {
            case LoginStatus.InvalidEmail:
                return BadRequest("Email Taken");
            case LoginStatus.InvalidUsername:
                return BadRequest("Username Taken");
            case LoginStatus.Fail:
                return BadRequest("Problem registering user");
        }
        return userDto;

    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        string email = User.FindFirstValue(ClaimTypes.Email);
        return await accountService.GetCurrentUserAsync(email);
    }
}

