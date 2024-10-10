using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Numerics;

namespace StickClassroom
{
    internal class Player
    {
        public int playerSpeed { get; set; } // possibly could be a boost or something
        public Point Position { get; set; }

        public Rectangle playerRect
        {
            get => new(Position, new Point(Globals.StudentSize, Globals.StudentSize));
            set
            {
                Position = new Point(value.X, value.Y);
            }
        }

        private Texture2D texture;

        // temp
        public int nextDX;
        public int nextDY;

        //CollisionManager collisionManager;

        public Player(int x, int y, Texture2D texture) 
        {
            this.texture = texture;
            this.Position = new Point(x, y);

            playerSpeed = 5;

            //collisionManager = new CollisionManager(); 
        }

        public void Update(KeyboardState kb, CollisionManager cm)
        {
            // Store next position deltas
            nextDX = 0;
            if (kb.IsKeyDown(Keys.A))
            {
                nextDX = -playerSpeed; // Move left
            }
            if (kb.IsKeyDown(Keys.D))
            {
                nextDX = playerSpeed; // Move right
            }
            Position = new Point(Position.X + nextDX, Position.Y);

            // Collision X
            if (!(cm.GetCollidingRectangle(playerRect).IsEmpty))
            {
                // Get the rectangle we are colliding with
                Rectangle collidingRect = cm.GetCollidingRectangle(playerRect);

                // Snap the player to the closest edge of the colliding rectangle
                if (nextDX > 0) // Moving right
                    Position = new Point(collidingRect.Left - playerRect.Width, Position.Y);
                else if (nextDX < 0) // Moving left
                    Position = new Point(collidingRect.Right, Position.Y);
            }

            nextDY = 0;
            if (kb.IsKeyDown(Keys.W))
            {
                nextDY = -playerSpeed; // Move up
            }
            if (kb.IsKeyDown(Keys.S))
            {
                nextDY = playerSpeed; // Move down
            }
            Position = new Point(Position.X, Position.Y + nextDY);

            // Collision Y
            if (!(cm.GetCollidingRectangle(playerRect).IsEmpty))
            {
                Rectangle collidingRect = cm.GetCollidingRectangle(playerRect);

                // Snap the player to the closest edge of the colliding rectangle
                if (nextDY > 0) // Moving down
                    Position = new Point(Position.X, collidingRect.Top - playerRect.Height);
                else if (nextDY < 0) // Moving up
                    Position = new Point(Position.X, collidingRect.Bottom);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, playerRect, Color.Red);
        }
    }
}
