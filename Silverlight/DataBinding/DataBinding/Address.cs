using System.ComponentModel;

namespace DataBinding_Address
{
    public class Address : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _name = "";
        private string _address1 = "";
        private string _address2 = "";
        private string _city = "";
        private string _state = "";
        private string _zipcode = "";

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }

        public string Address1
        {
            get
            {
                return _address1;
            }
            set
            {
                _address1 = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Address1"));
                }
            }
        }

        public string Address2
        {
            get
            {
                return _address2;
            }
            set
            {
                _address2 = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Address2"));
                }
            }
        }

        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("City"));
                }
            }
        }

        public string State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("State"));
                }
            }
        }

        public string Zipcode
        {
            get
            {
                return _zipcode;
            }
            set
            {
                _zipcode = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Zipcode"));
                }
            }
        }
    }
}
