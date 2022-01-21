using System.ComponentModel.DataAnnotations.Schema;

namespace Api.VlsDomain.EntityModel
{
    [Table("epcis_object_event")]
    public class EpcisObjectEvent
    {
        [Column("id")]
        public Guid Id { get; set; }
        
        [Column("action")]
        public string? Action { get; set; } 
    }
}
