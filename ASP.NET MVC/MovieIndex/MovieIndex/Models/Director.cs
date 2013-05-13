using System.ComponentModel.DataAnnotations;

namespace MovieIndex.Models
{
    public class Director
    {
        public int Id
        {
            get;
            set;
        }

        [Required]
        [StringLength( 100 )]
        public string FullName
        {
            get;
            set;
        }
    }
}