using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.IO;
using System.Reflection;

namespace Compilation.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var someCode = @"

            using System;
            namespace SampleDemoApplication
            {
                public class Program 
                {
                    static void Main(){}

                    public void Print()
                    {
                        Console.WriteLine(""Escribiendo un Mensaje"");
                    }

                    public int Add()
                    {
                        return 1 + 2;
                    }
                }
            }";

            var syntaxTree = CSharpSyntaxTree.ParseText(someCode);

            var compilationOptions = new CSharpCompilationOptions(OutputKind.ConsoleApplication);
            var compilation = CSharpCompilation.Create("SampleApp", options: compilationOptions)
                                               .AddReferences(new MetadataFileReference(typeof(object).Assembly.Location))
                                               .AddSyntaxTrees(syntaxTree);

            using (var outputStream = new MemoryStream())
            using (var pdbStream = new MemoryStream())
            {
                var result = compilation.Emit(outputStream: outputStream, pdbStream: pdbStream);
                if (result.Success)
                {
                    var assembly = Assembly.Load(outputStream.ToArray(), pdbStream.ToArray());
                    var types = assembly.GetTypes();
                    dynamic instance = Activator.CreateInstance(assembly.GetTypes().First());
                    var opResult = instance.Add();
                    Console.WriteLine(opResult);
                }
                else
                {
                    foreach (var diganostic in result.Diagnostics)
                    {
                        Console.WriteLine(diganostic);
                    }
                }
            }
            Console.Read();

        }
    }
}
