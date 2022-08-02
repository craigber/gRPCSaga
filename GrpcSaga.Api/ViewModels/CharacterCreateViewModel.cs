using System.ComponentModel.DataAnnotations;

namespace Cartoonalogue.Api.ViewModels;

public class CharacterCreateViewModel
{
    [Required(ErrorMessage = "Character Name is required")]
    public string Name { get; set; }

    public string? Description { get; set; }

    [Required(ErrorMessage = "CartoonId is required")]
    public int CartoonId { get; set; }
}
