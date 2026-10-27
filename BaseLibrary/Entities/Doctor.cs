using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities
{
    public class Doctor:OtherBaseEntity
    {
        [Required]
        public DateTime Data { get; set; }
        [Required]
        public string MedicalDiagnose { get; set; }
        [Required]
        public string MedicalRecommendation { get; set; }
    }
}
