using System.ComponentModel.DataAnnotations;

namespace MovieIndex.Validators
{
    public class RatingByGenreAttribute : ValidationAttribute
    {
        public string GenreIdProperty
        {
            get;
            set;
        }

        public int GenreId
        {
            get;
            set;
        }

        public byte MinimunRating
        {
            get;
            set;
        }

        public RatingByGenreAttribute( string property, int genreId, byte minimunRating )
        {
            this.GenreIdProperty = property;
            this.GenreId = genreId;
            this.MinimunRating = minimunRating;
        }

        public override bool IsValid( object value )
        {
            return true;
        }
    }
}