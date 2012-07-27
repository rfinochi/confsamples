using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MapReduce
{
    static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }

    }

    class Program
    {
        private static IEnumerable<KeyValuePair<string, int>> WordCountMap(
            string docId,
            string doc)
        {
            char[] splitChars = { ' ', '\r', '\n', '\t', '\x0085' };
            foreach (var word in doc.Split(splitChars, StringSplitOptions.RemoveEmptyEntries))
                yield return new KeyValuePair<string, int>(word, 1);
        }

        private static IEnumerable<KeyValuePair<string, int>> WordCountReduce(
            string word,
            IEnumerable<int> counts)
        {
            var wordCount = counts.Sum();
            yield return new KeyValuePair<string, int>(word, wordCount);
        }

        private static IEnumerable<KeyValuePair<string, string>> AllFilesInDirectory(
            string path,
            string searchPattern)
        {
            foreach (var fileName in Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories))
                yield return new KeyValuePair<string, string>(fileName, File.ReadAllText(fileName));
        }

        private static void Main(string[] args)
        {
            MapReduce.Run<string, string, string, int, string, int>(WordCountMap, WordCountReduce, AllFilesInDirectory(args[0], "*.cs")).ForEach(wordAndCount =>
                Console.WriteLine("{0}: {1}", wordAndCount.Value, wordAndCount.Key));
        }
    }
}
