using System;
using System.Collections.Generic;

namespace Common
{
    public sealed class Tuple<T1, T2> : IEquatable<Tuple<T1, T2>>
    {
        private readonly T1 item1;
        private readonly T2 item2;

        public Tuple(T1 item1, T2 item2)
        {
            this.item1 = item1;
            this.item2 = item2;
        }

        public override bool Equals(object obj)
        {
            if (obj is Tuple<T1, T2>)
                return Equals((Tuple<T1, T2>)obj);

            return false;
        }
        public bool Equals(Tuple<T1, T2> other)
        {
            if (!EqualityComparer<T1>.Default.Equals(item1, other.item1))
                return false;
            if (!EqualityComparer<T2>.Default.Equals(item2, other.item2))
                return false;

            return true;
        }
        public override int GetHashCode()
        {
            int result = EqualityComparer<T1>.Default.GetHashCode(item1);
            result ^= EqualityComparer<T2>.Default.GetHashCode(item2);
            return result;
        }

        public T1 Item1
        {
            get { return item1; }
        }
        public T2 Item2
        {
            get { return item2; }
        }
    }

    public static class Tuple
    {
        public static Tuple<T1, T2> New<T1, T2>(T1 fst, T2 snd)
        {
            return new Tuple<T1, T2>(fst, snd);
        }
    }
}
