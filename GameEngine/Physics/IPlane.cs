using Microsoft.Xna.Framework;

namespace DemonstrationEngine.Physics
{
    public interface IPlane
    {
        Vector2 Normal { get; set; }
        float Offset { get; set; }
    }
}