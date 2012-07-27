using System;
using System.Collections.Generic;

namespace ValidationFramework.Dynamic
{
	public abstract class BaseRulesFactory<TEntity> : IRulesFactory<TEntity>
	{
		#region Properties

		private Dictionary<string, List<RuleData>> _rules = new Dictionary<string, List<RuleData>>( );

		protected Dictionary<string, List<RuleData>> Rules
		{
			get
			{
				return _rules;
			}
		}

		#endregion

		#region Public Methods

		public void AddRule( RuleType type, string name, string code, string errorMessage, params string[] properties )
		{
			AddRule( type, new RuleData( name, code, errorMessage, properties ) );
		}

		public void AddRule( RuleType type, string code, string errorMessage, params string[] properties )
		{
			AddRule( type, new RuleData( code, errorMessage, properties ) );
		}

		public void AddRule( RuleType type, RuleData rule )
		{
			string key = typeof( TEntity ).FullName + type;

			if ( _rules.ContainsKey( key ) )
				_rules[ key ].Add( rule );
			else
				_rules.Add( key, new List<RuleData> { rule } );
		}
		
		public IEnumerable<IRule<TEntity>> FindRules( RuleType type )
		{
			if ( type == RuleType.All )
				throw new ApplicationException( "Only 'Persistence' or 'Business' rule types are valid to search" );
			else
				return InternalFindRules( type );
		}

		#endregion

		#region Private Methods

		private IEnumerable<IRule<TEntity>> InternalFindRules( RuleType type )
		{
			List<IRule<TEntity>> rules = new List<IRule<TEntity>>( );

			if ( type != RuleType.All )
				rules.AddRange( InternalFindRules( RuleType.All ) );

			string key = typeof( TEntity ).FullName + type;

			if ( _rules.ContainsKey( key ) && _rules[ key ].Count > 0 )
				rules.AddRange( CompileRules( _rules[ key ] ) );

			return rules;
		}

		#endregion

		#region Abstract Methods

		protected abstract IEnumerable<IRule<TEntity>> CompileRules( IEnumerable<RuleData> rules );

		#endregion
	}
}