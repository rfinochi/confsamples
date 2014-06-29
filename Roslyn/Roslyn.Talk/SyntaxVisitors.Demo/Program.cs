using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SyntaxVisitors.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var statement = @"
                            public void SomeMethod(int someValue)
                            {
                                try
                                {
                                    var result = someValue / 0;
                                }
                                catch (DivideByZeroException ex)
                                {
                                }
                            }";

            var tree = CSharpSyntaxTree.ParseText(statement);
            //Console.WriteLine(tree.ToString());
            //new CatchSyntaxWalker().Visit(tree.GetRoot());

            Console.WriteLine(new CatchSyntaxRewriter().Visit(tree.GetRoot()));
            Console.Read();
        }
    }
}
