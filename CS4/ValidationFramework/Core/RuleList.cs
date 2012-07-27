using System;
using System.Collections.Generic;

namespace ValidationFramework
{
	public class RuleList<TEntity>
	{
		#region Private Vars

		private List<IRule<TEntity>> _rules = new List<IRule<TEntity>>( );

		#endregion

		#region Public Methods

		public void Add( IRule<TEntity> rule )
		{
			if ( rule == null )
				throw new ArgumentNullException( "rule" );

			_rules.Add( rule );
		}

		public void Add( IEnumerable<IRule<TEntity>> rules )
		{
			if ( rules == null )
				throw new ArgumentNullException( "rules" );

			foreach ( IRule<TEntity> rule in rules )
				_rules.Add( rule );
		}

		public bool Passes( TEntity entity )
		{
			if ( entity == null )
				throw new ArgumentNullException( "entity" );

			foreach ( IRule<TEntity> rule in _rules )
			{
				if ( !rule.Passes( entity ) )
					return false;
			}

			return true;
		}

		public IEnumerable<BrokenRuleData> GetFailedRules( TEntity entity )
		{
			List<BrokenRuleData> result = new List<BrokenRuleData>( );
			
			foreach ( IRule<TEntity> rule in _rules )
			{
				if ( !rule.Passes( entity ) )
					result.Add( new BrokenRuleData( rule.ErrorMessage, rule.Properties ) );
			}

			return result;
		}

		#endregion
	}
}