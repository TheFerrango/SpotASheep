using System;
using System.Collections.Generic;
using System.Threading;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpotASheep
{
  class AboutScreen : GameScreen
  {
    #region Fields

    ContentManager content;

    Texture2D abo;
    SpriteBatch spriteBatch;
    SpriteFont sf;
    float pauseAlpha;
    List<string> c;
    #endregion

    #region Initialization


    /// <summary>
    /// Constructor.
    /// </summary>
    public AboutScreen()
    {
      TransitionOnTime = TimeSpan.FromSeconds(1.5);
      TransitionOffTime = TimeSpan.FromSeconds(0.5);
    }

    public void LoadDataForScreen()
    {
      spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);
      sf = content.Load<SpriteFont>("SpriteFont1");
      abo = content.Load<Texture2D>("about");
      c = new List<string>();
      c.Add("Spot A Sheep - version 1.5");
      c.Add("");
      c.Add("Game created by TheFerrango");
      c.Add("Textures by Dora");
      c.Add("");
      c.Add("Includes Newtonsoft's JSON.NET library");
      c.Add(""); 
      c.Add("You can contact me at ferrango@oggettone.com,");
      c.Add("or you can follow me on twitter, i'm Ferrango");
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
           
      
      base.Update(gameTime, otherScreenHasFocus, false);

      // Gradually fade in or out depending on whether we are covered by the pause screen.
      if (coveredByOtherScreen)
        pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
      else
        pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

      
      
    }


    protected void OnCancel()
    {
      LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                     new MainMenuScreen());      
    }

    #endregion
    

    #region Drawing functions

    /// <summary>
    /// Draws the gameplay screen.
    /// </summary>
    public override void Draw(GameTime gameTime)
    {
      // This game has a blue background. Why? Because!
      DrawAbo();
      Vector2 pos = new Vector2(0,100);
      if (c != null)
        for (int coop = 0; coop < c.Count; coop++)
        {
          DrawPoints(pos, c[coop]);
          pos.Y += 25;
        }      
    }

    public void DrawAbo()
    {
      spriteBatch.Begin();
      spriteBatch.Draw(abo, new Vector2((float)(ScreenManager.GraphicsDevice.DisplayMode.Height / 2 - abo.Width / 2), 0.0f), Color.DarkBlue);
      spriteBatch.End();
    }

    public void DrawPoints(Vector2 position, string text)
    {
      spriteBatch.Begin();      
      spriteBatch.DrawString(sf, text, position, Color.DarkBlue);
      spriteBatch.End();
    }

    #endregion
   
  }
}
