using System;
using System.Collections.Generic;

namespace ValidationFramework.Dynamic
{
	public class RuleData
	{
		#region Constructors

		public RuleData( string code, string errorMessage, params string[] properties ) : this( String.Empty, code, errorMessage, properties ) { }

		public RuleData( string name, string code, string errorMessage, params string[] properties )
		{
			if ( String.IsNullOrEmpty( code ) )
				throw new ArgumentNullException( "code" );
			else
				_code = code;

			if ( String.IsNullOrEmpty( name ) )
				_name = name.GetHashCode( ).ToString( );
			else
				_code = code;

			if ( String.IsNullOrEmpty( errorMessage ) )
				throw new ArgumentNullException( "errorMessage" );
			else
				_errorMessage = errorMessage;

			_properties = properties;
		}

		#endregion

		#region Properties

		private readonly string _name;

		public string Name
		{
			get
			{
				return _name;
			}
		}

		private readonly string _code;

		public string Code
		{
			get
			{
				return _code; 
			}
		}

		private readonly string _errorMessage;

		public string ErrorMessage
		{
			get
			{
				return _errorMessage;
			}
		}

		private readonly string[] _properties;

		public IEnumerable<string> Properties
		{
			get
			{
				return _properties;
			}
		}

		#endregion
	}
}