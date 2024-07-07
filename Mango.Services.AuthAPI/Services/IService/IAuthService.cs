using Mango.Services.AuthAPI.Models.Dto;

namespace Mango.Services.AuthAPI.Services.IService
{
    public interface IAuthService
    {
        Task<UserDto> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    }
}
