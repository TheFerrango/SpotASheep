using System;

namespace SpotASheep
{
  static class variables
  {
    static Random rand = new Random();

    public static int Rand
    {
      get { return variables.rand.Next(); }      
    }

    static string myId = "";

    static string protVers = "1.1";

    public static string ProtVers
    {
      get { return variables.protVers; }
    }

    public static string MyId
    {
      get { return variables.myId; }
      set { variables.myId = value; }
    }
  }

#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}

