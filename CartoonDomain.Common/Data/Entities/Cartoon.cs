using System.ComponentModel.DataAnnotations;

namespace CartoonDomain.Common.Data.Entities;

public class Cartoon
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public int YearBegin { get; set; }
    public int? YearEnd { get; set; }
    public string? Description { get; set; }
    public decimal? Rating { get; set; }
    public int StudioId { get; set; }
    public virtual IEnumerable<Character> Characters { get; set; }

    public bool IsValid()
    {
        if (string.IsNullOrEmpty(Title)) return false;
        if (YearBegin <= 0) return false;
        if (StudioId <= 0) return false;
        return true;
    }
}
