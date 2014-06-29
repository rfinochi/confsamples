using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

namespace SyntaxVisitors.Demo
{
    class CatchSyntaxRewriter : CSharpSyntaxRewriter
    {

        public override SyntaxNode VisitCatchClause(CatchClauseSyntax node)
        {
            if (!node.Block.Statements.Any())
            {
                return SyntaxFactory.CatchClause(block:
                       SyntaxFactory.Block(statements:
                                SyntaxFactory.ThrowStatement()),
                        filter: null,
                        declaration: node.Declaration)
                        .WithAdditionalAnnotations(Formatter.Annotation);
            }
            return base.VisitCatchClause(node);

        }

    }
}
