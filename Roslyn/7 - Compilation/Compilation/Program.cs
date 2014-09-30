using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Compilation
{
    class Program
    {
        static void Main(string[] args)
        {
            var someCode = @"using System;

                            namespace SampleDemoApplication
                            {
                                public class Program 
                                {
                                    static void Main(){}

                                    public void Print()
                                    {
                                        Console.WriteLine(""Hello"");
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

            using (var peStream = new MemoryStream())
            {
                using (var pdbStream = new MemoryStream())
                {
                    var result = compilation.Emit(peStream: peStream, pdbStream: pdbStream);

                    if (result.Success)
                    {
                        var assembly = Assembly.Load(peStream.ToArray(), pdbStream.ToArray());
                        var types = assembly.GetTypes();
                        dynamic instance = Activator.CreateInstance(assembly.GetTypes().First());

                        instance.Print();

                        var addResult = instance.Add();
                        Console.WriteLine(addResult);
                    }
                    else
                    {
                        foreach (var diganostic in result.Diagnostics)
                        {
                            Console.WriteLine(diganostic);
                        }
                    }
                }
            }

            Console.ReadLine();
        }
    }
}