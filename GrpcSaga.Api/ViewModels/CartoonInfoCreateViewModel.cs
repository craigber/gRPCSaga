namespace Cartoonalogue.Api.ViewModels;

public class CartoonDetailsCreateViewModel
{
    public CartoonCreateViewModel Cartoon { get; set; }

    public List<CharacterCreateViewModel?> Characters { get; set; }

    public StudioCreateViewModel Studio { get; set; }
}
