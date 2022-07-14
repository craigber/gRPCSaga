using StudioDomain.Service.Data.Entities;

namespace StudioDomain.Service.Data;

public static class DbInitializer
{

    public static void Initialize(StudioQueryContext context)
    {
        context.Database.EnsureCreated();

        SeedStudios(context);
    }

    private static void SeedStudios(StudioQueryContext context)
    {
        if (context.Studios.Any())
        {
            return;
        }

        var studios = new Studio[]
        {
            new Studio
            {
                Name = "Jay Ward Productions"
            },
            new Studio
            {
                Name = "20th Television Animation"
            },
            new Studio
            {
                Name = "Hanna-Barbera"
            },
            new Studio
            {
                Name = "Depatie-Freleng Enterprises"
            }
        };
        foreach (var studio in studios)
        {
            context.Studios.Add(studio);
        }
        context.SaveChanges();
    }
}
