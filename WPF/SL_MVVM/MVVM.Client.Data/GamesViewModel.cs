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
using System.Collections.ObjectModel;
using MVVM.Client.Data.DataServices;

namespace MVVM.Client.Data
{
  public class GamesViewModel
  {
    IGameCatalog theCatalog;
    ObservableCollection<Game> theGames = new ObservableCollection<Game>();

    public event EventHandler LoadComplete;
    public event EventHandler ErrorLoading;

   public GamesViewModel() : 
      this(new GameCatalog())
    {
    }

    public GamesViewModel(IGameCatalog catalog)
    {
      theCatalog = catalog;
      theCatalog.GameLoadingComplete += 
        new EventHandler<GameLoadingEventArgs>(games_GameLoadingComplete);
      theCatalog.GameLoadingError += 
        new EventHandler<GameCatalogErrorEventArgs>(games_GameLoadingError);
    }

    void games_GameLoadingError(object sender, GameCatalogErrorEventArgs e)
    {
      // Fire Event on UI Thread
      Application.Current.RootVisual.Dispatcher.BeginInvoke(() =>
        {
          if (ErrorLoading != null) ErrorLoading(this, null);
        });
    }

    void games_GameLoadingComplete(object sender, GameLoadingEventArgs e)
    {
      // Fire Event on UI Thread
      Application.Current.RootVisual.Dispatcher.BeginInvoke(() =>
        {
          // Clear the list
          theGames.Clear();

          // Add the new games
          foreach (Game g in e.Results) theGames.Add(g);

          if (LoadComplete != null) LoadComplete(this, null);
        });
    }


    public void LoadGames()
    {
      theCatalog.GetGames();
    }

    public void LoadGamesByGenre(string genre)
    {
      theCatalog.GetGamesByGenre(genre);
    }

    public ObservableCollection<Game> Games
    {
      get
      {
        return theGames;
      }
    }
  }
}
