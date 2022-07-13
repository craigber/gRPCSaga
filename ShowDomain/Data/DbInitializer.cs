using CartoonDomain.Service.Data.Entities;

namespace CartoonDomain.Service.Data;

public static class DbInitializer
{

    public static void Initialize(CartoonQueryContext context)
    {
        context.Database.EnsureCreated();

        SeedCartoons(context);
        SeedCharacters(context);
    }

    private static void SeedCharacters(CartoonQueryContext context)
    {
        if (context.Characters.Any())
        {
            return;
        }

        var characters = new Character[]
        {
            new Character
            {
                Name = "Rocky",
                Description = "A plucky hero squirrel",
                CartoonId = 1
            },
            new Character
            {
                Name = "Bullwinke J. Moose",
                Description = "A somewhat-dimwitted moose but still one of our heros",
                CartoonId = 1
            },
            new Character
            {
                Name = "Boris Badanov",
                Description = "Sneaky bad guy, often in disguise",
                CartoonId = 1
            },
            new Character
            {
                Name = "Natasha Fatale",
                Description = "Tall, secretive sidekick to Boris",
                CartoonId = 1
            },
            new Character
            {
                Name = "Homer Simpson",
                Description = "All-American dad",
                CartoonId = 2
            },
            new Character
            {
                Name = "Marge Simpson",
                Description = "Wife and mother, who somehow makes it all work",
                CartoonId = 2
            },
            new Character
            {
                Name = "Bart Simpson",
                Description = "Best quote: I didn't think it possible, but this both sucks and blows at the same time",
                CartoonId = 2
            },
            new Character
            {
                Name = "Lisa Simpson",
                Description = "Smart and saxaphone playing daughter",
                CartoonId = 2
            },
            new Character
            {
                Name = "Maggie Simpson",
                Description = "Oh, look at the cute baby",
                CartoonId = 2
            },
            new Character
            {
                Name = "Fred Flintstone",
                Description = "The leader and chief trouble-maker",
                CartoonId = 3
            },
            new Character
            {
                Name = "Wilma Flintstone",
                Description = "How does she put up with Fred's shenanigans?",
                CartoonId = 3
            },
            new Character
            {
                Name = "Barney Rubble",
                Description = "Neighbor and assistant trouble-maker",
                CartoonId = 3
            },
            new Character
            {
                Name = "Betty Rubble",
                Description = "Neighbor and Wilma's best friend",
                CartoonId = 3
            },
            new Character
            {
                Name = "George Jetson",
                Description = "All American dad of the future",
                CartoonId = 4
            },
            new Character
            {
                Name = "Jane Jetson",
                Description = "Everyone's favorite cartoon mom",
                CartoonId = 4
            },
            new Character
            {
                Name = "Judy Jetson",
                Description = "Daughter on the teen-set",
                CartoonId = 4
            },
            new Character
            {
                Name = "Elroy Jetson",
                Description = "Thinks his dad is the greatest",
                CartoonId = 4
            },
            new Character
            {
                Name = "Astro",
                Description = "The Jetson family dog",
                CartoonId = 4
            },
            new Character
            {
                Name = "Pink Panther",
                Description = "Never talking star",
                CartoonId = 5
            },
            new Character
            {
                Name = "Inspector Clouseau",
                Description = "Bumbling detective",
                CartoonId = 5
            }
        };
        foreach (var character in characters)
        {
            context.Characters.Add(character);
        }
        context.SaveChanges();
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
