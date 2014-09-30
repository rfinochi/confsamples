using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Text;

namespace DiagnosticAndCodeFix
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
            var root = await document.GetSyntaxRootAsync(cancellationToken);

            var token = root.FindToken(span.Start);

            if (!token.IsKind(SyntaxKind.CatchKeyword))
                return null;

            var catchBlock = (CatchClauseSyntax)token.Parent;
            var tryStmt = (TryStatementSyntax)catchBlock.Parent;

            var throwStatement = SyntaxFactory.ThrowStatement();
            var newStatements = new SyntaxList<StatementSyntax>().Add(throwStatement);
            var newBlock = SyntaxFactory.Block().WithStatements(newStatements);
            var newCatchBlock = SyntaxFactory.CatchClause().WithBlock(newBlock).WithAdditionalAnnotations(Formatter.Annotation);

            var newRoot = root.ReplaceNode(catchBlock, newCatchBlock);

            return new[] { CodeAction.Create("throw", document.WithSyntaxRoot(newRoot)) };
        }
    }
}