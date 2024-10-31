
namespace ModelsDTO
{
    public class DotwRequestDTO : BaseEntityDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int? IDs { get; set; }
        public string? CompanyId { get; set; }
        public string? Source { get; set; }
        public string? Product { get; set; }
    }
}
