
namespace DomainLayer.Entities
{
    public class Country : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Currency { get; set; }
        public bool IsActive { get; set; }
    }

}
