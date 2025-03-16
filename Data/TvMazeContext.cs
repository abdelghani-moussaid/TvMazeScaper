using Microsoft.EntityFrameworkCore;

namespace TvMazeScaper.Data;

public class TvMazeContext : DbContext
{
    public TvMazeContext(DbContextOptions<TvMazeContext> options) : base(options) { }

    public TvMazeContext() : base() { }

    public DbSet<Show> Show => Set<Show>();
    public DbSet<Person> Person => Set<Person>();
    public DbSet<Cast> Cast => Set<Cast>();
}