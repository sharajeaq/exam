using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using perAPI.Data;
using perAPI.Data.Reflection;
using perAPI.Dto.Reflection;
using perAPI.Models;

namespace perAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReflectionsController(DataContext context) : ControllerBase
{
    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<ActionResult<ReflectionDto>> Crear(ReflectionCreateDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var habit = await context.Habits.FirstOrDefaultAsync(h => h.Id == dto.HabitId && h.UserId == userId);
        if (habit == null) return BadRequest("Hábito no encontrado o no te pertenece.");

        var reflection = new Reflection
        {
            Content = dto.Content,
            HabitId = dto.HabitId,
            UserId = userId,
            Date = DateTime.UtcNow
        };

        context.Reflections.Add(reflection);
        await context.SaveChangesAsync();

        return new ReflectionDto
        {
            Content = reflection.Content,
            Date = reflection.Date,
            HabitId = reflection.HabitId,
            HabitName = habit.Name
        };
    }

    [Authorize(Roles = "User")]
    [HttpGet("mias")]
    public async Task<ActionResult<List<ReflectionDto>>> VerMias()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var data = await context.Reflections
            .Where(r => r.UserId == userId)
            .Include(r => r.Habit)
            .Select(r => new ReflectionDto
            {
                Content = r.Content,
                Date = r.Date,
                HabitId = r.HabitId,
                HabitName = r.Habit.Name
            }).ToListAsync();

        return Ok(data);
    }

    [Authorize(Roles = "Coach")]
    [HttpGet("asignado/{userId}")]
    public async Task<ActionResult<List<ReflectionDto>>> VerDeAsignado(int userId)
    {
        var coachId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var usuario = await context.Users.FirstOrDefaultAsync(u => u.Id == userId && u.CoachId == coachId);

        if (usuario == null) return Forbid();

        var reflexiones = await context.Reflections
            .Where(r => r.UserId == userId)
            .Include(r => r.Habit)
            .Select(r => new ReflectionDto
            {
                Content = r.Content,
                Date = r.Date,
                HabitId = r.HabitId,
                HabitName = r.Habit.Name
            }).ToListAsync();

        return Ok(reflexiones);
    }
}