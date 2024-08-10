using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ToDoItemsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ToDoItemsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ToDoItem item)
    {
        var userId = GetUserId();
        item.UserId = userId;

        _context.ToDoItems.Add(item);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ToDoItem item)
    {
        var userId = GetUserId();
        var existingItem = await _context.ToDoItems.FindAsync(id);

        if (existingItem == null || existingItem.UserId != userId)
        {
            return Forbid();
        }

        existingItem.Title = item.Title;
        existingItem.Description = item.Description;

        _context.ToDoItems.Update(existingItem);
        await _context.SaveChangesAsync();

        return Ok(existingItem);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();
        var item = await _context.ToDoItems.FindAsync(id);

        if (item == null || item.UserId != userId)
        {
            return Forbid();
        }

        _context.ToDoItems.Remove(item);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int limit = 10)
    {
        var userId = GetUserId();
        var query = _context.ToDoItems.Where(t => t.UserId == userId);

        var total = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        return Ok(new
        {
            Data = items,
            Page = page,
            Limit = limit,
            Total = total
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = GetUserId();
        var item = await _context.ToDoItems.FindAsync(id);

        if (item == null || item.UserId != userId)
        {
            return NotFound();
        }

        return Ok(item);
    }

    private int GetUserId()
    {
        var userId = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userId);
    }
    
    [HttpGet("todos")]
    public async Task<IActionResult> GetToDoItems([FromQuery] string filter = null, [FromQuery] string sort = null)
    {
        var query = _context.ToDoItems.AsQueryable();

        if (!string.IsNullOrEmpty(filter))
        {
            query = query.Where(t => t.Title.Contains(filter) || t.Description.Contains(filter));
        }

        if (sort == "asc")
        {
            query = query.OrderBy(t => t.Title);
        }
        else if (sort == "desc")
        {
            query = query.OrderByDescending(t => t.Title);
        }

        var todos = await query.ToListAsync();
        return Ok(todos);
    }
}
