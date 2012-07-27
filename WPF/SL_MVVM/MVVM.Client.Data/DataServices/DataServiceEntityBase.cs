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
using System.ComponentModel;

namespace MVVM.Client.Data.DataServices
{
  // COPIED FROM SILVERLIGHTCONTRIB Project for simplicity

  /// <summary>
  /// Base class for DataService Data Contract classes to implement 
  /// base functionality that is needed like INotifyPropertyChanged.  
  /// Add the base class in the partial class to add the implementation.
  /// </summary>
  public abstract class DataServiceEntityBase : INotifyPropertyChanged
  {
    /// <summary>
    /// The handler for the registrants of the interface's event 
    /// </summary>
    PropertyChangedEventHandler _propertyChangedHandler;

    /// <summary>
    /// Allow inheritors to fire the event more simply.
    /// </summary>
    /// <param name="propertyName"></param>
    protected void FirePropertyChanged(string propertyName)
    {
      if (_propertyChangedHandler != null)
      {
        _propertyChangedHandler(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    #region INotifyPropertyChanged Members
    /// <summary>
    /// The interface used to notify changes on the entity.
    /// </summary>
    event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
    {
      add
      {
        _propertyChangedHandler += value;
      }
      remove
      {
        _propertyChangedHandler -= value;
      }
    }
    #endregion
  }
}
