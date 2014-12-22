using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Simplification;

namespace DiagnosticWithCodeFixEmptyConst
{
    [ExportCodeFixProvider("DiagnosticWithCodeFixEmptyConstCodeFixProvider", LanguageNames.CSharp), Shared]
    public class DiagnosticWithCodeFixEmptyConstCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> GetFixableDiagnosticIds()
        {
            return ImmutableArray.Create(DiagnosticWithCodeFixEmptyConstAnalyzer.DiagnosticId);
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

            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<LocalDeclarationStatementSyntax>().First();

            context.RegisterFix(CodeAction.Create("Make constant", c => MakeConstAsync(context.Document, declaration, c)), diagnostic);
        }

        private async Task<Document> MakeConstAsync(Document document, LocalDeclarationStatementSyntax localDeclaration, CancellationToken cancellationToken)
        {
            var firstToken = localDeclaration.GetFirstToken();
            var leadingTrivia = firstToken.LeadingTrivia;
            var trimmedLocal = localDeclaration.ReplaceToken(firstToken, firstToken.WithLeadingTrivia(SyntaxTriviaList.Empty));

            var constToken = SyntaxFactory.Token(leadingTrivia, SyntaxKind.ConstKeyword, SyntaxFactory.TriviaList(SyntaxFactory.ElasticMarker));

            var newModifiers = trimmedLocal.Modifiers.Insert(0, constToken);

            var variableDeclaration = localDeclaration.Declaration;
            var variableTypeName = variableDeclaration.Type;
            if (variableTypeName.IsVar)
            {
                var semanticModel = await document.GetSemanticModelAsync(cancellationToken);

                var aliasInfo = semanticModel.GetAliasInfo(variableTypeName);
                if (aliasInfo == null)
                {
                    var type = semanticModel.GetTypeInfo(variableTypeName).ConvertedType;

                    if (type.Name != "var")
                    {
                        var typeName = SyntaxFactory.ParseTypeName(type.ToDisplayString())
                            .WithLeadingTrivia(variableTypeName.GetLeadingTrivia())
                            .WithTrailingTrivia(variableTypeName.GetTrailingTrivia());

                        var simplifiedTypeName = typeName.WithAdditionalAnnotations(Simplifier.Annotation);

                        variableDeclaration = variableDeclaration.WithType(simplifiedTypeName);
                    }
                }
            }

            var newLocal = trimmedLocal.WithModifiers(newModifiers)
                                       .WithDeclaration(variableDeclaration);

            var formattedLocal = newLocal.WithAdditionalAnnotations(Formatter.Annotation);

            var root = await document.GetSyntaxRootAsync(cancellationToken);
            var newRoot = root.ReplaceNode(localDeclaration, formattedLocal);

            return document.WithSyntaxRoot(newRoot);
        }

        private async Task<Solution> MakeUppercaseAsync(Document document, TypeDeclarationSyntax typeDecl, CancellationToken cancellationToken)
        {
            // Compute new uppercase name.
            var identifierToken = typeDecl.Identifier;
            var newName = identifierToken.Text.ToUpperInvariant();

            // Get the symbol representing the type to be renamed.
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
            var typeSymbol = semanticModel.GetDeclaredSymbol(typeDecl, cancellationToken);

            // Produce a new solution that has all references to that type renamed, including the declaration.
            var originalSolution = document.Project.Solution;
            var optionSet = originalSolution.Workspace.Options;
            var newSolution = await Renamer.RenameSymbolAsync(document.Project.Solution, typeSymbol, newName, optionSet, cancellationToken).ConfigureAwait(false);

            // Return the new solution with the now-uppercase type name.
            return newSolution;
        }
    }
}