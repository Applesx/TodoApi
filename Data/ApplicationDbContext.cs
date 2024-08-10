using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public  ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; init; }
    public DbSet<ToDoItem> ToDoItems { get; init; }
}