using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities
{
    public class Snaction:OtherBaseEntity
    {
        [Required]
        public DateTime Data { get; set; }
        [Required]
        public string ?Punishment { get; set; }
        [Required]
        public DateTime PunishmentDate { get; set; }
        public SnactionType SnactionType { get; set; }
    }
}
