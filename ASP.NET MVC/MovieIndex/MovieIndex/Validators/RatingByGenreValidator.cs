using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieIndex.Validators
{
    public class RatingByGenreValidator : DataAnnotationsModelValidator
    {
        public RatingByGenreValidator( ModelMetadata metadata, ControllerContext context, ValidationAttribute attribute ) : base( metadata, context, attribute ) { }

        public override IEnumerable<ModelValidationResult> Validate( object container )
        {
            var dependentField = Metadata.ContainerType.GetProperty( ( (RatingByGenreAttribute)Attribute ).GenreIdProperty );
            int genreId = ( (RatingByGenreAttribute)Attribute ).GenreId;
            byte minimunRating = ( (RatingByGenreAttribute)Attribute ).MinimunRating;
            var field = Metadata.ContainerType.GetProperty( this.Metadata.PropertyName );

            if ( dependentField != null && field != null )
            {
                object dependentValue = dependentField.GetValue( container, null );
                object value = field.GetValue( container, null );
                if ( ( dependentValue != null && dependentValue.Equals( genreId ) && (byte)value < minimunRating ) )
                    if ( !Attribute.IsValid( this.Metadata.Model ) )
                        yield return new ModelValidationResult { Message = ErrorMessage };
                    else
                        yield return new ModelValidationResult { Message = ErrorMessage };
            }
        }
    }
}