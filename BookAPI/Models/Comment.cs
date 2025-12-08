using BookAPI.Data;

namespace BookAPI.Models;

public class Comment
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public string Author { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public float Rating { get; set; }
    public int BookId { get; set; }
    
}