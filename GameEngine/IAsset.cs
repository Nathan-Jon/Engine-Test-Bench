using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace DemonstrationEngine
{
    public interface IAsset
    {
        void Draw(SpriteBatch spriteBatch);
        void Update();
        void BuildEdges();
        void SetPoints();
        List<Vector2> Point();
        List<Vector2> Edges();
        Vector2 Center();


        string Tag { get; set; }
        Vector2 Position { get; set; }
        Texture2D Texture { get; set; }

    }
}
