using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace StickClassroom
{
    internal class Desk
    {
        public Rectangle DeskRect { get; private set; }
        private Texture2D texture;

        private int deskWidth = 80;
        private int deskHeight = 40;

        public Desk(int x, int y, Texture2D texture) 
        {
            DeskRect = new Rectangle(x, y, deskWidth, deskHeight);
            this.texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, DeskRect, Color.Brown);
        }
    }
}
