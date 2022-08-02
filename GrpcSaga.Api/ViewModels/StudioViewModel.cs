using System.ComponentModel.DataAnnotations;

namespace Cartoonalogue.Api.ViewModels;

public class StudioViewModel
{
    [Required(ErrorMessage = "Studio Id is required")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Studio Id is required")]
    public string Name { get; set; }
}
