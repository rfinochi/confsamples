using System.Collections.Generic;

namespace ValidationFramework.Dynamic
{
	public interface IRulesFactory<TEntity>
	{
		#region Public Methods

		void AddRule( RuleType type, string rule, string errorMessage, params string[] properties );

		void AddRule( RuleType type, RuleData rule );

		IEnumerable<IRule<TEntity>> FindRules( RuleType type );

		#endregion
	}
}