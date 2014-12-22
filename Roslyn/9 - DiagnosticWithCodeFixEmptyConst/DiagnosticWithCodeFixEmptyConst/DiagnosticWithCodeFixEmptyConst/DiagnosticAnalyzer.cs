using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DiagnosticWithCodeFixEmptyConst
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DiagnosticWithCodeFixEmptyConstAnalyzer : DiagnosticAnalyzer
    {
        internal const string DiagnosticId = "MakeConst";
        internal const string Title = "Make Constant";
        internal const string MessageFormat = "Can be made constant";
        internal const string Category = "Usage";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.LocalDeclarationStatement);
        }
        private static void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var localDeclaration = (LocalDeclarationStatementSyntax)context.Node;

            if (localDeclaration.Modifiers.Any(SyntaxKind.ConstKeyword))
                return;

            var variableTypeName = localDeclaration.Declaration.Type;
            var variableType = context.SemanticModel.GetTypeInfo(variableTypeName).ConvertedType;

            foreach (var variable in localDeclaration.Declaration.Variables)
            {
                var initializer = variable.Initializer;
                if (initializer == null)
                    return;

                var constantValue = context.SemanticModel.GetConstantValue(initializer.Value);
                if (!constantValue.HasValue)
                    return;

                var conversion = context.SemanticModel.ClassifyConversion(initializer.Value, variableType);
                if (!conversion.Exists || conversion.IsUserDefined)
                    return;

                if (constantValue.Value is string)
                    if (variableType.SpecialType != SpecialType.System_String)
                        return;
                    else if (variableType.IsReferenceType && constantValue.Value != null)
                        return;
            }

            var dataFlowAnalysis = context.SemanticModel.AnalyzeDataFlow(localDeclaration);

            foreach (var variable in localDeclaration.Declaration.Variables)
            {
                var variableSymbol = context.SemanticModel.GetDeclaredSymbol(variable);
                if (dataFlowAnalysis.WrittenOutside.Contains(variableSymbol))
                    return;
            }

            context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation()));
        }
    }
}
