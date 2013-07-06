using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace SpotASheep
{
  class ScoresScreen : GameScreen
  {
    #region Fields

    const int mostrati = 14;

    ContentManager content;
    int inizio;
    bool todayTop;
    Random rand;
    Texture2D high, uparrow, downarrow, topCloud, bigCloud;
    List<string[]> c;
    SpriteBatch spriteBatch;
    SpriteFont sf;
    TouchCollection tc;
    WebClient wc;
    float pauseAlpha;
    
    #endregion

    #region Initialization


    /// <summary>
    /// Constructor.
    /// </summary>
    public ScoresScreen()
    {
      TransitionOnTime = TimeSpan.FromSeconds(1.5);
      TransitionOffTime = TimeSpan.FromSeconds(0.5);
    }

    public void LoadDataForScreen()
    {
      spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);
      sf = content.Load<SpriteFont>("SpriteFont1");
      high = content.Load<Texture2D>("HighScores");
      topCloud = content.Load<Texture2D>("cloudTop");
      bigCloud = content.Load<Texture2D>("cloudBig");
      uparrow = content.Load<Texture2D>("upArrow");
      downarrow = content.Load<Texture2D>("downArrow");
      tc = TouchPanel.GetState();
      wc = new WebClient();
      todayTop = true;
      rand = new Random();
      wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
      inizio = 0;
      GetHighScores();

    }

    /// <summary>
    /// Load graphics content for the game.
    /// </summary>
    public override void Activate(bool instancePreserved)
    {

      if (!instancePreserved)
      {

        if (content == null)
          content = new ContentManager(ScreenManager.Game.Services, "Content");
       
        // gameFont = content.Load<SpriteFont>("gamefont");
        LoadDataForScreen();
        // A real game would probably have more content than this sample, so
        // it would take longer to load. We simulate that by delaying for a
        // while, giving you a chance to admire the beautiful loading screen.
        Thread.Sleep(1000);

        // once the load has finished, we use ResetElapsedTime to tell the game's
        // timing mechanism that we have just finished a very long frame, and that
        // it should not try to catch up.


        ScreenManager.Game.ResetElapsedTime();
      }


      if (Microsoft.Phone.Shell.PhoneApplicationService.Current.State.ContainsKey("PlayerPosition"))
      {
        // playerPosition = (Vector2)Microsoft.Phone.Shell.PhoneApplicationService.Current.State["PlayerPosition"];
        //enemyPosition = (Vector2)Microsoft.Phone.Shell.PhoneApplicationService.Current.State["EnemyPosition"];
      }
    }


    public override void Deactivate()
    {
      // Microsoft.Phone.Shell.PhoneApplicationService.Current.State["PlayerPosition"] = playerPosition;
      //Microsoft.Phone.Shell.PhoneApplicationService.Current.State["EnemyPosition"] = enemyPosition;
      base.Deactivate();
    }

    /// <summary>
    /// Unload graphics content used by the game.
    /// </summary>
    public override void Unload()
    {
      content.Unload();
      //Microsoft.Phone.Shell.PhoneApplicationService.Current.State.Remove("PlayerPosition");
      //Microsoft.Phone.Shell.PhoneApplicationService.Current.State.Remove("EnemyPosition");
    }

    #endregion

    #region Update calls for objects

    
    #endregion

    #region Update 


    /// <summary>
    /// Updates the state of the game. This method checks the GameScreen.IsActive
    /// property, so the game will stop updating when the pause menu is active,
    /// or if you tab away to a different application.
    /// </summary>
    public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                   bool coveredByOtherScreen)
    {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
      {
        OnCancel();
      }

      if(c!= null)
        TouchInput();      
      
      base.Update(gameTime, otherScreenHasFocus, false);

      // Gradually fade in or out depending on whether we are covered by the pause screen.
      if (coveredByOtherScreen)
        pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
      else
        pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);     
      
    }

    public bool ilVecchioContiene(TouchLocation tl)
    {
      foreach (TouchLocation tOld in tc)
        if (tOld.Position == tl.Position)
          return true;
      return false;
    }

    protected void OnCancel()
    {
      LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                     new MainMenuScreen());      
    }

    private void GetHighScores()
    {
      if (NetworkInterface.GetIsNetworkAvailable() && !wc.IsBusy)
      {        
        wc.DownloadStringAsync(new Uri("http://ferrangosoft.altervista.org/ServerPecore/index.php?ISNEW=TRUE&id=" + rand.Next()));
      }
      else { c = new List<string[]>(); c.Add(new string[] { "No network avaible.", "Unable to fetch scores" }); }
    }

    private void GetBESTScores()
    {
      if (NetworkInterface.GetIsNetworkAvailable() && !wc.IsBusy)
      {
        wc.DownloadStringAsync(new Uri("http://ferrangosoft.altervista.org/ServerPecore/index.php?ISNEW=TRUE&BEST=TRUE&id=" + rand.Next()));
      }
      else { c = new List<string[]>(); c.Add(new string[] { "No network avaible.", "Unable to fetch scores" }); }
    }

    void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
    {
      inizio = 0;
      c = Newtonsoft.Json.JsonConvert.DeserializeObject <List<string[]>>(e.Result);      
    }


    #endregion
    
    #region Input

    /// <summary>
    /// Lets the game respond to player input. Unlike the Update method,
    /// this will only be called when the gameplay screen is active.
    /// </summary>
    public void TouchInput()
    {
      TouchCollection colle = TouchPanel.GetState();
      foreach (TouchLocation tl in colle)
      {
        if (tl.Position.X >= ScreenManager.GraphicsDevice.DisplayMode.Height - uparrow.Width && !ilVecchioContiene(tl))
        {
          if (tl.Position.Y < uparrow.Width+75 && tl.Position.Y > 75 )
          {
            if (inizio > 0)
              inizio--;
          }
          else if(tl.Position.Y >ScreenManager.GraphicsDevice.DisplayMode.Width - uparrow.Width)
          {
            if (inizio < c.Count - mostrati)
            {
              inizio++;
            }
          }
        }
        else if (tl.Position.Y < 66)
        {
          if (tl.Position.X <= 220 && tl.Position.X >= 30)
          {
            GetBESTScores();
            todayTop = false;
          }
          else if (tl.Position.X <= 675 && tl.Position.X >= 325)
          {
            GetHighScores();
            todayTop = true;
          }
        }

      }
    }

    #endregion

    #region Drawing functions

    /// <summary>
    /// Draws the gameplay screen.
    /// </summary>
    public override void Draw(GameTime gameTime)
    {
     
      DrawCloud();
      DrawHigh();
      DrawArrows();
      Vector2 pos = new Vector2(5,90);
      if (c != null)
        for (int coop = inizio; coop < c.Count && coop - inizio < mostrati; coop++)
        {
          DrawPoints(pos, c[coop][0], c[coop][1]);
          pos.Y += 25;
        }      
    }

    public void DrawHigh()
    {
      spriteBatch.Begin();
      spriteBatch.Draw(high, new Vector2((float)(ScreenManager.GraphicsDevice.DisplayMode.Height / 2 - high.Width / 2), 0.0f), Color.DarkBlue);
      spriteBatch.End();
    }

    public void DrawPoints(Vector2 position, string user, string points)
    {
      spriteBatch.Begin();
      spriteBatch.DrawString(sf, string.Format("{0}: ", user), position, Color.DarkBlue);
      spriteBatch.DrawString(sf, string.Format("{0,-7}", points), new Vector2(position.X + 300.0f, position.Y), Color.DarkBlue);
      spriteBatch.End();
    }

    public void DrawArrows()
    {
      spriteBatch.Begin();
      spriteBatch.Draw(uparrow, new Vector2((float)(ScreenManager.GraphicsDevice.DisplayMode.Height - uparrow.Width), 75f), Color.Blue);
      spriteBatch.Draw(downarrow, new Vector2((float)(ScreenManager.GraphicsDevice.DisplayMode.Height - downarrow.Width), (float)(ScreenManager.GraphicsDevice.DisplayMode.Width - downarrow.Width)), Color.Blue);
      spriteBatch.End();
    }

    public void DrawCloud()
    {
      spriteBatch.Begin();
      if(!todayTop)
        spriteBatch.Draw(topCloud, new Vector2((float)(ScreenManager.GraphicsDevice.DisplayMode.Height / 2 - 395), 2.0f), Color.White);
      else
        spriteBatch.Draw(bigCloud, new Vector2((float)(ScreenManager.GraphicsDevice.DisplayMode.Height / 2 - 145), 2.0f), Color.White);
      spriteBatch.End();

    }

    #endregion
   
  }
}
