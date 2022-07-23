namespace Cartoonalogue.Api.ViewModels;

public class CartoonCreateViewModel
{
    public string Title { get; set; }
    public int YearBegin { get; set; }
    public int? YearEnd { get; set; }
    public string? Description { get; set; }
    public int? Rating { get; set; }
    public int StudioId { get; set; }

}
