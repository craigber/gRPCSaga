using System.ComponentModel.DataAnnotations;

namespace Cartoonalogue.Api.ViewModels;

public class CartoonUpdateViewModel
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
    
    [Required]
    public int StudioId { get; set; }
}
