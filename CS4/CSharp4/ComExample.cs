namespace ConsoleApplication1
{
    class ComExample
    {
        static void Main2( string[] args )
        {
            #region COM

            #region C#3

            Scripting.FileSystemObject fso = new Scripting.FileSystemObjectClass( );
            var stream = fso.OpenTextFile( @"C:\Archivo.txt", Scripting.IOMode.ForReading, true, Scripting.Tristate.TristateUseDefault );

            #endregion

            #region C#4

            //var fso = new Scripting.FileSystemObject();
            //var stream = 
            //    fso.OpenTextFile(
            //        Create: true, 
            //        FileName: @"C:\archivo.txt");

            #endregion

            #endregion
        }
    }
}