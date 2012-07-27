using System;

namespace PatternMatching
{
    class Program
    {
        public static bool IsEven(int? n)
        {
            return n != null ? n % 2 == 0 : false;
        }

        public static bool IsOdd(int? n)
        {
            return n != null ? !IsEven(n) : true;
        }

        static void Main(string[] args)
        {
            int? n = 3;
            var oddNumber = n.Match()
                .With(IsEven, x => "Even number")
                .With(IsOdd, x => "Odd number")
                .Else(x => "Null")
                .Do();
            Console.WriteLine(oddNumber);

            var stringValue = "foobar";
            var throwsException = stringValue.Match()
                .With(x => x == "foo", x => "Found foo")
                .With(x => x == "bar", x => "Found bar")
                .Do();
            Console.WriteLine(throwsException);

        }
    }
}
