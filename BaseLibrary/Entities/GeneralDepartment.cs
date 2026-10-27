namespace BaseLibrary.Entities
{
    public class GeneralDepartment : BaseEntity
    {
        public List<Employee> Employees { get; set; } = new List<Employee>();

    }
}
