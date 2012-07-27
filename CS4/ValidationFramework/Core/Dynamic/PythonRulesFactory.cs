using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using IronPython.Runtime;

namespace ValidationFramework.Dynamic
{

    public class PythonRulesFactory<TEntity> : BaseRulesFactory<TEntity>
    {
        ScriptEngine _python;

        public PythonRulesFactory()
        {
            _python = Python.CreateEngine(AppDomain.CurrentDomain);
        }

        protected override IEnumerable<IRule<TEntity>> CompileRules(IEnumerable<RuleData> rules)
        {
            var result = new List<IRule<TEntity>>();
            foreach (RuleData rule in rules)
            {
                var scope = _python.CreateScope();
                _python.CreateScriptSourceFromString(
                    rule.Code,
                    Microsoft.Scripting.SourceCodeKind.Statements).Execute(scope);

                #region C# 3

                //var validateFunction = scope.GetVariable<PythonFunction>( "validate" );
                //result.Add(
                //    new StandardRule<TEntity>(
                //        e => (bool)validateFunction.Target.DynamicInvoke( e ),
                //        rule.ErrorMessage,
                //        rule.Properties.ToArray( ) ) );

                #endregion

                #region C# 4
                
                result.Add(
                    new StandardRule<TEntity>(
                        e => ((dynamic)scope).validate(e),
                        rule.ErrorMessage,
                        rule.Properties.ToArray()));
                
                #endregion
            }

            return result;
        }
    }
}