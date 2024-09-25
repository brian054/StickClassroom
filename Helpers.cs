using Microsoft.Xna.Framework;

namespace StickClassroom
{
    internal class Helpers
    {
        // AABB collision detection method
        public static bool AABBCollision(Rectangle rect1, Rectangle rect2)
        {
            // AABB: check if the two rectangles intersect
            return rect1.X < rect2.X + rect2.Width &&
                   rect1.X + rect1.Width > rect2.X &&
                   rect1.Y < rect2.Y + rect2.Height &&
                   rect1.Y + rect1.Height > rect2.Y;
        }
    }
}
