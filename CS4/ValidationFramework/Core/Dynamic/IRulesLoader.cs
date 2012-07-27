namespace ValidationFramework.Dynamic
{
	public interface IRulesLoader<TEntity, TRulesFactory>
		where TRulesFactory : IRulesFactory<TEntity>
	{
		#region Properties

		TRulesFactory RulesFactory
		{
			get;
		}
		
		#endregion
		
		#region Public Methods

		void AddRules( string fileOrServerOrConnection );

		#endregion
	}
}