using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxRewriter
{
    public class TypeInferenceRewriter : CSharpSyntaxRewriter
    {
        private readonly SemanticModel SemanticModel;

        public TypeInferenceRewriter(SemanticModel semanticModel)
        {
            this.SemanticModel = semanticModel;
        }

        public override SyntaxNode VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            if (node.Declaration.Variables.Count > 1)
                return node;
            if (node.Declaration.Variables[0].Initializer == null)
                return node;

            VariableDeclaratorSyntax declarator = node.Declaration.Variables.First();
            TypeSyntax variableTypeName = node.Declaration.Type;

            ITypeSymbol variableType = (ITypeSymbol)SemanticModel.GetSymbolInfo(variableTypeName).Symbol;

            TypeInfo initializerInfo = SemanticModel.GetTypeInfo(declarator.Initializer.Value);

            if (variableType == initializerInfo.Type)
            {
                TypeSyntax varTypeName = SyntaxFactory.IdentifierName("var")
                                                .WithLeadingTrivia(variableTypeName.GetLeadingTrivia())
                                                .WithTrailingTrivia(variableTypeName.GetTrailingTrivia());

                return node.ReplaceNode(variableTypeName, varTypeName);
            }
            else
            {
                return node;
            }
        }
    }
}