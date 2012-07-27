using System;
using System.Collections.Generic;

namespace FunctionCSharpSamples
{
    public class EqualityComparer<T> : IEqualityComparer<T>
    {
        public EqualityComparer( Func<T, T, Boolean> equalityComparerMethod )
        {
            EqualityComparerMethod = equalityComparerMethod;
        }

        public Func<T, T, Boolean> EqualityComparerMethod 
        {
            get;
            set;
        }

        #region IEqualityComparer<T> Members

        public bool Equals( T x, T y )
        {
            return EqualityComparerMethod( x, y );
        }

        public int GetHashCode( T obj )
        {
            return obj.GetHashCode( );
        }

        #endregion
    }
}
