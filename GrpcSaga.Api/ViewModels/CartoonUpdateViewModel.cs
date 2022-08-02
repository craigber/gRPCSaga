using System.ComponentModel.DataAnnotations;

namespace Cartoonalogue.Api.ViewModels;

public class CartoonUpdateViewModel
{
    [Required(ErrorMessage = "Id is required")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }

    [Required(ErrorMessage = "YearBegin is required")]
    public int YearBegin { get; set; }
    public int? YearEnd { get; set; }
    public string? Description { get; set; }

    [Range(0, 5, ErrorMessage = "Rating must be in the range 0-5")]
    public decimal? Rating { get; set; }
    
    [Required(ErrorMessage = "StudioId is required")]
    public int StudioId { get; set; }
}
