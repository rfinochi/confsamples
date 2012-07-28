using System.Collections.ObjectModel;
using System.Windows.Input;

using Silverlight4.CommandBinding.Demo.CommandBase;
using Silverlight4.CommandBinding.Demo.Model;
using Silverlight4.CommandBinding.Demo.Provider;

namespace Silverlight4.CommandBinding.Demo.ViewModel
{
    public class CustomerViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets or sets the load customers command.
        /// </summary>
        /// <value>The load customers command.</value>
        public ICommand LoadCustomersCommand { get; set; }

        private ObservableCollection<Customer> m_customerCollection;
        /// <summary>
        /// Gets or sets the customer collection.
        /// </summary>
        /// <value>The customer collection.</value>
        public ObservableCollection<Customer> CustomerCollection
        {
            get { return m_customerCollection; }
            set
            {
                if ( m_customerCollection != value )
                {
                    m_customerCollection = value;
                    OnPropertyChanged( "CustomerCollection" );
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerViewModel"/> class.
        /// </summary>
        public CustomerViewModel( )
        {
            LoadCustomersCommand = new DelegateCommand( LoadCustomers, CanLoadCustomers );
        }

        /// <summary>
        /// Loads the customers.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private void LoadCustomers( object parameter )
        {
            CustomerCollection = CustomerProvider.LoadCustomers( );
        }

        /// <summary>
        /// Determines whether this instance [can load customers] the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        /// 	<c>true</c> if this instance [can load customers] the specified parameter; 
        /// 	otherwise, <c>false</c>.
        /// </returns>
        private bool CanLoadCustomers( object parameter )
        {
            return true;
        }
    }
}