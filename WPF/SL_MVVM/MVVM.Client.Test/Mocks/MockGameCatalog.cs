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
using MVVM.Client.Data;
using System.Windows.Threading;
using MVVM.Client.Data.DataServices;
using System.Linq;
using System.Collections.Generic;

namespace MVVM.Client.Test.Mocks
{

  public class MockGameCatalog : IGameCatalog
  {
    const int DELAY = 1000;

    public void GetGames()
    {
      DispatcherTimer timer =
        new DispatcherTimer()
            {
              Interval = TimeSpan.FromMilliseconds(DELAY)
            };

      timer.Tick += (s, e) =>
        {
          if (GameLoadingComplete != null)
          {
            GameLoadingComplete(this,
                                new GameLoadingEventArgs(MockData())
                               );
          }
        };
      timer.Start();

    }

    public void GetGamesByGenre(string genre)
    {
      DispatcherTimer timer =
        new DispatcherTimer()
        {
          Interval = TimeSpan.FromMilliseconds(DELAY)
        };

      timer.Tick += (s, e) =>
      {
        if (GameLoadingComplete != null)
        {
          IEnumerable<Game> filtered = 
            MockData().Where(g => g.Genre == genre).AsEnumerable();

          GameLoadingComplete(this,
                              new GameLoadingEventArgs(filtered)
                             );
        }
      };
      timer.Start();
    }

    public void SaveChanges()
    {
      throw new NotImplementedException();
    }

    IEnumerable<Game> MockData()
    {
      // Create a sample game list
      return new Game[]
      {
        new Game()
        {
          ProductName = "Some Game",
          Genre = "Family",
          Rating = "E (Everyone)",
          ReleaseDate = DateTime.Parse("04/04/2008")
        },
        new Game()
        {
          ProductName = "Some Violent Game",
          Genre = "Shooter",
          Rating = "M (Mature)",
          ReleaseDate = DateTime.Parse("04/04/2008")
        },
      };
    }

    public event EventHandler<GameLoadingEventArgs> GameLoadingComplete;
    public event EventHandler<GameCatalogErrorEventArgs> GameLoadingError;
    public event EventHandler GameSavingComplete;
    public event EventHandler<GameCatalogErrorEventArgs> GameSavingError;
  }
}
