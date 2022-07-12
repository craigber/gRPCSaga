using CartoonDomain.Service.Data.Entities;

namespace CartoonDomain.Service.Data
{
    public static class DbInitializer
    {
        public static void Initialize(CartoonQueryContext context)
        {
            context.Database.EnsureCreated();

            if (context.Cartoons.Any())
            {
                return;
            }

            var cartoons = new Cartoon[]
            {
                new Cartoon
                {
                    Id = 1,
                    Title = "Rocky and Friends",
                    YearBegin = 1959,
                    YearEnd = 1963,
                    Description = "The adventures of Moose and Squirrel as they fight the bad guys from Potsylvania.",
                    Rating = 5.0m
                },
                new Cartoon
                {
                    Id = 2,
                    Title = "The Simpsons",
                    YearBegin = 1989,
                    Description = "Just your typical American family.",
                    Rating = 5.0m
                }
            };
            context.SaveChanges();
        }
    }
}
