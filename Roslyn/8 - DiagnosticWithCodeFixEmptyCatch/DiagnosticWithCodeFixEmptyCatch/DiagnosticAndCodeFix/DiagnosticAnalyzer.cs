using System;
using System.Collections.Immutable;
using System.Threading;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DiagnosticAndCodeFix
{
    [DiagnosticAnalyzer]
    [ExportDiagnosticAnalyzer(DiagnosticId, LanguageNames.CSharp)]
    public class DiagnosticAnalyzer : ISyntaxNodeAnalyzer<SyntaxKind>
    {
        internal const string DiagnosticId = "EmptyCatchDiagnostic";
        internal const string Description = "Catch block is empty, app could be unknowningly missing exceptions";
        internal const string MessageFormat = "'{0}' is empty, app could be unknowningly missing exceptions";
        internal const string Category = "Safety";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Description, MessageFormat, Category, DiagnosticSeverity.Warning, true);

        public ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return ImmutableArray.Create(Rule);
            }
        }

        public ImmutableArray<SyntaxKind> SyntaxKindsOfInterest
        {
            get
            {
                return ImmutableArray.Create(SyntaxKind.CatchClause);
            }
        }

        public void AnalyzeNode(SyntaxNode node, SemanticModel semanticModel, Action<Diagnostic> addDiagnostic, AnalyzerOptions options, CancellationToken cancellationToken)
        {
            var catchBlock = node as CatchClauseSyntax;

            if (catchBlock != null && catchBlock.Block != null && catchBlock.Block.Statements.Count == 0)
            {
               var diagnostic = Diagnostic.Create(Rule, catchBlock.CatchKeyword.GetLocation(), "Catch block");
                addDiagnostic(diagnostic);
            }
        }

    }
}