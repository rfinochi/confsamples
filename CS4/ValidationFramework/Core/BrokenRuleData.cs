using System.Collections.Generic;

namespace ValidationFramework
{
	public class BrokenRuleData
	{
		#region Constructors

		public BrokenRuleData( string message, IEnumerable<string> properties )
		{
			Message = message;
			Properties = properties;
		}

		#endregion

		#region Properties

		public string Message
		{
			get;
			private set;
		}

		public IEnumerable<string> Properties
		{
			get;
			private set;
		}

		#endregion
	}
}