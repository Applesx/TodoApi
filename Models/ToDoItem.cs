﻿using System.ComponentModel.DataAnnotations;

public class ToDoItem
{
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
}