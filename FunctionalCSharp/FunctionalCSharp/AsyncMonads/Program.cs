using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.FSharp;
using Microsoft.FSharp.Control;
using Microsoft.FSharp.Core;

namespace AsyncMonads
{
    public static class FileSystem
    {
        public static Async<FileStream> AsyncOpen(string path, FileMode mode)
        {
            return AsyncOpen(path, mode, (mode == FileMode.Append) ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None);
        }

        public static Async<FileStream> AsyncOpen(string path, FileMode mode, FileAccess access)
        {
            return AsyncOpen(path, mode, access, FileShare.None);
        }

        public static Async<FileStream> AsyncOpen(string path, FileMode mode, FileAccess access, FileShare share)
        {
            Func<FileStream> f = () => File.Open(path, mode, access, share);
            return AsyncExtensions.UnblockViaNewThread(FuncConvertExtensions.ToFastFunc(f));
        }

        public static Async<FileStream> AsyncOpenRead(string path)
        {
            Func<FileStream> f = () => File.OpenRead(path);
            return AsyncExtensions.UnblockViaNewThread(FuncConvertExtensions.ToFastFunc(f));
        }

        public static Async<StreamReader> AsyncOpenText(string path)
        {
            Func<StreamReader> f = () => File.OpenText(path);
            return AsyncExtensions.UnblockViaNewThread(FuncConvertExtensions.ToFastFunc(f));
        }
    }

    public static class WebExtensions
    {
        public static Async<WebRequest> AsyncCreateWebRequest
            (string requestUriString)
        {
            Func<WebRequest> f = () => WebRequest.Create(requestUriString);
            return AsyncExtensions.UnblockViaNewThread(FuncConvertExtensions.ToFastFunc(f));
        }

        public static Async<WebResponse> AsyncGetResponse
            (this WebRequest request)
        {
            return AsyncExtensions.BuildPrimitive<WebResponse>(
                request.BeginGetResponse, request.EndGetResponse);
        }
    }

    public static class IOExtensions
    {
        public static Async<string> AsyncReadToEnd(this StreamReader reader)
        {
            Func<string> f = () => reader.ReadToEnd();
            return AsyncExtensions.UnblockViaNewThread(FuncConvertExtensions.ToFastFunc(f));
        }
    }

    static class AsyncExtensions
    {
        public static AsyncBuilder async = CreateAsyncBuilder();

        private static AsyncBuilder CreateAsyncBuilder()
        {
            var asyncType = typeof(AsyncBuilder);
            var ci = asyncType.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic, 
                null, new Type[0], null);
            var result = ci.Invoke(null);

            return result as AsyncBuilder;
        }

        public static FastFunc<Tuple<A, B>, C> ToTupledFastFunc<A, B, C>
            (this Func<A, B, C> f)
        {
            return FuncConvertExtensions.ToTupledFastFunc(f);
        }
        public static FastFunc<Tuple<A, B, C>, D> ToTupledFastFunc<A, B, C, D>
            (this Func<A, B, C, D> f)
        {
            return FuncConvertExtensions.ToTupledFastFunc(f);
        } 

        public static FastFunc<A, B> ToFastFunc<A, B>
            (this Func<A, B> f)
        {
            return FuncConvertExtensions.ToFastFunc(f);
        }
        
        public static Async<B> Select<A, B>(
            this Async<A> x, Func<A, B> selector)
        {
            return async.Bind(x,
                ToFastFunc<A, Async<B>>((r) => async.Return(selector(r))));
        }

        public static Async<V> SelectMany<T, U, V>(
            this Async<T> p, Func<T, Async<U>> selector, Func<T, U, V> projector)
        {
            return async.Bind(p, ToFastFunc<T, Async<V>>(r1 =>
                async.Bind(selector(r1), ToFastFunc<U, Async<V>>(r2 =>
                    async.Return(projector(r1, r2))))));
        }

        public static Async<R[]> Parallel<R>(IEnumerable<Async<R>> computations)
        {
            return Async.Parallel(computations);
        }
        public static R Run<R>(Async<R> computation)
        {
            return Async.Run(computation, Option<int>.None, Option<bool>.None);
        }

