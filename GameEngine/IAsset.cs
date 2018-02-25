using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public interface IAsset
    {
        void Draw(SpriteBatch spriteBatch);
        void Offset(Vector2 translation);
        void setPos(Vector2 locn);
        void setTex(Texture2D tex);
        void Update();
        List<Vector2> Point();
        List<Vector2> Edges();
        Vector2 Center();
        Vector2 Velocity();
        Vector2 Position { get; set; }
        Texture2D Texture { get; set; }
        float Radius();
        void ApplyForce(Vector2 force);

    }
}
