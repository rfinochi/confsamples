using System.Linq;
using System;
using System.Collections.Generic;

namespace CodeCamp2009Demos.Models
{
    public class TalksRepository
    {
        private static List<Category> categories = new List<Category> 
        {
            new Category { Id = 1, Description = "ASP.NET MVC" },
            new Category { Id = 2, Description = "C# 4.0" },
            new Category { Id = 3, Description = "Visual Studio 2010" },
            new Category { Id = 4, Description = "Windows Azure" },
            new Category { Id = 5, Description = "JQuery" }
        };

        private static List<Talk> talks = new List<Talk> 
        { 
            new Talk 
            { 
                Id = 1, 
                Title = "Introduction to ASP.NET MVC",
                Description = "Jonh will present an introduction to the new ASP.NET MVC Framework.",
                Category = categories[0],
                Speaker = "Jonh"
            },

            new Talk 
            { 
                Id = 2, 
                Title = "Dynamics with C#", 
                Description = "In this presentation we will explore the dynamics features in C# 4.0.", 
                Category = categories[1],
                Speaker = "George"
            },

            new Talk 
            { 
                Id = 3, 
                Title = "Introduction to C# 4.0", 
                Description = "Paul will provide an introduction to the new features of C# 4.0.",
                Category = categories[1],
                Speaker = "Paul"
            },

            new Talk 
            { 
                Id = 4, 
                Title = "Cloud Computing and Microsoft", 
                Description = "Windows Azure is the new Microsoft's Cloud Computing platform.",
                Category = categories[3],
                Speaker = "Ringo"
            },

            new Talk 
            { 
                Id = 5, 
                Title = "Using Visual Studio 2010",
                Description = "All about the new IDE.",
                Category = categories[2],
                Speaker = "Charles"
            },

            new Talk 
            { 
                Id = 5, 
                Title = "Pimp my Applicacion with JQuery",
                Description = "Jquery is a new Javascript famework, Paul will show us how to use it.",
                Category = categories[4],
                Speaker = "Paul"
            },
        };

        public static Category[] GetCategories()
        {
            return categories.ToArray();
        }

        public static Category GetCategoryById(int categoryId)
        {
            return categories.FirstOrDefault(x => x.Id == categoryId);
        }

        public static Talk[] GetTalks()
        {
            return talks.ToArray();
        }

        public static Talk[] GetTalksByCategoryId(int categoryId)
        {
            return talks.Where(x => x.Category.Id == categoryId).ToArray();
        }

        public static Talk GetTalkByTitle(string title)
        { 
            return talks.FirstOrDefault(x => x.Title.Equals(title, StringComparison.InvariantCultureIgnoreCase));
        }

        public static void Add(Talk talk)
        {
            talk.Id = talks.Last().Id + 1;

            talks.Add(talk);
        }
    }
}
