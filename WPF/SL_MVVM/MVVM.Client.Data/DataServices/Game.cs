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
  public partial class Game : DataServiceEntityBase
  {
    #region Partial Method INotifyPropertyChanged Implementation
    // Override the Changed partial methods to implement the 
    // INotifyPropertyChanged interface

    // This helps with the Model implementation to be a mostly
    // DataBound implementation

    partial void OnDeveloperChanged() { base.FirePropertyChanged("Developer"); }
    partial void OnGenreChanged() { base.FirePropertyChanged("Genre"); }
    partial void OnListPriceChanged() { base.FirePropertyChanged("ListPrice"); }
    partial void OnListPriceCurrencyChanged() { base.FirePropertyChanged("ListPriceCurrency"); }
    partial void OnPlayerInfoChanged() { base.FirePropertyChanged("PlayerInfo"); }
    partial void OnProductDescriptionChanged() { base.FirePropertyChanged("ProductDescription"); }
    partial void OnProductIDChanged() { base.FirePropertyChanged("ProductID"); }
    partial void OnProductImageUrlChanged() { base.FirePropertyChanged("ProductImageUrl"); }
    partial void OnProductNameChanged() { base.FirePropertyChanged("ProductName"); }
    partial void OnProductTypeIDChanged() { base.FirePropertyChanged("ProductTypeID"); }
    partial void OnPublisherChanged() { base.FirePropertyChanged("Publisher"); }
    partial void OnRatingChanged() { base.FirePropertyChanged("Rating"); }
    partial void OnRatingUrlChanged() { base.FirePropertyChanged("RatingUrl"); }
    partial void OnReleaseDateChanged() { base.FirePropertyChanged("ReleaseDate"); }
    partial void OnSystemNameChanged() { base.FirePropertyChanged("SystemName"); }
    #endregion
  }
}
