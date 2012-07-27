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
using Microsoft.Silverlight.Testing.UnitTesting.Metadata.VisualStudio;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVM.Client.Data;
using MVVM.Client.Test.Mocks;
using System.Diagnostics;

namespace MVVM.Client.Test
{
  [TestClass]
  public class GameModelTests : SilverlightTest
  {
    bool loadComplete = false;
    GamesViewModel viewModel = new GamesViewModel(new MockGameCatalog());

    [TestInitialize]
    public void TestInit()
    {
      viewModel.LoadComplete += (s, e) =>
      {
        loadComplete = true;
      };
    }
    
    [TestMethod]
    [Description("Test LoadGames from the ViewModel")]
    [Asynchronous]
    public void TestLoadGames()
    {

      viewModel.LoadGames();

      EnqueueConditional(() => loadComplete);

      EnqueueCallback(() =>
        {
          Assert.AreNotEqual(viewModel.Games, null, "Expected games list not to be null.");
        });

      EnqueueCallback(() =>
        {
          Assert.IsTrue(viewModel.Games.Count > 0, "Expected games list have results.");
        });

      EnqueueTestComplete();
    }

    [TestMethod]
    [Description("Test Successful LoadGamesByGenre from the ViewModel")]
    [Asynchronous]
    public void TestLoadGamesByGenre()
    {
      bool loadComplete = false;

      GamesViewModel viewModel = new GamesViewModel(new MockGameCatalog());
      viewModel.LoadComplete += (s, e) =>
      {
        loadComplete = true;
      };
      viewModel.LoadGamesByGenre("Family");

      EnqueueConditional(() => loadComplete);

      EnqueueCallback(() =>
      {
        Assert.AreNotEqual(viewModel.Games, null, "Expected games list not to be null.");
      });

      EnqueueCallback(() =>
      {
        Assert.IsTrue(viewModel.Games.Count > 0, "Expected games list have results.");
      });

      EnqueueTestComplete();
    }

    [TestMethod]
    [Description("Test Failed LoadGamesByGenre from the ViewModel")]
    [Asynchronous]
    public void TestLoadGamesByGenreWithoutResults()
    {
      bool loadComplete = false;

      GamesViewModel viewModel = new GamesViewModel(new MockGameCatalog());
      viewModel.LoadComplete += (s, e) =>
      {
        loadComplete = true;
      };
      viewModel.LoadGamesByGenre("Mature"); // Non-existent Genre

      EnqueueConditional(() => loadComplete);

      EnqueueCallback(() =>
      {
        Assert.AreNotEqual(viewModel.Games, null, "Expected games list not to be null.");
      });

      EnqueueCallback(() =>
      {
        Assert.IsTrue(viewModel.Games.Count == 0, "Expected games list have no results.");
      });

      EnqueueTestComplete();
    }
  }
}
