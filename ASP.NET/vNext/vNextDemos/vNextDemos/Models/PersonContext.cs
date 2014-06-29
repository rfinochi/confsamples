using Microsoft.Data.Entity;
using System;

namespace vNextDemos
{
    /// <summary>
    /// Summary description for PersonContext
    /// </summary>
    public class PersonContext : DbContext
    {
	    public PersonContext()
	    {
		    //
		    // TODO: Add constructor logic here
		    //
	    }

        public DbSet<Person> People { get; set; }
    }

    public class Person
    {

        public int Id { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public DateTime Birthdate { get; set; }
        public int Height { get; set; }

    }
}