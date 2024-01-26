﻿using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Services.IService;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Services
{
	public class AuthService : IAuthService
	{
		private readonly AppDbContext _db;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext db, 
				UserManager<ApplicationUser> userManager, 
				RoleManager<IdentityRole> roleManager, 
				IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
			_userManager = userManager;
			_roleManager = roleManager;
			_jwtTokenGenerator = jwtTokenGenerator;
        }

		public async Task<bool> AssignRole(string email, string roleName)
		{
			var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
			var isRoleExist = await _roleManager.RoleExistsAsync(roleName);

			if(user != null)
			{
				if (!isRoleExist)
				{
					await _roleManager.CreateAsync(new IdentityRole(roleName));
				}

				await _userManager.AddToRoleAsync(user, roleName);
				return true;
			}

			return false;
		}

		public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
		{
			var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == loginRequestDto.UserName);
			bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

			if(user == null || isValid == false)
			{
				return new LoginResponseDto() { User = null, Token = "" };
			}

            IList<string> roles = await _userManager.GetRolesAsync(user);
			var token = _jwtTokenGenerator.GenerateToken(user, roles);

			UserDto userDto = new()
			{
				Email = user.Email,
				ID = user.Id,
				Name = user.Name,
				PhoneNumber = user.PhoneNumber
			};

			return new LoginResponseDto()
			{
				User = userDto,
				Token = token
			};
		}

		public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
		{
			ApplicationUser user = new()
			{
				UserName = registrationRequestDto.Email,
				Email = registrationRequestDto.Email,
				NormalizedEmail = registrationRequestDto.Email.ToUpper(),
				Name = registrationRequestDto.Name,
				PhoneNumber = registrationRequestDto.PhoneNumber
			};

			try
			{
				var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);

				if(result.Succeeded)
				{
					return "";
				} else
				{
					return result.Errors.FirstOrDefault().Description;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
