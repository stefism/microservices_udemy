using Mango.Web.Models;
using Mango.Web.Service.IService;

namespace Mango.Web.Service
{
	public class AuthService : IAuthService
	{
		private readonly IBaseService _baseService;

        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> AssignRoleAsync(RegistrationRequestDto registrationRequestDto)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = Enums.ApiType.POST,
				Url = Helpers.AuthAPIBase + "/api/auth/assignRole",
				Data = registrationRequestDto
			});
		}

		public async Task<ResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = Enums.ApiType.POST,
				Url = Helpers.AuthAPIBase + "/api/auth/login",
				Data = loginRequestDto
			}, withBearer: false);
		}

		public async Task<ResponseDto> RegisterAsync(RegistrationRequestDto registrationRequestDto)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = Enums.ApiType.POST,
				Url = Helpers.AuthAPIBase + "/api/auth/register",
				Data = registrationRequestDto
			}, withBearer: false);
		}
	}
}
