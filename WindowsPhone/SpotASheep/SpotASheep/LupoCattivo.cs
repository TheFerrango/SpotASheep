using Microsoft.Xna.Framework;

namespace SpotASheep
{
  class LupoCattivo
  {
    Vector2 _Coord;
    Vector2 _speed;

    public Vector2 Coord
    {
      get { return _Coord; }
      set { _Coord = value; }
    }

    public LupoCattivo()
    {
      _Coord = Vector2.Zero;
      _speed = new Vector2((float)40, (float)8);
    }

    public void UpdateLupo()
    {
      _Coord += _speed;
    }
  }
}
