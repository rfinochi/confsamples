using System.Collections.Generic;


namespace ValidationFramework.UnitTests
{
	public class BusinessEntity : IValidatable<BusinessEntity>
	{
		#region Properties

		public int Value
		{
			get;
			set;
		}

		public int Value2
		{
			get;
			set;
		}

		public int Value3
		{
			get;
			set;
		}

		public int Value4
		{
			get;
			set;
		}

		public int Value5
		{
			get;
			set;
		}
	
		#endregion

		#region IValidatable Members

		private RuleList<BusinessEntity> _persistenceRules = new RuleList<BusinessEntity>( );

		public RuleList<BusinessEntity> PersistenceRules
		{
			get
			{
				return _persistenceRules;
			}
		}

		private RuleList<BusinessEntity> _businessRules = new RuleList<BusinessEntity>( );

		public RuleList<BusinessEntity> BusinessRules
		{
			get
			{
				return _businessRules;
			}
		}

		public bool IsPersistable( )
		{
			return PersistenceRules.Passes( this );
		}

		public bool IsValid( )
		{
			if ( !PersistenceRules.Passes( this ) )
				return false;

			return BusinessRules.Passes( this );
		}

		public IEnumerable<BrokenRuleData> GetBrokenBusinessRules( )
		{
			return _businessRules.GetFailedRules( this );
		}

		public IEnumerable<BrokenRuleData> GetBrokenPersistenceRules( )
		{
			return _persistenceRules.GetFailedRules( this );
		}

		public IEnumerable<BrokenRuleData> GetAllBrokenRules( )
		{
			var result = new List<BrokenRuleData>( );
			
			result.AddRange( GetBrokenPersistenceRules( ) );
			result.AddRange( GetBrokenBusinessRules( ) );
			
			return result;
		}  

		#endregion
	}
}