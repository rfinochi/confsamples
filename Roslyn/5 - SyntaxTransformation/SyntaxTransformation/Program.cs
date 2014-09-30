using System;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxTransformation
{
    class Program
    {
        static void Main(string[] args)
        {
            NameSyntax name = SyntaxFactory.IdentifierName("System");
            Console.WriteLine(name.ToString());

            name = SyntaxFactory.QualifiedName(name, SyntaxFactory.IdentifierName("Collections"));
            Console.WriteLine(name.ToString());

            name = SyntaxFactory.QualifiedName(name, SyntaxFactory.IdentifierName("Generic"));
            Console.WriteLine(name.ToString());

            SyntaxTree tree = CSharpSyntaxTree.ParseText(
                                @"using System;
                                using System.Collections;
                                using System.Linq;
                                using System.Text;
 
                                namespace HelloWorld
                                {
                                    class Program
                                    {
                                        static void Main(string[] args)
                                        {
                                            Console.WriteLine(""Hello, World!"");
                                        }
                                    }
                                }");

            var root = (CompilationUnitSyntax)tree.GetRoot();

            var oldUsing = root.Usings[1];
            var newUsing = oldUsing.WithName(name);

            root = root.ReplaceNode(oldUsing, newUsing);

            Console.WriteLine(root.ToString());

            Console.ReadLine();
        }
    }
}