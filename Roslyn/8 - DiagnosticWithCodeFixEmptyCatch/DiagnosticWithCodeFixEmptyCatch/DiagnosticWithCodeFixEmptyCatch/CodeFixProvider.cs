using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

namespace DiagnosticWithCodeFixEmptyCatch
{
    [ExportCodeFixProvider("DiagnosticWithCodeFixEmptyCatchCodeFixProvider", LanguageNames.CSharp), Shared]
    public class DiagnosticWithCodeFixEmptyCatchCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> GetFixableDiagnosticIds()
        {
            return ImmutableArray.Create(DiagnosticWithCodeFixEmptyCatchAnalyzer.DiagnosticId);
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task ComputeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
                             
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var token = root.FindToken(diagnosticSpan.Start);

            if (token.IsKind(SyntaxKind.CatchKeyword))
            {
                var catchBlock = (CatchClauseSyntax)token.Parent;
                var tryStmt = (TryStatementSyntax)catchBlock.Parent;

                var throwStatement = SyntaxFactory.ThrowStatement();
                var newStatements = new SyntaxList<StatementSyntax>().Add(throwStatement);
                var newBlock = SyntaxFactory.Block().WithStatements(newStatements);
                var newCatchBlock = SyntaxFactory.CatchClause().WithBlock(newBlock).WithAdditionalAnnotations(Formatter.Annotation);

                var newRoot = root.ReplaceNode(catchBlock, newCatchBlock);

                context.RegisterFix(
                    CodeAction.Create("throw", context.Document.WithSyntaxRoot(newRoot)),
                    diagnostic);
            }
        }
    }
}