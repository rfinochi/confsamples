using System;
using System.Collections.Generic;
using System.Linq;

using IronRuby.Builtins;

using Microsoft.Scripting.Hosting;

namespace ValidationFramework.Dynamic
{
	public class RubyRulesFactory<TEntity> : BaseRulesFactory<TEntity>
	{
		#region Private Vars

		private ScriptEngine _scriptEngine;

		#endregion

		#region Constructors

		public RubyRulesFactory( )
		{
			var setup = new ScriptRuntimeSetup( );
			setup.LanguageSetups.Add(
				new LanguageSetup(
					"IronRuby.Runtime.RubyContext, IronRuby",
					"IronRuby 1.0",
					new[] { "IronRuby", "Ruby", "rb" },
					new[] { ".rb" } ) );
			var runtime = ScriptRuntime.CreateRemote( AppDomain.CurrentDomain, setup );
			_scriptEngine = runtime.GetEngine( "Ruby" );
		}

		#endregion

		#region BaseRulesFactory Members

		protected override IEnumerable<IRule<TEntity>> CompileRules( IEnumerable<RuleData> rules )
		{
			List<IRule<TEntity>> result = new List<IRule<TEntity>>( );

			foreach ( RuleData rule in rules )
			{
				ScriptSource scriptSource = _scriptEngine.CreateScriptSourceFromString( String.Format( "Proc.new {{ |e| {0} }}", rule.Code ) );
				
				var proc = (Proc)scriptSource.Execute( );
				Predicate<TEntity> predicate = ( e => (bool)proc.Call( e ) );

				result.Add( new StandardRule<TEntity>( predicate, rule.ErrorMessage, rule.Properties.ToArray( ) ) );
			}

			return result;
		}

		#endregion
	}
}