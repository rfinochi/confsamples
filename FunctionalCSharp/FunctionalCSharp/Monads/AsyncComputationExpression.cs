using System;
using System.IO;
using System.Text;

namespace Monads
{
    public delegate void Continuation<T>(Action<T> c);
    
    public class FileSystem
    {
        public Stream OpenRead(string path)
        {
            return File.OpenRead(path);
        }
    }
    
    public static class AsyncComputationExpression
    {
        public static Continuation<T> ToContinuation<T>(this T value)
        {
            return c => c(value);
        }

        public static Continuation<U> SelectMany<T, U>(this Continuation<T> m, Func<T, Continuation<U>> k)
        {
            return c => m(x => k(x)(c));
        }

        public static Continuation<V> SelectMany<T, U, V>(this Continuation<T> m, Func<T, Continuation<U>> k, Func<T, U, V> s)
        {
            return m.SelectMany(x => k(x).SelectMany(y => s(x, y).ToContinuation()));
        }

        public static Continuation<U> Select<U, T>(this Continuation<T> m, Func<T, U> k)
        {
            return c => m(x => c(k(x)));
        }

        public static Continuation<string> ReadToEndAsync(this Stream stream)
        {
            return send =>
            {
                var buffer = new byte[1024];
                var builder = new StringBuilder();
                Func<IAsyncResult> readChunk = null;
                readChunk = () => stream.BeginRead(buffer, 0, 1024, result =>
                {
                    var read = stream.EndRead(result);
                    if (read > 0)
                    {
                        builder.Append(Encoding.UTF8.GetString(buffer, 0, read));
                        readChunk();
                    }
                    else
                    {
                        stream.Dispose();
                        send(builder.ToString());
                    }
                }, null);
                readChunk();
            };
        } 
    }

    public static class AsyncComputationExample
    {
        public static void LinqExample()
        {
            Func<string, Continuation<string>> download_files = file =>
            {
                var stream = File.OpenRead(file);
                return 
                    from text in stream.ReadToEndAsync()
                    select text; 
            };

            var r = download_files("test.txt");
            r(Console.WriteLine);
            Console.ReadKey();
        }
    }
}
