using System.ComponentModel.DataAnnotations;

namespace Cartoonalogue.Api.ViewModels;

public class CartoonCreateViewModel
{
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }

    [Required(ErrorMessage = "YearBegin is required")]
    public int YearBegin { get; set; }
    public int? YearEnd { get; set; }
    public string? Description { get; set; }

    [Range(0, 5, ErrorMessage = "Rating must be a value from 0-5")]
    public int? Rating { get; set; }

    [Required(ErrorMessage ="StudioId is required")]
    public int StudioId { get; set; }

}
