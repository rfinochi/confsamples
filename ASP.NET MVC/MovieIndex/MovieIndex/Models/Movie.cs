using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using MovieIndex.Validators;

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
        [StringLength(200)]
        public string Name
        {
            get;
            set;
        }

        [Required]
        [DisplayName( "Director" )]
        [StringLength( 100 )]
        public string DirectorName
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
        [DisplayFormat( DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true )]
        public DateTime ReleaseDate
        {
            get;
            set;
        }

        [Range( 0, 5 )]
        [RatingByGenre( "GenreId", 1, 2, ErrorMessage = "An Thiller movie can't be a rating minor of 2" )]
        public byte Rating
        {
            get;
            set;
        }
    }
}