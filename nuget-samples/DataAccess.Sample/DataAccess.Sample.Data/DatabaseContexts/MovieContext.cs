using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DataAccess.Sample.Data.DatabaseContexts;

public class MovieContext: DbContext
{
    public MovieContext(DbContextOptions options): base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var assembly = Assembly.GetAssembly(typeof(MovieContext));

        if (assembly == null)
        {
            throw new Exception($"Failed to get assembly for {nameof(MovieContext)}");
        }
        
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
    }
}