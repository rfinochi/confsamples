using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DiagnosticWithCodeFixEmptyCatch
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DiagnosticWithCodeFixEmptyCatchAnalyzer : DiagnosticAnalyzer
    {
        internal const string DiagnosticId = "EmptyCatchDiagnostic";
        internal const string Title = "Catch block is empty, app could be unknowningly missing exceptions";
        internal const string MessageFormat = "'{0}' is empty, app could be unknowningly missing exceptions";
        internal const string Category = "Safety";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.CatchClause);
        }

        private static void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var catchBlock = context.Node as CatchClauseSyntax;

            if (catchBlock != null && catchBlock.Block != null && catchBlock.Block.Statements.Count == 0)
            {
                var diagnostic = Diagnostic.Create(Rule, catchBlock.CatchKeyword.GetLocation(), "Catch block");
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
