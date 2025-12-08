namespace BookAPI.ModelView;

public class UpdateAuthorReq
{
    // public int Id { get; set; }
    public string? Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Image  { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    // public List<Book> Books { get; set; }
}