using System;

using System.Console;

namespace CSharp6Features
{
    class Program
    {
        static void Main(string[] args)
        {
            UsingStatic();
            ExceptionFiltering();
            NullPropagating();

            Console.ReadLine();
        }

        public static void ExceptionFiltering()
        {
            try
            {
                throw new Exception("Me");
            }
            catch (Exception ex) if (ex.Message == "You")
            {
                Console.WriteLine("You");
            }
            catch (Exception ex) if (ex.Message == "Me")
            {
                Console.WriteLine("Me");
            }
        }

        public static void UsingStatic()
        {
            WriteLine("Hello");
        }

        public static void NullPropagating()
        {
            Product product = null;


            double minPrice = 0;
            double? minPriceNulleable = 0;

            if (product != null
                && product.PriceBreaks != null
                && product.PriceBreaks[0] != null)
            {
                minPrice = product.PriceBreaks[0].Price;
            }

            minPriceNulleable = product?.PriceBreaks?[0]?.Price;

            minPrice = product?.PriceBreaks?[0]?.Price ?? 0;

            Console.WriteLine(minPriceNulleable);
            Console.WriteLine(minPrice);
        }

        class Product
        {
            public double Price { get; set; }

            public Product[] PriceBreaks { get; set; }
        }
    }
}