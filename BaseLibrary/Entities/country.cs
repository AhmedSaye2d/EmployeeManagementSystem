namespace BaseLibrary.Entities
{
    public class country : BaseEntity
    {
        public List<City> Cities { get; set; } = new List<City>();
    }
}
