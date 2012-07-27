using System.Collections.Generic;

namespace ValidationFramework
{
	public interface IRule<T>
	{
		#region Properties

		string ErrorMessage
		{
			get;
		}
		
		IEnumerable<string> Properties
		{
			get;
		}

		#endregion

		#region Methods

		bool Passes( T entity );

		#endregion
	}
}