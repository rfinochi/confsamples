using System.Collections.Generic;

namespace ValidationFramework
{
	public abstract class BaseRule<T> : IRule<T>
	{
		#region Constructors

		public BaseRule( string errorMessage, params string[] properties )
		{
			_errorMessage = errorMessage;
			_properties = properties;
		}

		#endregion

		#region Properties

		private readonly string[] _properties;

		public IEnumerable<string> Properties
		{
			get
			{
				return _properties;
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


		#endregion

		#region Abstract Methods

		public abstract bool Passes( T entity );

		#endregion
	}
}