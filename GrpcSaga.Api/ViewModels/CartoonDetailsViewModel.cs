namespace Cartoonalogue.Api.ViewModels;

public class CartoonDetailsViewModel
{
    public CartoonViewModel Cartoon { get; set; }

    public IList<CharacterViewModel>? Characters { get; set; }
    public StudioViewModel Studio { get; set; }
}
