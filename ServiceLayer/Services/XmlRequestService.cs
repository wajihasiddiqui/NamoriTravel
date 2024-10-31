using ServiceLayer.ServiceInterfaces;
using DomainLayer.Entities;
using System.Xml.Linq;
using System.Text;
using DomainLayer;
using AutoMapper;
using ModelsDTO;

namespace ServiceLayer.Services
{
    internal class XmlRequestService : IXmlRequestService
    {
        private readonly IRepositoryManager _Repository;
        private readonly IMapper _mapper;
        public XmlRequestService(IRepositoryManager Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<XmlRequestDTO>> GetAllAsync(int? userId)
        {
            try
            {
                var Result = await _Repository.XmlRequestRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<XmlRequestDTO>>(Result);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching all XmlRequests", userId);
                throw;
            }
        }
        public async Task<XmlRequestDTO> GetByIdAsync(int id, int? userId)
        {
            try
            {
                var Result = await _Repository.XmlRequestRepository.GetByIdAsync(id);
                return _mapper.Map<XmlRequestDTO>(Result);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error fetching XmlRequest with ID {id}", userId);
                throw;
            }
        }
        public async Task AddAsync(XmlRequestDTO dto, int? userId)
        {
            try
            {
                var Result = _mapper.Map<XmlRequest>(dto);
                Result.CreatedDate = DateTime.UtcNow;
                Result.CreatedBy = userId;
                Result.IsActive = true;
                Result.IsDeleted = false;
                await _Repository.XmlRequestRepository.AddAsync(Result);

                await _Repository.AuditLogRepository.LogAuditAsync(userId, "XmlRequestService", "AddXmlRequest", $"Added XmlRequest with ID {Result.Id}");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error adding new XmlRequest", userId);
                throw;
            }
        }
        public async Task UpdateAsync(XmlRequestDTO dto, int? userId)
        {
            try
            {
                var Result = _mapper.Map<XmlRequest>(dto);
                Result.ModifiedDate = DateTime.UtcNow;
                Result.ModifiedBy = userId;
                Result.IsActive = true;
                Result.IsDeleted = false;
                await _Repository.XmlRequestRepository.UpdateAsync(Result);

                await _Repository.AuditLogRepository.LogAuditAsync(userId, "XmlRequestService", "UpdateXmlRequest", $"Updated XmlRequest with ID {Result.Id}");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error updating XmlRequest with ID {dto.Id}", userId);
                throw;
            }
        }
        public async Task DeleteAsync(XmlRequestDTO dto, int? userId)
        {
            try
            {
                var Result = _mapper.Map<XmlRequest>(dto);
                Result.IsDeleted = true;
                Result.ModifiedDate = DateTime.UtcNow;
                Result.ModifiedBy = userId;
                await _Repository.XmlRequestRepository.UpdateAsync(Result);

                await _Repository.AuditLogRepository.LogAuditAsync(userId, "XmlRequestService", "Delete XmlRequest", $"Deleted XmlRequest with ID {Result.Id}");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error deleting XmlRequest with ID {dto.Id}", userId);
                throw;
            }
        }
        public async Task<(IEnumerable<XmlRequestDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize)
        {
            try
            {
                if (Common.Common.IsStringValue(search))
                {
                    var Result = await _Repository.XmlRequestRepository.GetByNameAsync(search);
                    return (_mapper.Map<IEnumerable<XmlRequestDTO>>(Result.Items), Result.Total);
                }
                else
                {
                    var data = await _Repository.XmlRequestRepository.GetAllByFilteredAsync(userId, search, sortColumn, sortColval, sortOrder, page, pageSize);
                    return (_mapper.Map<IEnumerable<XmlRequestDTO>>(data.Items), data.TotalCount);
                }
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching all XmlRequests", userId);
                throw;
            }
        }
        public async Task<string> GenerateRequestXmlAsync(string UserType, string requestType, Dictionary<string, string> parameters, int userId)
        {
            try
            {
                // Retrieve the header XML request for user-related requests
                var xmlHeaderRequest = await _Repository.XmlRequestRepository.GetXmlRequestByTypeAsync(UserType);

                // Retrieve the XML request template based on the request type
                var xmlRequest = await _Repository.XmlRequestRepository.GetXmlRequestByTypeAsync(requestType);

                if (xmlHeaderRequest == null || xmlRequest == null)
                {
                    throw new Exception($"Request type '{requestType}' or header 'UserType' not found");
                }

                // Retrieve user details
                var dotwConnect = await _Repository.DotwRequestRepository.GetAllAsync();
                var newDotwConnect = dotwConnect.FirstOrDefault();

                // Combine header and request XML
                string completeRequestXml = xmlHeaderRequest.RequestXml.Replace("{requestXml}", xmlRequest.RequestXml);

                // Replace placeholders with actual values
                foreach (var param in parameters)
                {
                    completeRequestXml = completeRequestXml.Replace($"{{{{{param.Key}}}}}", param.Value);
                }

                // Replace header placeholders
                completeRequestXml = completeRequestXml
                    .Replace("{{username}}", newDotwConnect.Username)
                    .Replace("{{password}}", newDotwConnect.Password)
                    .Replace("{{id}}", newDotwConnect.CompanyId == null ? string.Empty : newDotwConnect.CompanyId.ToString())
                    .Replace("{{source}}", newDotwConnect.Source == null ? string.Empty : newDotwConnect.Source)
                    .Replace("{{product}}", newDotwConnect.Product == null ? string.Empty : newDotwConnect.Product);


                // Trim unnecessary whitespace and line breaks
                completeRequestXml = completeRequestXml.Trim();

                // Load the XML into an XDocument
                var xDocument = XDocument.Parse(completeRequestXml, LoadOptions.PreserveWhitespace);

                // Return the formatted XML string
                return xDocument.ToString();

                //return completeRequestXml;
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error generating XML request for type {requestType} and Header DetailsType: {UserType}", userId);
                throw;
            }
        }
        public async Task<string> SendDotWConnectRequestAsync(string requestXml, int userId, string URL)
        {
            try
            {
               
                using (var httpClient = new HttpClient())
                {
                    var content = new StringContent(requestXml, Encoding.UTF8, "text/xml");

                    // Sending POST request to DOTWConnect API
                    HttpResponseMessage response = await httpClient.PostAsync(URL, content);

                    // Check if the response was successful
                    response.EnsureSuccessStatusCode();

                    // Read the response as string
                    string responseXml = await response.Content.ReadAsStringAsync();

                    return responseXml;
                }
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error sending request to DOTWConnect API", userId);
                throw;
            }
        }
    }
}
