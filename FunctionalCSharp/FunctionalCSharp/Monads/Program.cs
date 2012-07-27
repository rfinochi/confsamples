namespace Monads
{
    class Program
    {
        static void Main(string[] args)
        {
            // Bind Example
            IdentityMonad.BindExample();

            // Select Many Example
            IdentityMonad.SelectManyExample();

            // LINQ Example
            IdentityMonad.LinqExample();

            // Maybe Monad Example
            MaybeMonad.LinqExample();

            // List Monad Example
            ListMonad.LinqExample();

            // Continuation Monad Example
            ContinuationMonad.LinqExample();

            // Async Computation Example
            AsyncComputationExample.LinqExample();
        }
    }
}
