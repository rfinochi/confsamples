using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxVisitors.Demo
{
    class CatchSyntaxWalker : CSharpSyntaxWalker
    {

        public override void VisitCatchClause(CatchClauseSyntax node)
        {
            if (!node.Block.Statements.Any())
            {
                var method = node.Ancestors()
                        .OfType<MethodDeclarationSyntax>().First();
                Console.WriteLine("Empty catch block in method : {0}", method.Identifier.Text);
            }
        }
    }
}
