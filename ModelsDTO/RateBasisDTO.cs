
namespace ModelsDTO
{
    public class RateBasisDTO : BaseEntityDTO
    {
        public string Description { get; set; }
        public int Value { get; set; }
    }
    public class BusinessDTO : BaseEntityDTO
    {
        public string Description { get; set; }
        public int Value { get; set; }
    }
    public class AmenitiesDTO : BaseEntityDTO
    {
        public string Description { get; set; }
        public int Value { get; set; }
    }
    public class CurrencyDTO : BaseEntityDTO
    {
        public string Shortcut { get; set; }
        public int Value { get; set; }
        public string Description { get; set; }
    }
}
