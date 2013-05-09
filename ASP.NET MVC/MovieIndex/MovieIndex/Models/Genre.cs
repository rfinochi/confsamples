using System.ComponentModel.DataAnnotations;

namespace MovieIndex.Models
{
    public class Genre
    {
        public int Id
        {
            get;
            set;
        }

        [Required]
        [StringLength( 100 )]
        public string Description
        {
            get;
            set;
        }
    }
}