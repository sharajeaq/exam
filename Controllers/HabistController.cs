using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using perAPI.Data;
using perAPI.Dto.Habit;
using perAPI.Models;

namespace perAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HabitsController : ControllerBase
{
    private readonly DataContext _context;

    public HabitsController(DataContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<ActionResult<HabitDto>> Crear(HabitCreateDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        var habit = new Habit
        {
            Name = dto.Name,
            UserId = userId
        };

        _context.Habits.Add(habit);
        await _context.SaveChangesAsync();

        return new HabitDto
        {
            Id = habit.Id,
            Name = habit.Name,
            CreatedAt = habit.CreatedAt
        };
    }

    [Authorize(Roles = "User")]
    [HttpGet("mios")]
    public async Task<ActionResult<List<HabitDto>>> VerMios()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var data = await _context.Habits
            .Where(h => h.UserId == userId)
            .Select(h => new HabitDto
            {
                Id = h.Id,
                Name = h.Name,
                CreatedAt = h.CreatedAt
            }).ToListAsync();

        return Ok(data);
    }

    [Authorize(Roles = "User")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Editar(int id, HabitCreateDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var habit = await _context.Habits.FirstOrDefaultAsync(h => h.Id == id && h.UserId == userId);

        if (habit == null) return NotFound();

        habit.Name = dto.Name;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize(Roles = "User")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Eliminar(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var habit = await _context.Habits.FirstOrDefaultAsync(h => h.Id == id && h.UserId == userId);

        if (habit == null) return NotFound();

        _context.Habits.Remove(habit);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize(Roles = "Coach")]
    [HttpGet("asignado/{userId}")]
    public async Task<ActionResult<List<HabitDto>>> VerDeAsignado(int userId)
    {
        var coachId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var usuario = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId && u.CoachId == coachId);

        if (usuario == null) return Forbid();

        var habits = await _context.Habits
            .Where(h => h.UserId == userId)
            .Select(h => new HabitDto
            {
                Id = h.Id,
                Name = h.Name,
                CreatedAt = h.CreatedAt
            }).ToListAsync<object>();

        return Ok(habits);
    }
}