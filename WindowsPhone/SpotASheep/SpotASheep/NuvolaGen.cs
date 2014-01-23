using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpotASheep
{
  public class NuvolaGen
  {
    Vector2 _position, _movement;
    Texture2D _texture;
    bool _touched;
    bool _isPecora;

    public bool IsPecora
    {
      get { return _isPecora; }
      set { _isPecora = value; }
    }
   
    public bool Touched
    {
      get { return _touched; }
      set { _touched = value; }
    }

    public Texture2D Texture
    {
      get { return _texture; }
      set { _texture = value; }
    }

    public Vector2 Position
    {
      get { return _position; }
      set { _position = value; }
    }

    public NuvolaGen(Texture2D txtr, float posY, float posX, float speed)
    {
      _position = new Vector2(posX,posY);
      _movement = new Vector2((float)-speed,0);
      _texture = txtr;
      _touched = false;
      _isPecora = false;
    }

    public NuvolaGen(Texture2D txtr, float posY, float posX, float speed, bool pecora) 
    {
      _position = new Vector2(posX, posY);
      _movement = new Vector2((float)-speed, 0);
      _touched = false;
     
      _texture = txtr;
      _isPecora = pecora;
    }

    public void Update()
    {
      _position += _movement;
    }

    public void ScappaDalLupoCattivo(int SuEGiu)
    {
      _movement *= 5;
      if(SuEGiu%2 ==0)
      _movement.Y = SuEGiu;
      else
        _movement.Y = -SuEGiu;
    }
  }
}
