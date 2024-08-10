using System.ComponentModel.DataAnnotations;

public class ToDoItem
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
    public string Title { get; set; } = string.Empty;  // Initialize to avoid null reference issues
    
    [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
    public string? Description { get; set; }  // Nullable reference type
    
    public int UserId { get; set; }
    
    // Navigation property to User
    public virtual User User { get; set; } = new User();  // Initialize to avoid null reference issues
}