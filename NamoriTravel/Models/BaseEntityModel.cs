namespace NamoriTravel.Models
{
    public class BaseEntityModel
    {
        public int Id { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }= DateTime.UtcNow;
        public bool? IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}
