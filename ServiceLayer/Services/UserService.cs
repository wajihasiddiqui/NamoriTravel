using AutoMapper;
using DomainLayer;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using ServiceLayer.ServiceInterfaces;
using System.Security.Claims;
using ServiceLayer;
using System.Text;
using ModelsDTO;
using ServiceLayer.Helper;

namespace ServiceLayer.Services
{
    public class UserService(IRepositoryManager repositoryManager, IMapper mapper, IConfiguration configuration) : IUserService
    {
        private readonly IRepositoryManager _repositoryManager = repositoryManager;
        private readonly IMapper _mapper = mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _config = configuration;

        public async Task<List<UserDTO>> GetAllUsersAsync(int? userId)
        {
            try
            {
                var users = await _repositoryManager.UserRepository.GetAllAsync();
                return _mapper.Map<List<UserDTO>>(users);
            }
            catch (Exception ex)
            {
               await _repositoryManager.ErrorLogRepository.LogErrorAsync(ex, "Error fetching all users", userId);
                throw;
            }
        }

        public async Task<UserDTO> GetUserByIdAsync(int id, int? userId)
        {
            try
            {
                var user = await _repositoryManager.UserRepository.GetByIdAsync(id);
                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                await _repositoryManager.ErrorLogRepository.LogErrorAsync(ex, $"Error fetching user with ID {id}", userId);
                throw;
            }
        }

        public async Task AddUserAsync(UserDTO userDto, int? userId)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                user.PasswordHash = Helper.Encryption.EncryptPasswordToSha256Hash(userDto.PasswordHash);
                user.CreatedBy = userId;
                user.CreatedDate = DateTime.UtcNow;
                user.IsDeleted = false;
                user.IsActive = true;
                await _repositoryManager.UserRepository.AddAsync(user);

                await _repositoryManager.AuditLogRepository.LogAuditAsync(userId, "UserService", "AddUser", $"Added user with ID {user.Id}");
            }
            catch (Exception ex)
            {
                await _repositoryManager.ErrorLogRepository.LogErrorAsync(ex, "Error adding new user", userId);
                throw;
            }
        }

        public async Task UpdateUserAsync(UserDTO userDto, int? userId)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                user.ModifiedDate = DateTime.UtcNow;
                user.ModifiedBy = userId;
                await _repositoryManager.UserRepository.UpdateAsync(user);

                await _repositoryManager.AuditLogRepository.LogAuditAsync(userId, "UserService", "UpdateUser", $"Updated user with ID {user.Id}");
            }
            catch (Exception ex)
            {
                await _repositoryManager.ErrorLogRepository.LogErrorAsync(ex, $"Error updating user with ID {userDto.Id}", userId);
                throw;
            }
        }

        public async Task DeleteUserAsync(UserDTO userDto, int? userId)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                user.IsDeleted = true;
                user.ModifiedDate = DateTime.UtcNow;
                user.ModifiedBy = userId;
                await _repositoryManager.UserRepository.UpdateAsync(user);

                await _repositoryManager.AuditLogRepository.LogAuditAsync(userId, "UserService", "DeleteUser", $"Deleted user with ID {user.Id}");
            }
            catch (Exception ex)
            {
                await _repositoryManager.ErrorLogRepository.LogErrorAsync(ex, $"Error deleting user with ID {userDto.Id}", userId);
                throw;
            }
        }

        public async Task<(IEnumerable<UserDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize)
        {
            try
            {
                if (Common.Common.IsStringValue(search))
                {
                    var Result = await _repositoryManager.UserRepository.GetUserByNameAsync(search);
                    return (_mapper.Map<IEnumerable<UserDTO>>(Result.Items), Result.Total);
                }
                else
                {
                    var data = await _repositoryManager.UserRepository.GetAllByFilteredAsync(userId, search, sortColumn, sortColval, sortOrder, page, pageSize);
                    return (_mapper.Map<IEnumerable<UserDTO>>(data.Items), data.TotalCount);
                }
            }
            catch (Exception ex)
            {
                await _repositoryManager.ErrorLogRepository.LogErrorAsync(ex, "Error fetching all Users", userId);
                throw;
            }
        }
    }
}
