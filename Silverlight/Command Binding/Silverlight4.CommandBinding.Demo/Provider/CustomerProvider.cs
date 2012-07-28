using System.Collections.ObjectModel;

using Silverlight4.CommandBinding.Demo.Model;

namespace Silverlight4.CommandBinding.Demo.Provider
{
    public class CustomerProvider
    {
        /// <summary>
        /// Loads the customers.
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<Customer> LoadCustomers( )
        {
            var customerCollection = new ObservableCollection<Customer>
                                        { 
                                            new Customer { Name="A. Hussain", Address="Mumbai", ContactNumber="833472873" },
                                            new Customer { Name="H. Singh", Address="Mumbai", ContactNumber="987352544" },
                                            new Customer { Name="P. Singh", Address="Pune", ContactNumber="988425111" },
                                            new Customer { Name="B. Wase", Address="Chennai", ContactNumber="988425111" },
                                            new Customer { Name="M. Pathak", Address="Delhi", ContactNumber="988425111" },
                                            new Customer { Name="R. Shikdar", Address="Kolkata", ContactNumber="988425111" },
                                            new Customer { Name="S. Sen", Address="Kolkata", ContactNumber="988425111" },
                                            new Customer { Name="R. Jana", Address="Mumbai", ContactNumber="988425111" },
                                            new Customer { Name="P. Majumder", Address="Bangalore", ContactNumber="988425111" }
                                        };

            return customerCollection;
        }
    }
}