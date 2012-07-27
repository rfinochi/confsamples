using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MVVM.Client.Data.DataServices
{
  // Partial Method to support the INotifyPropertyChanged interface
  public partial class Supplier : DataServiceEntityBase
  {
    #region Partial Method INotifyPropertyChanged Implementation
    // Override the Changed partial methods to implement the 
    // INotifyPropertyChanged interface

    // This helps with the Model implementation to be a mostly
    // DataBound implementation

    partial void OnSupplierAddressChanged() { base.FirePropertyChanged("SupplierAddress"); }
    partial void OnSupplierIDChanged() { base.FirePropertyChanged("SupplierID"); }
    partial void OnSupplierNameChanged() { base.FirePropertyChanged("SupplierName"); }
    partial void OnSupplierPhoneChanged() { base.FirePropertyChanged("SupplierPhone"); }
    
    #endregion
  }
}
