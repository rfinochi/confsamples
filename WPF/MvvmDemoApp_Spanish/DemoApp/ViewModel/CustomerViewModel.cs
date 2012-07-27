using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;


using DemoApp.DataAccess;
using DemoApp.Model;
using DemoApp.Properties;

namespace DemoApp.ViewModel
{
	public class CustomerViewModel : WorkspaceViewModel, IDataErrorInfo
	{
		#region Fields

		private readonly Customer _customer;
		private readonly CustomerRepository _customerRepository;
		private string _customerType;
		private string[] _customerTypeOptions;
		private bool _isSelected;
		private RelayCommand _saveCommand;

		#endregion

		#region Constructor

		public CustomerViewModel( Customer customer, CustomerRepository customerRepository )
		{
			if ( customer == null )
				throw new ArgumentNullException( "customer" );

			if ( customerRepository == null )
				throw new ArgumentNullException( "customerRepository" );

			_customer = customer;
			_customerRepository = customerRepository;
			_customerType = Strings.CustomerViewModel_CustomerTypeOption_NotSpecified;

			if ( !IsNewCustomer )
			{
				if ( customer.IsCompany )
					CustomerType = Strings.CustomerViewModel_CustomerTypeOption_Company;
				else
					CustomerType = Strings.CustomerViewModel_CustomerTypeOption_Person;
			}
		}

		#endregion

		#region Customer Properties

		public string Email
		{
			get
			{
				return _customer.Email;
			}
			set
			{
				if ( value == _customer.Email )
					return;

				_customer.Email = value;

				base.OnPropertyChanged( "Email" );
			}
		}

		public string FirstName
		{
			get
			{
				return _customer.FirstName;
			}
			set
			{
				if ( value == _customer.FirstName )
					return;

				_customer.FirstName = value;

				base.OnPropertyChanged( "FirstName" );
			}
		}

		public bool IsCompany
		{
			get
			{
				return _customer.IsCompany;
			}
		}

		public string LastName
		{
			get
			{
				return _customer.LastName;
			}
			set
			{
				if ( value == _customer.LastName )
					return;

				_customer.LastName = value;

				base.OnPropertyChanged( "LastName" );
			}
		}

		public double TotalSales
		{
			get
			{
				return _customer.TotalSales;
			}
		}

		#endregion

		#region Presentation Properties

		public string CustomerType
		{
			get
			{
				return _customerType;
			}
			set
			{
				if ( value == _customerType || String.IsNullOrEmpty( value ) )
					return;

				_customerType = value;

				if ( _customerType == Strings.CustomerViewModel_CustomerTypeOption_Company )
				{
					_customer.IsCompany = true;
				}
				else if ( _customerType == Strings.CustomerViewModel_CustomerTypeOption_Person )
				{
					_customer.IsCompany = false;
				}

				base.OnPropertyChanged( "CustomerType" );

				base.OnPropertyChanged( "LastName" );
			}
		}

		public string[] CustomerTypeOptions
		{
			get
			{
				if ( _customerTypeOptions == null )
				{
					_customerTypeOptions = new string[]
                    {
                        Strings.CustomerViewModel_CustomerTypeOption_NotSpecified,
                        Strings.CustomerViewModel_CustomerTypeOption_Person,
                        Strings.CustomerViewModel_CustomerTypeOption_Company
                    };
				}
				return _customerTypeOptions;
			}
		}

		public override string DisplayName
		{
			get
			{
				if ( this.IsNewCustomer )
				{
					return Strings.CustomerViewModel_DisplayName;
				}
				else if ( _customer.IsCompany )
				{
					return _customer.FirstName;
				}
				else
				{
					return String.Format( "{0}, {1}", _customer.LastName, _customer.FirstName );
				}
			}
		}

		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}
			set
			{
				if ( value == _isSelected )
					return;

				_isSelected = value;

				base.OnPropertyChanged( "IsSelected" );
			}
		}

		public ICommand SaveCommand
		{
			get
			{
				if ( _saveCommand == null )
					_saveCommand = new RelayCommand( param => this.Save( ), param => this.CanSave );

				return _saveCommand;
			}
		}

		#endregion

		#region Public Methods

		public void Save( )
		{
			if ( !_customer.IsValid )
				throw new InvalidOperationException( Strings.CustomerViewModel_Exception_CannotSave );

			if ( this.IsNewCustomer )
				_customerRepository.AddCustomer( _customer );

			_customerRepository.Save( );

			base.OnPropertyChanged( "DisplayName" );
		}

		#endregion

		#region Private Helpers

		private bool IsNewCustomer
		{
			get
			{
				return !_customerRepository.ContainsCustomer( _customer );
			}
		}

		private bool CanSave
		{
			get
			{
				return String.IsNullOrEmpty( this.ValidateCustomerType( ) ) && _customer.IsValid;
			}
		}

		#endregion

		#region IDataErrorInfo Members

		string IDataErrorInfo.Error
		{
			get
			{
				return ( _customer as IDataErrorInfo ).Error;
			}
		}

		string IDataErrorInfo.this[ string propertyName ]
		{
			get
			{
				string error = null;

				if ( propertyName == "CustomerType" )
					error = this.ValidateCustomerType( );
				else
					error = ( _customer as IDataErrorInfo )[ propertyName ];

				CommandManager.InvalidateRequerySuggested( );

				return error;
			}
		}

		string ValidateCustomerType( )
		{
			if ( this.CustomerType == Strings.CustomerViewModel_CustomerTypeOption_Company ||
			   this.CustomerType == Strings.CustomerViewModel_CustomerTypeOption_Person )
				return null;

			return Strings.CustomerViewModel_Error_MissingCustomerType;
		}

		#endregion
	}
}