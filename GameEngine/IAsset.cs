using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace DemonstrationEngine
{
    public interface IAsset
    {
        void Draw(SpriteBatch spriteBatch);
        void Offset(Vector2 translation);
        void setPos(Vector2 locn);
        void setTex(Texture2D tex);
        void Update();
        string getTag();
        void ApplyForce(Vector2 force);
        void ApplyImpulse(Vector2 closingVelo);

        List<Vector2> Point();
        List<Vector2> Edges();
        Vector2 Center();
        Vector2 Velocity();
        Vector2 Position { get; set; }
        Texture2D Texture { get; set; }
        void SetGravity(bool val);

    }
}
