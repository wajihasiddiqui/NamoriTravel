
namespace DomainLayer.Entities
{
    public class RateBasis:BaseEntity
    {
        public string Description { get; set; }
        public int Value { get; set; }
    }
    public class Busines: BaseEntity
    {
        public string Description { get; set; }
        public int Value { get; set; }
    }
    public class Amenities : BaseEntity
    {
        public string Description { get; set; }
        public int Value { get; set; }
    } 
    public class Currency : BaseEntity
    {
        public string Shortcut { get; set; }
        public int Value { get; set; }
        public string Description { get; set; }
    }
}
