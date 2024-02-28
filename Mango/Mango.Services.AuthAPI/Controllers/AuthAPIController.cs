﻿using Mango.MessageBus;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class AuthAPIController : ControllerBase
	{
		private readonly IAuthService _authService;
		private readonly IMessageBus _messageBus;
		private readonly IConfiguration _configuration;
		protected ResponseDto _response;
		public AuthAPIController(IAuthService authService, IMessageBus messageBus, IConfiguration configuration)
		{
			_authService = authService;
			_messageBus = messageBus;
			_configuration = configuration;
			_response = new();
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
		{
			var message = await _authService.Register(model);
			
			if(!string.IsNullOrEmpty(message))
			{
				_response.IsSuccess = false;
				_response.Message = message;

				return BadRequest(_response);
			}

			await _messageBus.PublishMessage(model.Email, _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue"));

			return Ok(_response);
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
		{
			var loginResponse = await _authService.Login(model);

			if(loginResponse.User == null)
			{
				_response.IsSuccess = false;
				_response.Message = "Username or password is incorrect";
				
				return BadRequest(_response);
			}

			_response.Result = loginResponse;
			return Ok(_response);
		}

		[HttpPost("assignRole")]
		public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto model)
		{
			var isAssignRole = await _authService.AssignRole(model.Email, model.Role.ToUpper());

			if(!isAssignRole)
			{
				_response.IsSuccess = false;
				_response.Message = "Error encountered";

				return BadRequest(_response);
			}

			return Ok(_response);
		}
	}
}
