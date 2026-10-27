using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities
{
    public class Vacation:OtherBaseEntity
    {
        public DateTime StartData { get; set; }
        public DateTime EndData =>StartData.AddDays(NumberOfDays);
        public int NumberOfDays { get; set; }
        [Required]
        public int VacationTypeId { get; set; }
        public VacationType VacationType { get; set; }
    }
}
