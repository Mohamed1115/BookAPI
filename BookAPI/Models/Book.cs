namespace BookAPI.Models;

public class Book
{
    public int Id { get; set; }
    public int? AuthorId { get; set; } 
    public Author Author { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int Price { get; set; }
    public float Rating { get; set; }
    public int Quantity { get; set; }
    public int ReviewsCount { get; set; }
    public int YearPublished { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public List<Comment> Comments { get; set; }
}