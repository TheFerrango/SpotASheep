﻿#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameStateManagement;
#endregion

namespace SpotASheep
{
  class BackgroundScreen : GameScreen
  {
    Texture2D backgroundTexture;
    ContentManager content;

    public BackgroundScreen()
    {
      TransitionOnTime = TimeSpan.FromSeconds(0.5);
      TransitionOffTime = TimeSpan.FromSeconds(0.5);
    }

    public override void Activate(bool instancePreserved)
    {
      if (!instancePreserved)
      {
        if (content == null)
          content = new ContentManager(ScreenManager.Game.Services, "Content");
        backgroundTexture = content.Load<Texture2D>("genMenuBg");       
      }
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
      base.Update(gameTime, otherScreenHasFocus, false);
    }

    public override void Unload()
    {
      content.Unload();
    }

    public override void Draw(GameTime gameTime)
    {
      SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
      Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
      Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

      spriteBatch.Begin();

      spriteBatch.Draw(backgroundTexture, fullscreen,
                       new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

      spriteBatch.End();
    }
  }
}
