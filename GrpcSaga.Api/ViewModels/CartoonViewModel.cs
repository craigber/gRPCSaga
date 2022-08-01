namespace Cartoonalogue.Api.ViewModels;

public class CartoonViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int YearBegin { get; set; }
    public int? YearEnd { get; set; }
    public string? Description { get; set; }
    public decimal? Rating { get; set; }
    public int StudioId { get; set; }
    public IList<CharacterViewModel>? Characters { get; set; }
}
