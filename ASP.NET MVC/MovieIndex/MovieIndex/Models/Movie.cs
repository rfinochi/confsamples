using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MovieIndex.Models
{
    public class Movie
    {
        public int Id
        {
            get;
            set;
        }

        [Required]
        public string Name
        {
            get;
            set;
        }

        [Required]
        public int GenreId
        {
            get;
            set;
        }

        public Genre Genre
        {
            get;
            set;
        }

        [Required]
        [DisplayName( "Release Date" )]
        [DisplayFormat( DataFormatString = "{0:mm/dd/yyyy}", ApplyFormatInEditMode = true )]
        public DateTime ReleaseDate
        {
            get;
            set;
        }

        [Range( 0, 5 )]
        public byte Rating
        {
            get;
            set;
        }
    }
}