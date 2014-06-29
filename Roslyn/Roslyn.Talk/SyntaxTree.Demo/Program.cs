using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SyntaxTree.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = CSharpSyntaxTree.ParseText(
               @"
                using System;
                using System.Collections;
                using System.Linq;
                using System.Text;

                namespace HelloWorld 
                {
                    class Program
                    {
                        static void Main(string[] args)
                        {
                            Console.WriteLine(""Hola MUG"");
                        }  
                    }
                }");

            var sample = tree.GetRoot();

            var root = (CompilationUnitSyntax)tree.GetRoot();
            var firstMember = root.Members.First();
            var kind = firstMember.CSharpKind();
            var helloWorldDeclaration = (NamespaceDeclarationSyntax)firstMember;
            var programDeclaration = (ClassDeclarationSyntax)helloWorldDeclaration.Members.First();
            var mainDeclaration = (BaseMethodDeclarationSyntax)programDeclaration.Members.First();
            var argsParameter = mainDeclaration.ParameterList.Parameters.First();

            var firstParameter = root.DescendantNodes().OfType<MethodDeclarationSyntax>()
                                     .Where(x => x.Identifier.ValueText == "Main")
                                     .SelectMany(x => x.ParameterList.Parameters).Single();
        }
    }
}
