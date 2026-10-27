namespace BaseLibrary.Entities
{
    public class Town : BaseEntity
    {
        public City City { get; set; }
        public int CityId { get; set; }
        public List<Employee> Employees { get; set; } = new List<Employee>();

    }
}
