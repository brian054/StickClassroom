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
        private const int playerSpeed = 2; // possibly could be a boost or something
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
        public float nextDX;
        public float nextDY;

        public Player(int x, int y, Texture2D texture) 
        {
            this.texture = texture;
            this.Position = new Point(x, y);
        }

        public void Update(KeyboardState kb)
        {
            // Store next position deltas
            nextDX = 0;
            nextDY = 0;

            // Movement input handling
            if (kb.IsKeyDown(Keys.W))
            {
                nextDY = -playerSpeed; // Move up
            }
            if (kb.IsKeyDown(Keys.A))
            {
                nextDX = -playerSpeed; // Move left
            }
            if (kb.IsKeyDown(Keys.S))
            {
                nextDY = playerSpeed; // Move down
            }
            if (kb.IsKeyDown(Keys.D))
            {
                nextDX = playerSpeed; // Move right
            }

            // Update the player's position using the deltas
            Position = new Point(Position.X + (int)nextDX, Position.Y + (int)nextDY);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, playerRect, Color.Red);
        }
    }
}
