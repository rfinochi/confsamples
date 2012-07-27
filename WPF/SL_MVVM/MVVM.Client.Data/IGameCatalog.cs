using System.Collections.ObjectModel;
using MVVM.Client.Data.DataServices;
using System;

namespace MVVM.Client.Data
{
  public interface IGameCatalog
  {
    void GetGames();
    void GetGamesByGenre(string genre);
    void SaveChanges();

    event EventHandler<GameLoadingEventArgs> GameLoadingComplete;
    event EventHandler<GameCatalogErrorEventArgs> GameLoadingError;
    event EventHandler GameSavingComplete;
    event EventHandler<GameCatalogErrorEventArgs> GameSavingError;
  }
}
