using System.ComponentModel.DataAnnotations;

namespace CartoonDomain.Common.Data.Entities
{
    public class Character
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public int CartoonId { get; set; }
    }
}
