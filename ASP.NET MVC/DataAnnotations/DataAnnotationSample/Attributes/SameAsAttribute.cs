using System.ComponentModel.DataAnnotations;

namespace DataAnnotationSample.Attributes
{
    public class SameAsAttribute : ValidationAttribute
    {
        public string Property { get; set; }

        public SameAsAttribute( string Property )
        {
            this.Property = Property;
        }

        public override bool IsValid( object value )
        {
            return true;
        }
    }
}