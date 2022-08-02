using System.ComponentModel.DataAnnotations;

namespace Cartoonalogue.Api.ViewModels;

public class StudioCreateViewModel
{
    [Required(ErrorMessage = "Studio name is required")]
    public string Name { get; set; }
}
