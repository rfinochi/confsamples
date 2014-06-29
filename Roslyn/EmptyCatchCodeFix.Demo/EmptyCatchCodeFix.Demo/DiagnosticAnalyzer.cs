using System;
using System.Collections.Immutable;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EmptyCatchCodeFix.Demo
{
    [DiagnosticAnalyzer]
    [ExportDiagnosticAnalyzer(DiagnosticId, LanguageNames.CSharp)]
    public class DiagnosticAnalyzer : ISyntaxNodeAnalyzer<SyntaxKind>
    {
        internal const string DiagnosticId = "EmptyCatchClauseCodeFix.Demo";
        internal const string Description = "Shows a Warning when a catch statement is empty.";
        internal const string MessageFormat = "{0} is Empty!";
        internal const string Category = "Code Quality";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Description, MessageFormat, Category, DiagnosticSeverity.Warning, true);

        public ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public ImmutableArray<SyntaxKind> SyntaxKindsOfInterest
        {
            get { return ImmutableArray.Create(SyntaxKind.CatchClause); }
        }

        public void AnalyzeNode(SyntaxNode node, SemanticModel semanticModel, Action<Diagnostic> addDiagnostic, CancellationToken cancellationToken)
        {
            var catchClause = node as CatchClauseSyntax;
            if (catchClause != null && !catchClause.Block.Statements.Any())
            {
                addDiagnostic(Diagnostic.Create(Rule, catchClause.GetLocation(), "Catch Statement"));
            }
        }
    }
}
