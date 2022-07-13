using CartoonDomain.Service.Data.Entities;

namespace CartoonDomain.Service.Data;

public static class DbInitializer
{

    public static void Initialize(CartoonQueryContext context)
    {
        context.Database.EnsureCreated();

        SeedCartoons(context);
    }

    private static void SeedCartoons(CartoonQueryContext context)
    {
        if (context.Cartoons.Any())
        {
            return;
        }

        var cartoons = new Cartoon[]
        {
            new Cartoon
            {
                //Id = 1,
                Title = "Rocky and Friends",
                YearBegin = 1959,
                YearEnd = 1963,
                Description = "The adventures of Moose and Squirrel as they fight the bad guys from Potsylvania.",
                Rating = 5.0m
            },
            new Cartoon
            {
                //Id = 2,
                Title = "The Simpsons",
                YearBegin = 1989,
                Description = "Just your typical American family.",
                Rating = 5.0m
            },
            new Cartoon
            {
                //Id = 3,
                Title = "The Flintstones",
                YearBegin = 1960,
                YearEnd = 1966,
                Description = "The Honeymooners return as cartoons, but in the stoneage",
                Rating = 4.0m
            },
            new Cartoon
            {
                //Id = 4,
                Title = "The Jetsons",
                YearBegin = 1962,
                YearEnd = 1963,
                Description = "Life in the future.",
                Rating = 3.0m
            },
            new Cartoon
            {
                //Id = 5,
                Title = "The Pink Panther",
                YearBegin = 1969,
                YearEnd = 2011,
                Description = "A bumbling detective tries to catch a panther that happens to be pink. Perhaps the panther sold Mary Kay cosmetics.",
                Rating = 3.0m
            }
        };

        foreach (var cartoon in cartoons)
        {
            context.Cartoons.Add(cartoon);
        }
        context.SaveChanges();
    }
}
