using perAPI.Models;
using Microsoft.EntityFrameworkCore;
using perAPI.Models;

namespace perAPI.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Models.Habit> Habits => Set<Models.Habit>();
    public DbSet<Models.Reflection> Reflections => Set<Models.Reflection>();
}