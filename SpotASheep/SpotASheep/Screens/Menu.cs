using System;
using Commons;
using GameStateManagement;
using Microsoft.Xna.Framework;

namespace SpotASheep
{
  class MainMenuScreen : PhoneMenuScreen
  {

    public MainMenuScreen()
      : base("Spot A Sheep")
    {
      Button playButton = new Button("New Game");
      playButton.Tapped += new EventHandler<EventArgs>(playButton_Tapped);
      MenuButtons.Add(playButton);

      Button punteggi = new Button("Highscores");
      punteggi.Tapped += new EventHandler<EventArgs>(punteggi_Tapped);
      MenuButtons.Add(punteggi);

      Button aboutMe = new Button("About");
      aboutMe.Tapped += new EventHandler<EventArgs>(aboutMe_Tapped);
      MenuButtons.Add(aboutMe);

      UserIDMethod();
    }
   
    private static void UserIDMethod()
    {
      IO.InitializeStorageAccess();
      if (IO.FileExists("SonIo"))
      {
        variables.MyId = IO.ReadFile("SonIo");
      }
      else
      {
        IO.WriteFile("SonIo", variables.Rand.ToString());
        variables.MyId = IO.ReadFile("SonIo");
      }
    }

    void aboutMe_Tapped(object sender, EventArgs e)
    {
      LoadingScreen.Load(ScreenManager, false, PlayerIndex.One, new BackgroundScreen(), new AboutScreen());
    }

    void punteggi_Tapped(object sender, EventArgs e)
    {
     LoadingScreen.Load(ScreenManager, false, PlayerIndex.One, new BackgroundScreen(), new ScoresScreen());
      //ScreenManager.AddScreen(new ScoresScreen(), PlayerIndex.One);
    }

    void playButton_Tapped(object sender, EventArgs e)
    {
      LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new ActualGameScreen());

    }

    protected override void OnCancel()
    {
      ScreenManager.Game.Exit();
      base.OnCancel();
    }

  }
}

