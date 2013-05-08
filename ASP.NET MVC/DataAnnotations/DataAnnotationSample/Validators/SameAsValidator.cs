using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using DataAnnotationSample.Attributes;

namespace DataAnnotationSample.Validators
{
    public class SameAsValidator : DataAnnotationsModelValidator
    {
        public SameAsValidator( ModelMetadata metadata,
                                ControllerContext context,
                                ValidationAttribute attribute )
            : base( metadata, context, attribute ) { }

        public override IEnumerable<ModelValidationResult> Validate( object container )
        {
            var dependentField = Metadata.ContainerType.GetProperty( ( (SameAsAttribute)Attribute ).Property );
            var field = Metadata.ContainerType.GetProperty( this.Metadata.PropertyName );
            if ( dependentField != null && field != null )
            {
                object dependentValue = dependentField.GetValue( container, null );
                object value = field.GetValue( container, null );
                if ( ( dependentValue != null && dependentValue.Equals( value ) ) )
                    if ( !Attribute.IsValid( this.Metadata.Model ) )
                        yield return new ModelValidationResult { Message = ErrorMessage };
                else
                    yield return new ModelValidationResult { Message = ErrorMessage };
            }
        }
    }
}