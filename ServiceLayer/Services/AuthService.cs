using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using ServiceLayer.ServiceInterfaces;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using DomainLayer.Entities;
using System.Text;
using DomainLayer;
using ModelsDTO;
using System.Data;

namespace ServiceLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IConfiguration _config;
        public AuthService(IRepositoryManager repositoryManager,IConfiguration configuration)
        {
            _config = configuration;
            _repositoryManager = repositoryManager;
        }

        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                var user = new User
                {
                    Username = registerDto.Username,
                    Email = registerDto.Email,
                    PasswordHash = Helper.Encryption.EncryptPasswordToSha256Hash(registerDto.Password),
                    IsDeleted = false,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                };

                await _repositoryManager.UserRepository.AddAsync(user);

                await _repositoryManager.AuditLogRepository.LogAuditAsync(null, "AuthService", "RegisterAsync", $"Registered new user with ID {user.Id}");

                return await GenerateToken(user);
            }
            catch (Exception ex)
            {
                await _repositoryManager.ErrorLogRepository.LogErrorAsync(ex, "Error registering user", null);
                throw;
            }
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _repositoryManager.UserRepository.GetUserByUsernameAsync(loginDto.Username,loginDto.Email, Helper.Encryption.EncryptPasswordToSha256Hash(loginDto.Password));

                if (user == null)
                {
                    throw new UnauthorizedAccessException("User not found");
                }
                if (user.PasswordHash !=  Helper.Encryption.EncryptPasswordToSha256Hash(loginDto.Password))
                {
                    throw new UnauthorizedAccessException("Invalid credentials");
                }

                await _repositoryManager.AuditLogRepository.LogAuditAsync(user.Id, "AuthService", "LoginUser", $"User with ID {user.Id} logged in");

                return await GenerateToken(user);
            }
            catch (Exception ex)
            {
                await _repositoryManager.ErrorLogRepository.LogErrorAsync(ex, "Error during login", null);
                throw;
            }
        }

        private async Task<string> GenerateToken(User user)
        {
            try
            {
                var permissions = await _repositoryManager.UserRepository.GetPermissionsForUserByIdAsync(user.Id);
                var Role = await _repositoryManager.RoleRepository.GetByIdAsync(Convert.ToInt32(user.RoleId));
               
                var key = Encoding.ASCII.GetBytes(_config["Jwt:SecretKey"]);
                var tokenHandler = new JwtSecurityTokenHandler();
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, Role.RoleName)
                };

                foreach (var permission in permissions)
                {
                    claims.Add(new Claim("PagePermission", $"{permission.PageName}:{permission.PageUrl}:{string.Join(",", permission.Permissions)}:{string.Join(",", permission.PageId)}"));
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var writtenToken = tokenHandler.WriteToken(token);

                return writtenToken;
            }
            catch (Exception ex)
            {
                await _repositoryManager.ErrorLogRepository.LogErrorAsync(ex, "Error generating token", null);
                throw;
            }
        }
    }
}
