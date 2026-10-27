namespace BaseLibrary.Entities
{
    public class SnactionType: BaseEntity
    {
        public List<Snaction> Snactions { get; set; } = new List<Snaction>();
    }
}
