using StudioDomain.Common.Data.Entities;

namespace StudioDomain.Command.Service.Data;

public static class DbInitializer
{

    public static void Initialize(StudioCommandContext context)
    {
        context.Database.EnsureCreated();

        SeedStudios(context);
    }

    private static void SeedStudios(StudioCommandContext context)
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
