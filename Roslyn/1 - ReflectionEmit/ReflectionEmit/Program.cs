using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ReflectionEmit
{
    class Program
    {
        static void Main( string[] args )
        {
            AssemblyName assemblyName = new AssemblyName( );
            assemblyName.Name = "Math";

            AssemblyBuilder CreatedAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly( assemblyName, AssemblyBuilderAccess.RunAndSave );

            ModuleBuilder AssemblyModule = CreatedAssembly.DefineDynamicModule( "MathModule", "Math.dll" );

            TypeBuilder MathType = AssemblyModule.DefineType( "DoMath", TypeAttributes.Public | TypeAttributes.Class );

            System.Type[] ParamTypes = new Type[] { typeof( int ), typeof( int ) };

            MethodBuilder SumMethod = MathType.DefineMethod( "Sum", MethodAttributes.Public, typeof( int ), ParamTypes );

            ParameterBuilder Param1 = SumMethod.DefineParameter( 1, ParameterAttributes.In, "num1" );
            ParameterBuilder Param2 = SumMethod.DefineParameter( 2, ParameterAttributes.In, "num2" );

            ILGenerator ilGenerator = SumMethod.GetILGenerator( );
            ilGenerator.Emit( OpCodes.Ldarg_1 );
            ilGenerator.Emit( OpCodes.Ldarg_2 );
            ilGenerator.Emit( OpCodes.Add );
            ilGenerator.Emit( OpCodes.Ret );

            MathType.CreateType( );

            CreatedAssembly.Save( "Math.dll" );
        }
    }
}