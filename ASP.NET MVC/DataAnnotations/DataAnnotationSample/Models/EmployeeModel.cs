using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using DataAnnotationSample.Attributes;

namespace DataAnnotationSample.Models
{
    public class EmployeeModel
    {
        [Required]
        [DisplayName( "Employee Id" )]
        public int Id { get; set; }

        [Required]
        [DataType( DataType.Text )]
        [DisplayName( "Name" )]
        public string Name { get; set; }

        [Required]
        [DataType( DataType.Text )]
        [DisplayName( "Address" )]
        public string Address { get; set; }

        [Required]
        [DataType( DataType.PhoneNumber )]
        [DisplayName( "Phone Number" )]
        public int PhoneNumber { get; set; }

        [Required]
        [DataType( DataType.EmailAddress )]
        [DisplayName( "Email" )]
        public string Email { get; set; }

        [Required]
        [DataType( DataType.EmailAddress )]
        [SameAs( "Email", ErrorMessage = "It should be similar to Email" )]
        [DisplayName( "Confirm Email" )]
        public string ConfirmEmail { get; set; }

        [Required]
        [DataType( DataType.Password )]
        [DisplayName( "Password" )]
        public string Password { get; set; }

        [Required]
        [DataType( DataType.Password )]
        [DisplayName( "Confirm Password" )]
        [SameAs( "Password", ErrorMessage = "It should be similar to Password" )]
        public string ConfirmPassword { get; set; }
    }
}