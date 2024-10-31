
namespace ModelsDTO
{
    public class FormDefinitionDTO:BaseEntityDTO
    {
        public string Name { get; set; }
        public int VisitorTypeId { get; set; }
    }
    public class FieldDefinitionDTO : BaseEntityDTO
    {
        public int FormId { get; set; }
        public string Label { get; set; }
        public string FieldType { get; set; }
        public bool IsRequired { get; set; }
        public string DefaultValue { get; set; }
    }
}
