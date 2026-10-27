namespace BaseLibrary.Entities
{
    public class City : BaseEntity
    {
        public int CountryId { get; set; }
        public country Country { get; set; }
        public List<Town> Towns { get; set; } = new List<Town>();
    }
}
