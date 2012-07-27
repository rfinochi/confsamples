using System;
using System.Linq;

namespace Monads
{
    public static class IdentityExtensions
    {
        public static Identity<T> ToIdentity<T>(this T value)
        {
            return new Identity<T>(value);
        }

        public static Identity<U> SelectMany<T, U>(this Identity<T> id, Func<T, Identity<U>> k)
        {
            return k(id.Value);
        }

        public static Identity<V> SelectMany<T, U, V>(this Identity<T> id, Func<T, Identity<U>> k, Func<T, U, V> s)
        {
            return s(id.Value, k(id.Value).Value).ToIdentity();
        }
    }

    public class IdentityMonad
    {
        public static Identity<T> Unit<T>(T value)
        {
            return new Identity<T>(value);
        }

        public static Identity<U> Bind<T, U>(Identity<T> id, Func<T, Identity<U>> k)
        {
            return k(id.Value);
        }

        public static void BindExample()
        {
            var r = Bind(Unit(7), x =>
                      Bind(Unit(6), y =>
                        Unit(x + y)));

            Console.WriteLine("Bind of 6 and 7 is {0}", r.Value);
        }

        public static void SelectManyExample()
        {
            var r = 7.ToIdentity().SelectMany(
                x => 6.ToIdentity().SelectMany(
                    y => (x + y).ToIdentity()));

            Console.WriteLine("Select Many with 6 and 7 is {0}", r.Value);
        }

        public static void LinqExample()
        {
            var r = from x in 7.ToIdentity()
                    from y in 6.ToIdentity()
                    select x + y;

            Console.WriteLine("LINQ with 6 and 7 is {0}", r.Value);
        }

 
    }

    public class Identity<T>
    {
        public T Value { get; private set; }
        public Identity(T value) { this.Value = value; }
    }
}
