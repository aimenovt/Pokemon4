using Pokemon4.Data;
using Pokemon4.Models;

namespace Pokemon4
{
    public static class InitialValues
    {
        public static WebApplication Seed(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                using var context = scope.ServiceProvider.GetRequiredService<DataContext>();

                try
                {
                    context.Database.EnsureCreated();

                    var pokemons = context.Pokemons.ToList();

                    if (!pokemons.Any())
                    {
                        context.Categories.AddRange(

                        //1
                        new Category()
                        {
                            Name = "Fiery",
                            PokemonCategories = new List<PokemonCategory>()
                            {
                                new PokemonCategory()
                                {
                                    Pokemon = new Pokemon
                                    {
                                        Name = "Pikachu",
                                        BirthDate = new DateTime(2001,12,7),
                                        PokemonOwners = new List<PokemonOwner>()
                                        {
                                            new PokemonOwner
                                            {
                                                 Owner = new Owner
                                                 {
                                                     FirstName = "Timur",
                                                     LastName = "Aimenov",
                                                     Gym = "AAA",
                                                     Country = new Country
                                                     {
                                                         Name = "USA"
                                                     }
                                                 }
                                            }
                                        },

                                        Reviews = new List<Review>()
                                        {
                                            new Review
                                            {
                                                Title = "Wow",
                                                Text = "sasasa",
                                                Rating = 5,
                                                Reviewer = new Reviewer
                                                {
                                                    FirstName = "Alan",
                                                    LastName = "Becker"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        },

                        //2
                        new Category()
                        {
                            Name = "Aquatic",
                            PokemonCategories = new List<PokemonCategory>()
                            {
                                new PokemonCategory()
                                {
                                    Pokemon = new Pokemon
                                    {
                                        Name = "Herychu",
                                        BirthDate = new DateTime(2012,5,1),
                                        PokemonOwners = new List<PokemonOwner>()
                                        {
                                            new PokemonOwner
                                            {
                                                 Owner = new Owner
                                                 {
                                                     FirstName = "Elon",
                                                     LastName = "Musk",
                                                     Gym = "VVV",
                                                     Country = new Country
                                                     {
                                                         Name = "Canada"
                                                     }
                                                 }
                                            }
                                        },

                                        Reviews = new List<Review>()
                                        {
                                            new Review
                                            {
                                                Title = "SS",
                                                Text = "Poer",
                                                Rating = 5,
                                                Reviewer = new Reviewer
                                                {
                                                    FirstName = "Cfe",
                                                    LastName = "Vght"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        },

                        //3
                        new Category()
                        {
                            Name = "Stone",
                            PokemonCategories = new List<PokemonCategory>()
                            {
                                new PokemonCategory()
                                {
                                    Pokemon = new Pokemon
                                    {
                                        Name = "Poschu",
                                        BirthDate = new DateTime(2001,12,7),
                                        PokemonOwners = new List<PokemonOwner>()
                                        {
                                            new PokemonOwner
                                            {
                                                 Owner = new Owner
                                                 {
                                                     FirstName = "Cdfd",
                                                     LastName = "Cefew",
                                                     Gym = "DDD",
                                                     Country = new Country
                                                     {
                                                         Name = "Russia"
                                                     }
                                                 }
                                            }
                                        },

                                        Reviews = new List<Review>()
                                        {
                                            new Review
                                            {
                                                Title = "Wrr",
                                                Text = "cbghg",
                                                Rating = 5,
                                                Reviewer = new Reviewer
                                                {
                                                    FirstName = "Vvvfg",
                                                    LastName = "Yrtd"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        );

                        context.SaveChanges();
                    }
                }

                catch (Exception)
                {

                    throw;
                }

                return app;
            }
        }
    }
}
