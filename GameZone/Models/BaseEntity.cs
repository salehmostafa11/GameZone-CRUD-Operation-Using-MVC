namespace GameZone.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        [MaxLength(70)]
        public string Name { get; set; } = string.Empty;
    }
}
