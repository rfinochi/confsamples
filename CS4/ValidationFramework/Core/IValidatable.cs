using System.Collections.Generic;

namespace ValidationFramework
{
	public interface IValidatable<TEntity>
	{
		#region Properties

		RuleList<TEntity> PersistenceRules
		{
			get;
		}

		RuleList<TEntity> BusinessRules
		{
			get;
		}

		#endregion

		#region Methods

		bool IsPersistable( );
		
		bool IsValid( );
		
		IEnumerable<BrokenRuleData> GetBrokenBusinessRules( );

		IEnumerable<BrokenRuleData> GetBrokenPersistenceRules( );

		IEnumerable<BrokenRuleData> GetAllBrokenRules( );

		#endregion
	}
}