using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace SpotASheep
{
  class Bonus
  {
    int value;

    public int Value
    {
      get { return this.value; }
      set { this.value = value; }
    } float momento;

    public float Momento
    {
      get { return momento; }
      set { momento = value; }
    }

    public Bonus(int val, float mom)
    {
      value = val;
      momento = mom;
    }
  }
  class ActualGameScreen : GameScreen
  {
    #region Fields

    ContentManager content;
    Bonus bonusPecora;
    bool attentiAlLupo;
    string input;
    TouchCollection _oldPanel;
    SpriteBatch spriteBatch;
    SpriteFont sf;
    LupoCattivo lc;
    SoundEffect boomAudio, achiAudio, wolfAudio;
    Texture2D nuvola, sfondo, boom, heart, lupo, perdente, achievement;
    Texture2D[] pecorelle;
    Color[] mappaNuvola;
    List<NuvolaGen> listaElementi;
    int pecore, nuvole, punti, vite, pecoreInFila, nextBonus;
    List<Esplosione> listaBoom;
    Random rand;

    float pauseAlpha;

    InputAction pauseAction;

    #endregion

    #region Initialization

    /// <summary>
    /// Constructor.
    /// </summary>
    public ActualGameScreen()
    {
      TransitionOnTime = TimeSpan.FromSeconds(1.5);
      TransitionOffTime = TimeSpan.FromSeconds(0.5);

      pauseAction = new InputAction(
          new Buttons[] { Buttons.Start, Buttons.Back },
          new Keys[] { Keys.Escape },
          true);
    }

    public void InizializzaGioco()
    {
      listaElementi = new List<NuvolaGen>();
      listaBoom = new List<Esplosione>();
      attentiAlLupo = false;
      punti = 0;
      pecore = 0;
      nuvole = 0;
      pecoreInFila = 0;
      nextBonus = 15;
      vite = 3;
      input = "";
      bonusPecora = null;
      pecorelle = new Texture2D[5];
      rand = new Random();
      _oldPanel = TouchPanel.GetState();
    }

    public void LoadDataForScreen()
    {

      spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);
      nuvola = content.Load<Texture2D>("nuvola");
      sf = content.Load<SpriteFont>("SpriteFont1");
      sfondo = content.Load<Texture2D>("genBackGround" + rand.Next(3).ToString());
      lupo = content.Load<Texture2D>("lupo");
      perdente = content.Load<Texture2D>("uLose");
      achievement = content.Load<Texture2D>("achi");
      pecorelle[0] = content.Load<Texture2D>("pecora1");
      pecorelle[1] = content.Load<Texture2D>("pecora2");
      pecorelle[2] = content.Load<Texture2D>("pecora3");
      pecorelle[3] = content.Load<Texture2D>("pecora4");
      pecorelle[4] = content.Load<Texture2D>("pecora5");
      boom = content.Load<Texture2D>("Explosion");
      boomAudio = content.Load<SoundEffect>("Bomb");
      achiAudio = content.Load<SoundEffect>("applause");
      wolfAudio = content.Load<SoundEffect>("wolves");
      heart = content.Load<Texture2D>("heart");
      mappaNuvola = new Color[nuvola.Height * nuvola.Width];
      nuvola.GetData(mappaNuvola);
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
        InizializzaGioco();
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

    public void killaPecora(GameTime t)
    {
      for (int i = 0; i < listaElementi.Count; i++)
        if (listaElementi[i].Touched && listaElementi[i].IsPecora)
        {
          AddExplosion(listaElementi[i].Position, 6, 5, t);
          listaElementi.RemoveAt(i);
          pecore--;
        }
    }

    public void UpdatePosElementi()
    {
      foreach (NuvolaGen ng in listaElementi)
      {
        ng.Update();
      }
    }

    public void RemoveUseless()
    {
      for (int i = listaElementi.Count - 1; i >= 0; i--)
      {
        if (listaElementi[i].Position.X < -200 || listaElementi[i].Touched)
        {
          if (listaElementi[i].IsPecora)
            pecore--;
          else
            nuvole--;
          listaElementi.RemoveAt(i);
        }
      }
    }

    public void UpdateLupoCattivo()
    {
      if (lc != null && (lc.Coord.X > 800 || lc.Coord.Y > 480))
      {
        lc = null;
        attentiAlLupo = false;
      }
      else
        lc.UpdateLupo();
    }

    private void UpdateParticles(GameTime gameTime)
    {
      float now = (float)gameTime.TotalGameTime.TotalMilliseconds;
      for (int i = listaBoom.Count - 1; i >= 0; i--)
      {
        Esplosione particle = listaBoom[i];
        // float timeAlive = now - particle.BirthTime;

        if (now > particle.AliveTime.TotalMilliseconds)
        {
          listaBoom.RemoveAt(i);
        }
        else
        {
          particle.Update();
          listaBoom[i] = particle;
        }
      }
    }

    public void UpdateBonus(GameTime gt)
    {
      if ((float)gt.TotalGameTime.TotalMilliseconds > bonusPecora.Momento)
        bonusPecora = null;
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
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed && !coveredByOtherScreen)
        ScreenManager.AddScreen(new PhonePauseScreen(), ControllingPlayer);

      base.Update(gameTime, otherScreenHasFocus, false);

      // Gradually fade in or out depending on whether we are covered by the pause screen.
      if (coveredByOtherScreen)
        pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
      else
        pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

      if (IsActive)
      {
        if (vite >= 0)
        {
          if (!attentiAlLupo)
            TouchInput(gameTime);
          killaPecora(gameTime);

          if (pecore < 2)
          {
            listaElementi.Add(new NuvolaGen(pecorelle[rand.Next(0, 5)], rand.Next(0, 9) * 48, rand.Next(606, 800), (float)(rand.NextDouble() * 5) + 1.0f + (float)punti / 100, true));
            pecore++;
          }
          if (nuvole < 5)
          {
            listaElementi.Add(new NuvolaGen(nuvola, rand.Next(0, 9) * 48, rand.Next(606, 800), (float)(rand.NextDouble() * 3) + 1f + (float)punti / 100));
            nuvole++;
          }
          if (bonusPecora != null)
            UpdateBonus(gameTime);
          UpdateParticles(gameTime);
          RemoveUseless();
          UpdatePosElementi();
          if (attentiAlLupo)
            UpdateLupoCattivo();
        }
        else
        {
          if (!Guide.IsVisible)
          {
            ShowKeyboard();
          }
        }
      }
    }

    #region Punteggio

    private void ShowKeyboard()
    {
      if (input == "")
        Guide.BeginShowKeyboardInput(PlayerIndex.One, "Submit score!", "Enter username to be associated to your score", "", delegate(IAsyncResult result) { input = Guide.EndShowKeyboardInput(result); SubmitYourScore(); }, null);
    }

    private void SubmitYourScore()
    {
      if (input != "")
      {
        if (NetworkInterface.GetIsNetworkAvailable())
        {
          try
          {
            WebClient wc = new WebClient();
            wc.UploadStringCompleted += new UploadStringCompletedEventHandler(wc_UploadStringCompleted);

            wc.Headers["Content-Type"] = "application/x-www-form-urlencoded, charset=utf-8";

            string os = Environment.OSVersion.Platform.ToString();
            string toPost = string.Format("Usr={0}&Punkts={1}&idPlayer={2}&PrtVer={3}&OSVersion={4}", input, punti, variables.MyId, variables.ProtVers, os);
            wc.UploadStringAsync(new Uri("http://ferrangosoft.altervista.org/ServerPecore/input.php"), toPost);
          }
          catch { }
        }
        else
        {
          LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                     new MainMenuScreen());
        }
      }
    }

    void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
    {
      LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                   new MainMenuScreen());
    }

    #endregion

    #endregion

    #region Input

    /// <summary>
    /// Lets the game respond to player input. Unlike the Update method,
    /// this will only be called when the gameplay screen is active.
    /// </summary>
    public void TouchInput(GameTime gt)
    {

      TouchCollection tc = TouchPanel.GetState();

      if (!attentiAlLupo)
        foreach (TouchLocation tl in tc)
        {
          if (tl.Position != Vector2.Zero && !ilVecchioContiene(tl))
          {
            NuvolaGen toRemovePecora;
            {
              toRemovePecora = IntersectElemento(tl);

              if (toRemovePecora != null)
              {
                int quale = listaElementi.IndexOf(toRemovePecora);
                listaElementi[quale].Touched = true;

                if (toRemovePecora.IsPecora)
                {
                  pecoreInFila++;
                  punti += 10;
                  boomAudio.Play();
                  if (pecoreInFila == nextBonus)
                  {
                    punti += nextBonus;
                    bonusPecora = new Bonus(nextBonus, (float)gt.TotalGameTime.TotalMilliseconds + 2500);
                    nextBonus *= 2;
                    achiAudio.Play();
                  }
                }
                else if (vite >= 0)
                {
                  vite--;
                  pecoreInFila = 0;
                  nextBonus = 15;
                  if (vite >= 0)
                  {
                    for (int i = 0; i < listaElementi.Count; i++)
                      if (listaElementi[i].IsPecora)
                        listaElementi[i].ScappaDalLupoCattivo(rand.Next(8, 11));
                    lc = new LupoCattivo();
                    wolfAudio.Play();
                    attentiAlLupo = true;
                  }
                }

              }
            }
          }
        }

      _oldPanel = tc;
    }

    //public void BackToMenu

    public bool ilVecchioContiene(TouchLocation tl)
    {
      foreach (TouchLocation tOld in _oldPanel)
        if (tOld.Position == tl.Position)
          return true;
      return false;
    }

    public NuvolaGen IntersectElemento(TouchLocation tl)
    {
      for (int i = listaElementi.Count - 1; i >= 0; i--)
      {
        Rectangle rettNuvola = new Rectangle((int)listaElementi[i].Position.X, (int)listaElementi[i].Position.Y, 194, 173);
        Rectangle rettTouch = new Rectangle((int)tl.Position.X, (int)tl.Position.Y, 1, 1);
        Color[] dataB = new Color[] { Color.Red };
        // Find the bounds of the rectangle intersection
        int top = Math.Max(rettNuvola.Top, rettTouch.Top);
        int bottom = Math.Min(rettNuvola.Bottom, rettTouch.Bottom);
        int left = Math.Max(rettNuvola.Left, rettTouch.Left);
        int right = Math.Min(rettNuvola.Right, rettTouch.Right);

        // Check every point within the intersection bounds
        for (int y = top; y < bottom; y++)
        {
          for (int x = left; x < right; x++)
          {
            // Get the color of both pixels at this point
            if (((x - rettNuvola.Left) + (y - rettNuvola.Top) * rettNuvola.Width) < mappaNuvola.Length)
            {
              Color colorA = mappaNuvola[(x - rettNuvola.Left) +
                                   (y - rettNuvola.Top) * rettNuvola.Width];
              Color colorB = dataB[(x - rettTouch.Left) +
                                   (y - rettTouch.Top) * rettTouch.Width];

              // If both pixels are not completely transparent,
              if (colorA.A != 0 && colorB.A != 0)
              {
                // then an intersection has been found
                //listaNuvole[i].Touched  = true;
                return listaElementi[i];
              }
            }
          }
        }

        // No intersection found

      }
      return null;
    }

    #endregion

    #region Drawing functions

    /// <summary>
    /// Draws the gameplay screen.
    /// </summary>
    public override void Draw(GameTime gameTime)
    {
      spriteBatch.Begin();
      spriteBatch.Draw(sfondo, new Rectangle(0, 0, 800, 480), Color.White);
      spriteBatch.End();
      if (vite >= 0)
      {
        DrawNuvole();
        DrawExplosione();
        if (attentiAlLupo)
          DrawLupoCattivo();
        DrawHearts();
        DrawPoints();
        if (bonusPecora != null)
          DrawBonus();
      }
      else
      {
        DrawHaHaHaiPerso();
      }


      // If the game is transitioning on or off, fade it out to black.
      if (TransitionPosition > 0 || pauseAlpha > 0)
      {
        float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);
        ScreenManager.FadeBackBufferToBlack(alpha);
      }
    }

    public void DrawNuvole()
    {
      spriteBatch.Begin();
      for (int i = 0; i < listaElementi.Count; i++)
      {
        //if (!ng.Touched)          
        spriteBatch.Draw(listaElementi[i].Texture, listaElementi[i].Position, Color.White);
      }
      spriteBatch.End();
    }

    public void DrawExplosione()
    {
      spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.Additive);

      for (int i = 0; i < listaBoom.Count; i++)
      {
        Esplosione particle = listaBoom[i];
        Vector2 coordEspl = particle.Coord;
        coordEspl.X += nuvola.Width / 2;
        coordEspl.Y += nuvola.Height / 2;
        spriteBatch.Draw(boom, coordEspl, null, Color.White, i, new Vector2(256, 256), 0.5f, SpriteEffects.None, 1);
      }
      spriteBatch.End();

    }

    public void DrawBonus()
    {
      spriteBatch.Begin();
      spriteBatch.Draw(achievement, new Rectangle(0, 0, 800, 45), Color.White);
      spriteBatch.DrawString(sf, " " + bonusPecora.Value.ToString() + " sheeps in a row! :)", new Vector2(50, 5), Color.Cyan);
      spriteBatch.End();
    }

    public void DrawHearts()
    {
      int posYStart = ScreenManager.GraphicsDevice.DisplayMode.Height - heart.Height * 3 - 9;
      spriteBatch.Begin();
      for (int i = 0; i < vite; i++)
      {
        spriteBatch.Draw(heart, new Vector2((float)posYStart, 4.0f), Color.Red);
        posYStart += heart.Width + 3;
      }
      spriteBatch.End();
    }

    public void DrawPoints()
    {
      spriteBatch.Begin();
      spriteBatch.DrawString(sf, "Score: " + punti.ToString(), Vector2.Zero, Color.Red);
      spriteBatch.End();
    }

    public void DrawLupoCattivo()
    {
      spriteBatch.Begin();
      spriteBatch.Draw(lupo, lc.Coord, Color.White);
      spriteBatch.End();
    }

    public void DrawHaHaHaiPerso()
    {
      spriteBatch.Begin();
      spriteBatch.Draw(perdente, new Vector2((float)(ScreenManager.GraphicsDevice.DisplayMode.Height / 2 - perdente.Width / 2), (float)(ScreenManager.GraphicsDevice.DisplayMode.Width - perdente.Height) / 2), Color.DarkBlue);
      spriteBatch.End();
    }

    #endregion

    #region others

    private void AddExplosion(Vector2 explosionPos, int numberOfParticles, float size, GameTime gameTime)
    {
      for (int i = 0; i < numberOfParticles; i++)
        AddExplosionParticle(explosionPos, size, gameTime);
    }

    private void AddExplosionParticle(Vector2 explosionPos, float explosionSize, GameTime gameTime)
    {
      Vector2 displacement = new Vector2((float)rand.NextDouble() * explosionSize, 0);
      displacement = Vector2.Transform(displacement, Matrix.CreateRotationZ(MathHelper.ToRadians(rand.Next(360))));

      Esplosione espl = new Esplosione(explosionPos, gameTime, displacement * 2.0f);
      listaBoom.Add(espl);
    }



    #endregion

  }
}
