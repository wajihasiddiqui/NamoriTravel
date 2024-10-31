using ModelsDTO;

namespace ServiceLayer.ServiceInterfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto registerDto);
        Task<string> LoginAsync(LoginDto loginDto);
    }
}
