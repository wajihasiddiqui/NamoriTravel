
namespace NamoriTravel.ModelsDTO
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Iconsvg { get; set; }
        public List<MenuItem> Children { get; set; } = new List<MenuItem>();
    }
}
