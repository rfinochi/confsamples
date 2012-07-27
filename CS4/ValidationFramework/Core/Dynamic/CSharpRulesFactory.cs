using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.CSharp;

namespace ValidationFramework.Dynamic
{
	public class CSharpRulesFactory<TEntity> : BaseRulesFactory<TEntity>
	{
		#region BaseRulesFactory Members

		protected override IEnumerable<IRule<TEntity>> CompileRules( IEnumerable<RuleData> rules )
		{
			List<IRule<TEntity>> result = new List<IRule<TEntity>>( );

			Type entityType = typeof( TEntity );

			var provider = new CSharpCodeProvider( );
			var cp = new CompilerParameters( );
			cp.GenerateExecutable = false;
			cp.GenerateInMemory = true;
			cp.ReferencedAssemblies.Add( "system.dll" );
			cp.ReferencedAssemblies.Add( Assembly.GetExecutingAssembly( ).ManifestModule.Name );
			cp.ReferencedAssemblies.Add( entityType.Assembly.ManifestModule.Name );

			string className = entityType.FullName.Replace( '.', '_' ) + "Rules";
			string unitNamespace = "BusinessRules";

			var businessRulesNamespace = new CodeNamespace( unitNamespace );
			businessRulesNamespace.Imports.Add( new CodeNamespaceImport( "System" ) );
			businessRulesNamespace.Imports.Add( new CodeNamespaceImport( "System.Collections.Generic" ) );

			var unit = new CodeCompileUnit( );
			unit.Namespaces.Add( businessRulesNamespace );

			var businessRuleClass = new CodeTypeDeclaration( className );

			int i = 0;
			foreach ( RuleData rule in rules )
			{
				i++;

				string source;

				if ( rule.Code.IndexOf("return") == -1)
					source = String.Format( CultureInfo.InvariantCulture, @"public bool Execute{0}({1} e){{return ({2});}}", i, entityType.FullName, rule.Code );
				else
					source = String.Format( CultureInfo.InvariantCulture, @"public bool Execute{0}({1} e){{{2}}}", i, entityType.FullName, rule.Code );

				businessRuleClass.Members.Add( new CodeSnippetTypeMember( source ) );
			}

			businessRulesNamespace.Types.Add( businessRuleClass );

			CompilerResults cr = provider.CompileAssemblyFromDom( cp, unit );
			if ( cr.Errors.Count > 0 )
			{
				StringBuilder sb = new StringBuilder( );
				foreach ( CompilerError error in cr.Errors )
				{
					sb.Append( Environment.NewLine );
					sb.Append( "'" );
					sb.Append( String.Format( CultureInfo.InvariantCulture, "{0} - {1}", error.ErrorNumber, error.ErrorText ) );
					sb.Append( "'" );
				}

				throw new ApplicationException( String.Format( CultureInfo.InvariantCulture, "Building rule has failed with {0} errors. Errors:{1}", cr.Errors.Count, sb.ToString( ) ) );
			}

			Type compiledType = cr.CompiledAssembly.GetType( unitNamespace + "." + className );
			if ( compiledType != null )
			{
				object myobj = Activator.CreateInstance( compiledType );
				i = 0;
				foreach ( RuleData rule in rules )
				{
					i++;
					MethodInfo mi = compiledType.GetMethod( "Execute" + i );
					Predicate<TEntity> predicate = p => (bool)mi.Invoke( myobj, new object[] { p } );
					result.Add( new StandardRule<TEntity>( predicate, rule.ErrorMessage, rule.Properties.ToArray( ) ) );
				}
			}
		
			return result;
		}

		#endregion
	}
}