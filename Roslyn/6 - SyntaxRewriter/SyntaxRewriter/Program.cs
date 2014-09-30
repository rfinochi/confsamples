using System;
using System.IO;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SyntaxRewriter
{
    internal class Program
    {
        private static void Main()
        {
            Compilation test = CreateTestCompilation();

            foreach (SyntaxTree sourceTree in test.SyntaxTrees)
            {
                SemanticModel model = test.GetSemanticModel(sourceTree);

                TypeInferenceRewriter rewriter = new TypeInferenceRewriter(model);

                SyntaxNode newSource = rewriter.Visit(sourceTree.GetRoot());

                if (newSource != sourceTree.GetRoot())
                    //File.WriteAllText(sourceTree.FilePath, newSource.ToFullString());
                    Console.WriteLine(newSource.ToFullString());
            }

            Console.ReadLine();
        }

        private static Compilation CreateTestCompilation()
        {
            SyntaxTree programTree = CSharpSyntaxTree.ParseText(File.ReadAllText( @"..\..\Program.cs"));
            SyntaxTree rewriterTree = CSharpSyntaxTree.ParseText(File.ReadAllText(@"..\..\TypeInferenceRewriter.cs"));

            SyntaxTree[] sourceTrees = { programTree, rewriterTree };

            MetadataReference mscorlib = new MetadataFileReference(typeof(object).Assembly.Location);
            MetadataReference codeAnalysis = new MetadataFileReference(typeof(SyntaxTree).Assembly.Location);
            MetadataReference csharpCodeAnalysis = new MetadataFileReference(typeof(CSharpSyntaxTree).Assembly.Location);

            MetadataReference[] references = { mscorlib, codeAnalysis, csharpCodeAnalysis };

            return CSharpCompilation.Create("SyntaxRewriterTest",
                                            sourceTrees,
                                            references,
                                            new CSharpCompilationOptions(OutputKind.ConsoleApplication));
        }
    }
}