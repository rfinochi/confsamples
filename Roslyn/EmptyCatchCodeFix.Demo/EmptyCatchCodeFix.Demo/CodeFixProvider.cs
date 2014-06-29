using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.Formatting;

namespace EmptyCatchCodeFix.Demo
{
    [ExportCodeFixProvider(DiagnosticAnalyzer.DiagnosticId, LanguageNames.CSharp)]
    internal class CodeFixProvider : ICodeFixProvider
    {
        public IEnumerable<string> GetFixableDiagnosticIds()
        {
            return new[] { DiagnosticAnalyzer.DiagnosticId };
        }

        public async Task<IEnumerable<CodeAction>> GetFixesAsync(Document document, TextSpan span, IEnumerable<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            var token = root.FindToken(span.Start);

            if (token.IsKind(SyntaxKind.CatchKeyword))
            {
                var catchClauseSyntax = token.Parent as CatchClauseSyntax;
                if (catchClauseSyntax != null)
                {
                    var newCatchClause = SyntaxFactory.CatchClause(block:
                                         SyntaxFactory.Block(statements:
                                         SyntaxFactory.ThrowStatement()),
                                                        filter: catchClauseSyntax.Filter,
                                                        declaration: catchClauseSyntax.Declaration)
                                                      .WithAdditionalAnnotations(Formatter.Annotation);

                    var newRoot = root.ReplaceNode(catchClauseSyntax, newCatchClause);
                    return new[] { CodeAction.Create("Add 'throw' to empty catch statements", document.WithSyntaxRoot(newRoot)) };
                }
            }
            return null;
        }
    }
}