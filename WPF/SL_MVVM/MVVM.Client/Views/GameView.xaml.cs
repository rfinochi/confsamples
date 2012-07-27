using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MVVM.Client.Data;
using System.Windows.Browser;

namespace MVVM.Client.Views
{
  public partial class GameView : UserControl
  {
    GamesViewModel viewModel = null;
    public GameView()
    {
      InitializeComponent();

      // Event Handlers
      Loaded += new RoutedEventHandler(Page_Loaded);
      genreComboBox.SelectionChanged += new SelectionChangedEventHandler(genreComboBox_SelectionChanged);

      // Wire up the View Model
      viewModel = Resources["TheViewModel"] as GamesViewModel;
      viewModel.ErrorLoading += new EventHandler(viewModel_ErrorLoading);
      viewModel.LoadComplete += new EventHandler(viewModel_LoadComplete);
    }

    void LoadGames(string genre)
    {
      loadingBar.Visibility = Visibility.Visible;
      if (genre == "(All)")
      {
        viewModel.LoadGames();
      }
      else
      {
        viewModel.LoadGamesByGenre(genre);
      }

    }

    void Page_Loaded(object sender, RoutedEventArgs e)
    {
      // Wire up a selection changed since Element Binding isn't supported yet
      gameListBox.SelectionChanged += (s, a) =>
        {
          detailGrid.DataContext = gameListBox.SelectedItem;
          detailGrid.Visibility = gameListBox.SelectedItem == null ?
                                    Visibility.Collapsed :
                                    Visibility.Visible;
        };

      // Setup the Genre Lists
      string[] genres = new string[] {
          "(All)",
          "Action", 
          "Adventure", 
          "Compilations", 
          "Family", 
          "Fighting", 
          "Music", 
          "Platform", 
          "Racing", 
          "RPG", 
          "Shooter", 
          "Simulation", 
          "Sports", 
          "Strategy", 
          "Xbox Originals", 
          "Xbox LIVE Arcade" 
          };
      genreComboBox.ItemsSource = genres;
      genreComboBox.SelectedIndex = 4;
    }

    void viewModel_LoadComplete(object sender, EventArgs e)
    {
      loadingBar.Visibility = Visibility.Collapsed;
    }

    void viewModel_ErrorLoading(object sender, EventArgs e)
    {
      loadingBar.Visibility = Visibility.Collapsed;
      HtmlPage.Window.Alert("Error occurred while loading data");
    }

    void genreComboBox_SelectionChanged(object sender,
                                        SelectionChangedEventArgs e)
    {
      string item = genreComboBox.SelectedItem as string;
      if (item != null)
      {
        LoadGames(item);
      }
    }

  }
}
