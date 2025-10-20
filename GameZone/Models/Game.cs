namespace GameZone.Models
{
    public class Game:BaseEntity
    {

        [MaxLength(2500)]
        public string Description { get; set; } = string.Empty;
        [MaxLength(500)]
        public string Cover { get; set; } = string.Empty;
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!; // -default- will create an object from the type
        public ICollection<GameDevice> Device { get; set; } = new List<GameDevice>();
    }
}
