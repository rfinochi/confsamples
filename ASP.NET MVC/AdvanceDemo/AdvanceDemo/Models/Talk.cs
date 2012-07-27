using System;

namespace CodeCamp2009Demos.Models
{
    public class Talk
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Category Category { get; set; }

        public string Speaker { get; set; }
    }
}
