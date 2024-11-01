using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace StickClassroom
{
    internal class Desk
    {
        public Rectangle DeskRect { get; private set; }
        public Rectangle StudentRect { get; private set; }
        private Texture2D texture;

        private int deskWidth = 80;
        private int deskHeight = 40;

        public Desk(int x, int y, Texture2D texture) 
        {
            DeskRect = new Rectangle(x, y, deskWidth, deskHeight);
            StudentRect = new Rectangle(x + 20, y + 20, 40, 40);
            this.texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, DeskRect, Color.Brown);
            spriteBatch.Draw(texture, StudentRect, Color.Brown);
        }
    }
}
