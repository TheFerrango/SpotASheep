using System;
using Microsoft.Xna.Framework;

namespace SpotASheep
{
  class Esplosione
  {
    Vector2 _Coord;


    public Vector2 Coord
    {
      get { return _Coord; }
      set { _Coord = value; }
    }
    Vector2 _Direzione;
    Vector2 _speed;
    TimeSpan _AliveTime;

    public TimeSpan AliveTime
    {
      get { return _AliveTime; }
      set { _AliveTime = value; }
    }

    public Esplosione(Vector2 Coords, GameTime BornTime, Vector2 Direction)
    {
      _Coord = Coords;
      _AliveTime = BornTime.TotalGameTime + new TimeSpan(0, 0, 2);
      
      _Direzione = Direction;
      _speed = -Direction;
    }

    public void Update()
    {
      _Coord += _speed;
    }
  }
}
