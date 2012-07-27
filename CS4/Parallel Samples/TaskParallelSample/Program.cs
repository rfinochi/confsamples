namespace Parallel
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            var invoice = new
            {
                CustomerId = 33,
                ProductId = 22,
                Quantity = 9,
                Amount = 3423.34m,
            };

            var startTime = Environment.TickCount;

            #region Versión secuencial
            /*
            if (!Stock.HasStock(invoice.ProductId, invoice.Quantity))
                throw new Exception();

            if (!CRM.HasCredit(invoice.CustomerId, invoice.Amount))
                throw new Exception();

            DateTime deliveryDate = Delivery.GetDeliveryDate(invoice.CustomerId, invoice.ProductId, invoice.Quantity);

            decimal discount = CRM.ValidateDiscount(invoice.CustomerId, invoice.CustomerId, invoice.Quantity, invoice.Amount, 7m);

            ERP.SaveInvoice(invoice.CustomerId, invoice.ProductId, invoice.Quantity, discount);

            CRM.NewInvoice(invoice.CustomerId, invoice.ProductId, invoice.Quantity, deliveryDate, discount);

            Delivery.RequestDelivery(invoice.CustomerId, invoice.ProductId, invoice.Quantity, deliveryDate);

            Console.WriteLine(Environment.TickCount - startTime);
            */
            #endregion

            #region Version concurrente
           
            var validateStock = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Stock Validation - Start");
                    var result = Stock.HasStock(invoice.ProductId, invoice.Quantity);
                    Console.WriteLine("Stock Validation - End");
                    
                    return result;
                }
            );
            var hasCredit = Task.Factory.StartNew(() =>
                 {
                    Console.WriteLine("Credit Validation - Start");
                    var result = CRM.HasCredit(invoice.CustomerId, invoice.Amount);
                    Console.WriteLine("Credit Validation - End");
                    
                    return result;
                }
            );

            var validation = Task.Factory.ContinueWhenAll(
                new Task[] { validateStock, hasCredit },
                (tasks) => tasks.Cast<Task<bool>>().All((task) => task.Result)
            );

            if (!validation.Result)
                throw new Exception();

            var getDeliveryDate = Task.Factory.StartNew(() =>
                Delivery.GetDeliveryDate(invoice.CustomerId, invoice.ProductId, invoice.Quantity)
            );

            var validateDiscount = Task.Factory.StartNew(() =>
                CRM.ValidateDiscount(invoice.CustomerId, invoice.CustomerId, invoice.Quantity, invoice.Amount, 7m)
            );

            var saveInvoice = validateDiscount.ContinueWith(
                (task) => ERP.SaveInvoice(invoice.CustomerId, invoice.ProductId, invoice.Quantity, task.Result)
            );

            var newInvoice = Task.Factory.ContinueWhenAll(
                new Task[] { getDeliveryDate, validateDiscount },
                (task) => CRM.NewInvoice(invoice.CustomerId, invoice.ProductId, invoice.Quantity, getDeliveryDate.Result, validateDiscount.Result)
            );

            var requestDelivery = Task.Factory.ContinueWhenAll(
                new Task[] { getDeliveryDate, saveInvoice },
                (task) => Delivery.RequestDelivery(invoice.CustomerId, invoice.ProductId, invoice.Quantity, getDeliveryDate.Result)
            );

            Console.Write("*");

            requestDelivery.Wait();

            Console.WriteLine(Environment.TickCount - startTime);
            
            #endregion

            Console.ReadKey();
        }

        class Stock
        {
            public static bool HasStock(int productId, int qty)
            {
                Console.WriteLine("<HasStock>");
                Thread.Sleep(900);
                Console.WriteLine("</HasStock>");
                return true;
            }
        }
        class CRM
        {
            public static bool HasCredit(int customerId, decimal amount)
            {
                Console.WriteLine("<HasCredit>");
                Thread.Sleep(800);
                Console.WriteLine("</HasCredit>");
                return true;
            }
            public static decimal ValidateDiscount(int customerId, int productId, int qty, decimal amount, decimal discount)
            {
                Console.WriteLine("<ValidateDiscount>");
                Thread.Sleep(300);
                Console.WriteLine("</ValidateDiscount>");
                return discount;
            }
            public static void NewInvoice(int customerId, int productId, int qty, DateTime deliveryDate, decimal discount)
            {
                Console.WriteLine("<NewInvoice>");
                Thread.Sleep(800);
                Console.WriteLine("</NewInvoice>");
            }
        }
        class Delivery
        {
            public static DateTime GetDeliveryDate(int customerId, int productId, int qty)
            {
                Console.WriteLine("<GetDeliveryDate>");
                Thread.Sleep(300);
                Console.WriteLine("</GetDeliveryDate>");
                return DateTime.Now.AddDays(30);
            }
            public static void RequestDelivery(int customerId, int productId, int qty, DateTime deliveryDate)
            {
                Console.WriteLine("<RequestDelivery>");
                Thread.Sleep(800);
                Console.WriteLine("</RequestDelivery>");
            }
        }
        class ERP
        {
            public static void SaveInvoice(int customerId, int productId, int qty, decimal discount)
            {
                Console.WriteLine("<SaveInvoice>");
                Thread.Sleep(300);
                Console.WriteLine("</SaveInvoice>");
            }
        }
    }
}
