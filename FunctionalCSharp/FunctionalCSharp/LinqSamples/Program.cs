using System;

namespace LinqSamples
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var recipientId = "1234567890";

            // Select

            // Imperative
            var findClaimsImperative = Transformations.FindClaimsFor(recipientId);
            Array.ForEach(findClaimsImperative, summary => Console.WriteLine(summary.ClaimTotal));

            // Extension Methods
            var findClaimsExtensions = Transformations.FindClaimsForLinq(recipientId);
            Array.ForEach(findClaimsExtensions, summary => Console.WriteLine(summary.ClaimTotal));

            // LINQ Expressions
            var findClaimsLinq = Transformations.FindClaimsForExpression(recipientId);
            Array.ForEach(findClaimsLinq, summary => Console.WriteLine(summary.ClaimTotal));

            // Where and SelectMany

            // Imperative
            var expensiveClaimsImperative = Transformations.FindExpensiveClaimsFor(recipientId);
            Array.ForEach(expensiveClaimsImperative, summary => Console.WriteLine(summary.ClaimPrice));

            // Extension Methods
            var expensiveClaimsExtensions = Transformations.FindExpensiveClaimsForLinq(recipientId);
            Array.ForEach(expensiveClaimsExtensions, summary => Console.WriteLine(summary.ClaimPrice));

            // LINQ Expressions
            var expensiveClaimsLinq = Transformations.FindExpensiveClaimsForExpression(recipientId);
            Array.ForEach(expensiveClaimsLinq, summary => Console.WriteLine(summary.ClaimPrice));
        }    
    }
}
