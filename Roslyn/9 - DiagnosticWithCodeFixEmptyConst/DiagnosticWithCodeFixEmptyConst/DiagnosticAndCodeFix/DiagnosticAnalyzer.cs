using System;
using System.Collections.Immutable;
using System.Linq;
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
        internal const string DiagnosticId = "MakeConst";
        internal const string Description = "Make Constant";
        internal const string MessageFormat = "Can be made constant";
        internal const string Category = "Usage";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Description, MessageFormat, Category, DiagnosticSeverity.Warning, true);

        public ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public ImmutableArray<SyntaxKind> SyntaxKindsOfInterest
        {
            get
            {
                return ImmutableArray.Create(SyntaxKind.LocalDeclarationStatement);
            }
        }

        public void AnalyzeNode(SyntaxNode node, SemanticModel semanticModel, Action<Diagnostic> addDiagnostic, AnalyzerOptions options, CancellationToken cancellationToken)
        {
            var localDeclaration = (LocalDeclarationStatementSyntax)node;

            if (localDeclaration.Modifiers.Any(SyntaxKind.ConstKeyword))
                return;

            var variableTypeName = localDeclaration.Declaration.Type;
            var variableType = semanticModel.GetTypeInfo(variableTypeName).ConvertedType;

            foreach (var variable in localDeclaration.Declaration.Variables)
            {
                var initializer = variable.Initializer;
                if (initializer == null)
                    return;

                var constantValue = semanticModel.GetConstantValue(initializer.Value);
                if (!constantValue.HasValue)
                    return;

                var conversion = semanticModel.ClassifyConversion(initializer.Value, variableType);
                if (!conversion.Exists || conversion.IsUserDefined)
                    return;

                if (constantValue.Value is string)
                    if (variableType.SpecialType != SpecialType.System_String)
                        return;
                    else if (variableType.IsReferenceType && constantValue.Value != null)
                        return;
            }

            var dataFlowAnalysis = semanticModel.AnalyzeDataFlow(localDeclaration);

            foreach (var variable in localDeclaration.Declaration.Variables)
            {
                var variableSymbol = semanticModel.GetDeclaredSymbol(variable);
                if (dataFlowAnalysis.WrittenOutside.Contains(variableSymbol))
                    return;
            }

            addDiagnostic(Diagnostic.Create(Rule, node.GetLocation()));
        }
    }
}