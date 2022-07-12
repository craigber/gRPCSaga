using System.ComponentModel.DataAnnotations;

namespace CartoonDomain.Service.Data.Entities
{
    public class Cartoon
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int YearBegin { get; set; }
        public int? YearEnd { get; set; }
        public string? Description { get; set; }
        public decimal? Rating { get; set; }
        public int StudioId { get; set; }
    }
}
