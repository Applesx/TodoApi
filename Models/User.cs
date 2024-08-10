using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class User
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    public string Name { get; set; } = string.Empty;  // Initialize to avoid null reference issues
    
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [StringLength(256, ErrorMessage = "Email cannot be longer than 256 characters.")]
    public string Email { get; set; } = string.Empty;  // Initialize to avoid null reference issues
    
    [Required(ErrorMessage = "Password hash is required.")]
    public string PasswordHash { get; set; } = string.Empty;  // Initialize to avoid null reference issues
    
    [Required(ErrorMessage = "Password salt is required.")]
    public string PasswordSalt { get; set; } = string.Empty;  // Initialize to avoid null reference issues

    // Navigation property for related ToDoItems
    public virtual ICollection<ToDoItem> ToDoItems { get; set; } = new List<ToDoItem>();  // Initialize to avoid null reference issues
}