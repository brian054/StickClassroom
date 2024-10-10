using Microsoft.Xna.Framework;
using System;

namespace StickClassroom
{
    internal class CollisionManager
    {
        private Rectangle[] collidables;

        public CollisionManager(Rectangle[] collidables) 
        {
            this.collidables = collidables;
        }

        public bool isCollidingRects(Rectangle playerRect, Rectangle copyZone) 
        {
            if (playerRect.Intersects(copyZone)) 
            {
                return true;    
            }
            return false;
        }

        public Rectangle GetCollidingRectangle(Rectangle playerRect)
        {
            for (int i = 0; i < collidables.Length; i++)
            {
                if (collidables[i].Intersects(playerRect))
                {
                    return collidables[i];
                }
            }
            return Rectangle.Empty;
        }
    }
}


