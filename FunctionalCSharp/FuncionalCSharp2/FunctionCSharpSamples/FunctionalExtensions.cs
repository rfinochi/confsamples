using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionCSharpSamples
{
    public static class FunctionalExtensions
    {
        public static void ForEach<T>( IEnumerable<T> items, Action<T> action )
        {
            foreach ( T item in items )
                action( item );
        }

        //public static void ForEach<T>( this IEnumerable<T> items, Action<T> action )
        //{
        //    foreach ( T item in items )
        //        action( item );
        //}

        public static void Use<T>( this T item, Action<T> action ) where T : IDisposable
        {
            using ( item )
            {
                action( item );
            }
        }

        public static IEnumerable<U> Convert<T, U>( this IEnumerable<T> items, Func<T, U> conversion )
        {
            foreach ( T item in items )
                yield return conversion( item );
        }

        //public static IEnumerable<KeyValuePair<T, int>> GetCounts<T>( this IEnumerable<T> items, Func<T, T, Boolean> comparerMethod )
        //{
        //    return items
        //        .Distinct( )
        //        .Convert( item => new KeyValuePair<T, int>( item, items.Count( i => comparerMethod( i, item ) ) ) );
        //}

        //public static IEnumerable<KeyValuePair<T, int>> GetCounts<T>( this IEnumerable<T> items, Func<T, T, Boolean> comparerMethod )
        //{
        //    Dictionary<T, int> result = new Dictionary<T, int>( new EqualityComparer<T>( comparerMethod ) );

        //    foreach ( T item in items )
        //    {
        //        if ( result.ContainsKey( item ) )
        //            result[ item ]++;
        //        else
        //            result.Add( item, 1 );
        //    }

        //    return result;
        //}
    }
}