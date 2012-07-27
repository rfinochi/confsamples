using System;

namespace ValidationFramework
{
	public class StandardRule<T> : BaseRule<T>
	{
		#region Private Vars

		private readonly Predicate<T> _predicate;

		#endregion

		#region Constructors

		public StandardRule( Predicate<T> predicate, string errorMessage, params string[] properties ) : base( errorMessage, properties )
		{
			_predicate = predicate;
		}

		#endregion

		#region Rule Members

		public override bool Passes( T entity )
		{
			if ( entity == null )
				throw new ArgumentNullException( "entity" );

			return _predicate( entity );
		}

		#endregion
	}
}