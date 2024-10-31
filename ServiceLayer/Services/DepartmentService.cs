using ServiceLayer.ServiceInterfaces;
using DomainLayer.Entities;
using DomainLayer;
using AutoMapper;
using ModelsDTO;

namespace ServiceLayer.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        

        public DepartmentService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            
        }

        public async Task<IEnumerable<DepartmentDTO>> GetAllDepartmentAsync(int? userId)
        {
            try
            {
                var departments = await _repository.DepartmentRepository.GetAllAsync();
                var activeDepartments = departments.Where(d => !d.IsDeleted);
                return _mapper.Map<IEnumerable<DepartmentDTO>>(activeDepartments);
            }
            catch (Exception ex)
            {
                await _repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching all departments", userId);
                throw;
            }
        }

        public async Task<DepartmentDTO> GetDepartmentByIdAsync(int id, int? userId)
        {
            try
            {
                var department = await _repository.DepartmentRepository.GetByIdAsync(id);
                return _mapper.Map<DepartmentDTO>(department);
            }
            catch (Exception ex)
            {
                await _repository.ErrorLogRepository.LogErrorAsync(ex, $"Error fetching department with ID {id}", userId);
                throw;
            }
        }

        public async Task AddDepartmentAsync(DepartmentDTO dto, int? userId)
        {
            try
            {
                var department = _mapper.Map<Department>(dto);
                department.CreatedDate = DateTime.UtcNow;
                department.CreatedBy = userId;
                await _repository.DepartmentRepository.AddAsync(department);

                await _repository.AuditLogRepository.LogAuditAsync(userId, "DepartmentService", "AddDepartment", $"Added department with ID {department.Id}");
            }
            catch (Exception ex)
            {
                await _repository.ErrorLogRepository.LogErrorAsync(ex, "Error adding new department", userId);
                throw;
            }
        }

        public async Task UpdateDepartmentAsync(DepartmentDTO dto, int? userId)
        {
            try
            {
                var department = _mapper.Map<Department>(dto);
                department.ModifiedDate = DateTime.UtcNow;
                department.ModifiedBy = userId;
                await _repository.DepartmentRepository.UpdateAsync(department);

                await _repository.AuditLogRepository.LogAuditAsync(userId, "DepartmentService", "UpdateDepartment", $"Updated department with ID {department.Id}");
            }
            catch (Exception ex)
            {
                await _repository.ErrorLogRepository.LogErrorAsync(ex, $"Error updating department with ID {dto.Id}", userId);
                throw;
            }
        }

        public async Task DeleteDepartmentAsync(DepartmentDTO dto, int? userId)
        {
            try
            {
                var department = _mapper.Map<Department>(dto);
                department.IsDeleted = true;
                department.ModifiedDate = DateTime.UtcNow;
                department.ModifiedBy = userId;
                await _repository.DepartmentRepository.UpdateAsync(department);

                await _repository.AuditLogRepository.LogAuditAsync(userId, "DepartmentService", "DeleteDepartment", $"Deleted department with ID {department.Id}");
            }
            catch (Exception ex)
            {
                await _repository.ErrorLogRepository.LogErrorAsync(ex, $"Error deleting department with ID {dto.Id}", userId);
                throw;
            }
        }
        public async Task<(IEnumerable<DepartmentDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize)
        {
            try
            {
                var data = await _repository.DepartmentRepository.GetAllByFilteredAsync(userId, search, sortColumn, sortColval, sortOrder, page, pageSize);
                return (_mapper.Map<IEnumerable<DepartmentDTO>>(data.Items), data.TotalCount);
            }
            catch (Exception ex)
            {
                await _repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching GetAllFilteredAsync Department", userId);
                throw;
            }
        }
    }
}