        public static Async<T> UnblockViaNewThread<T>(FastFunc<Unit, T> f)
        {
            var type = typeof(FileExtensions);
            var methodInfo = type.GetMethod("UnblockViaNewThread", BindingFlags.NonPublic | BindingFlags.Static);

            var genericArguments = new[] { typeof(T) };
            var genericMethodInfo = methodInfo.MakeGenericMethod(genericArguments);
            return genericMethodInfo.Invoke(null, new[] { f }) as Async<T>;
        }

        public static Async<Unit> BuildVoidPrimitive(
            Func<AsyncCallback, object, IAsyncResult> begin,
            Action<IAsyncResult> end)
        {
            return Async.BuildPrimitive(
                begin.ToTupledFastFunc(), FuncConvert.ToFastFunc(end));
        }

        public static Async<R> BuildPrimitive<R>(
            Func<AsyncCallback, object, IAsyncResult> begin,
            Func<IAsyncResult, R> end)
        {
            return Async.BuildPrimitive(
                begin.ToTupledFastFunc(), end.ToFastFunc());
        }

        public static Async<R> BuildPrimitive<Arg, R>(
            Arg a,
            Func<Arg, AsyncCallback, object, IAsyncResult> begin,
            Func<IAsyncResult, R> end)
        {
            return Async.BuildPrimitive(
                a, begin.ToTupledFastFunc(), end.ToFastFunc());
        } 
            
        public static Async<int> StartWorkflow = async.Return(0);
        }

    class Program
    {
        static void Main(string[] args)
        {
            Func<string, IEnumerable<Match>> get_links = html =>
            {
                var linkRegex = new Regex(
                    @"<a\s{1}href=\""(?<url>.*?)\""(\s?target=\""" +
                    @"(?<target>_(blank|new|parent|self|top))\"")?" +
                    @"(\s?class=\""(?<class>.*?)\"")?(\s?style=\""" +
                    @"(?<style>.*?)\"")?>(?<title>.*?)</a>");
                return linkRegex.Matches(html).Cast<Match>();
            };

            Func<string, Async<Tuple<string, int>>> links_async = requestUriString =>
                from _ in AsyncExtensions.StartWorkflow
                let request = WebRequest.Create(requestUriString)
                from response in request.AsyncGetResponse()
                let reader = new StreamReader(response.GetResponseStream())
                from html in reader.AsyncReadToEnd()
                let links = get_links(html)
                select new Tuple<string, int>(requestUriString, links.Count());

            var sites = new[]
                {
                    "http://live.com/",
                    "http://www.google.com/",
                    "http://codebetter.com/"
                };
            var results = Async.Run(
                Async.Parallel(from site in sites select links_async(site)), 
                    Option<int>.None,
                    Option<bool>.None);
            foreach (var result in results) 
                Console.WriteLine("Site {0} has {1} links", result.Item1, result.Item2);

            Func<string, Async<string>> read_file = path =>
                from _ in AsyncExtensions.StartWorkflow
                from reader in FileSystem.AsyncOpenText(path)
                from text in reader.AsyncReadToEnd()
                select text;
            var files = Directory.GetFiles(@"D:\Program Files\FSharp-1.9.6.2\source\fsharp\FSharp.Core\math");
            var fileTexts = Async.Run(
                Async.Parallel(from file in files select read_file(file)),
                Option<int>.None, Option<bool>.None);
            Array.ForEach(fileTexts, Console.WriteLine);

            Func<string, Async<string>> get_html_async = url =>
               from _ in AsyncExtensions.StartWorkflow
               from request in WebExtensions.AsyncCreateWebRequest(url)
               from response in request.AsyncGetResponse()
               let reader = new StreamReader(response.GetResponseStream())
               from urlText in reader.AsyncReadToEnd()
               select urlText;

            var urls = new[] { "http://live.com/", "http://www.google.com/", "http://codebetter.com/" };
            var urlTexts = Async.Run(
                Async.Parallel(from url in urls select get_html_async(url)), Option<int>.None, Option<bool>.None);
            Array.ForEach(urlTexts, Console.WriteLine);
        }
    }
}
