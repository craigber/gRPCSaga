using System.ComponentModel.DataAnnotations;

namespace Cartoonalogue.Api.ViewModels;

public class CharacterViewModel
{
    [Required(ErrorMessage = "Id is required")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Character name is required")]
    public string Name { get; set; }
    public string? Description { get; set; }

    [Required(ErrorMessage = "CartoonId is required")]
    public int CartoonId { get; set; }

}