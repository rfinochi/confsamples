using System;
using System.Collections.Generic;

namespace FunctionCSharpSamples
{
    public static class Enumerable
    {
        public static IEnumerable<int> Range( int from, int to )
        {
            for ( int i = from; i < to; i++ )
                yield return i;
        }
    }
}