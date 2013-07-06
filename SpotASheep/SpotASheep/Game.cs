using System;
using GameStateManagement;
using Microsoft.Xna.Framework;

namespace SpotASheep
{
  /// <summary>
  /// This is the main type for your game
  /// </summary>
  public class Game : Microsoft.Xna.Framework.Game
  {
    GraphicsDeviceManager graphics;
    ScreenManager screenManager;
    ScreenFactory screenFactory;

    public Game()
    {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      graphics.IsFullScreen = true;
      InitializeLandscapeGraphics();
      // Frame rate is 30 fps by default for Windows Phone.
      TargetElapsedTime = TimeSpan.FromTicks(333333);

      // Extend battery life under lock.
      InactiveSleepTime = TimeSpan.FromSeconds(1);

      screenFactory = new ScreenFactory();
      Services.AddService(typeof(IScreenFactory), screenFactory);

      screenManager = new ScreenManager(this);
      Components.Add(screenManager);

      Microsoft.Phone.Shell.PhoneApplicationService.Current.Launching +=
                new EventHandler<Microsoft.Phone.Shell.LaunchingEventArgs>(GameLaunching);
      Microsoft.Phone.Shell.PhoneApplicationService.Current.Activated +=
          new EventHandler<Microsoft.Phone.Shell.ActivatedEventArgs>(GameActivated);
      Microsoft.Phone.Shell.PhoneApplicationService.Current.Deactivated +=
          new EventHandler<Microsoft.Phone.Shell.DeactivatedEventArgs>(GameDeactivated);
    }


    private void InitializeLandscapeGraphics()
    {
      graphics.PreferredBackBufferWidth = 800;
      graphics.PreferredBackBufferHeight = 480;
    }

    private void AddInitialScreens()
    {
      // Activate the first screens.
      screenManager.AddScreen(new BackgroundScreen(), null);

      // We have different menus for Windows Phone to take advantage of the touch interface
      //#if WINDOWS_PHONE
      screenManager.AddScreen(new MainMenuScreen(), null);
    }

    void GameLaunching(object sender, Microsoft.Phone.Shell.LaunchingEventArgs e)
    {
      AddInitialScreens();
    }

    void GameActivated(object sender, Microsoft.Phone.Shell.ActivatedEventArgs e)
    {
      // Try to deserialize the screen manager
      if (!screenManager.Activate(e.IsApplicationInstancePreserved))
      {
        // If the screen manager fails to deserialize, add the initial screens
        AddInitialScreens();
      }
    }

    void GameDeactivated(object sender, Microsoft.Phone.Shell.DeactivatedEventArgs e)
    {
      // Serialize the screen manager when the game deactivated
      screenManager.Deactivate();
    }

    #region System-generated functions

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
      // TODO: Add your initialization logic here

      base.Initialize();

    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
      // Create a new SpriteBatch, which can be used to draw textures.
     
      // TODO: use this.Content to load your game content here
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// all content.
    /// </summary>
    protected override void UnloadContent()
    {
      // TODO: Unload any non ContentManager content here
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    /*protected override void Update(GameTime gameTime)
    {
      // Allows the game to exit
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
      {
        this.Exit();
      }

      base.Update(gameTime);
    }*/

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {

      // TODO: Add your drawing code here

      base.Draw(gameTime);
    }

    #endregion
  }

}
