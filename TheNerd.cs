using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace StickClassroom
{
    internal class TheNerd
    {
        public Rectangle nerdRect { get; private set; }
        public Rectangle nerdCopyZone { get; private set; }
        private Texture2D texture;

        //int padding = 15;
        //nerdRect = new Rectangle(600, 825, Globals.studentSize, Globals.studentSize); // 100, 195 ???
        //nerdCopyZone = new Rectangle(
        //    nerdRect.X - padding,
        //    nerdRect.Y - padding,
        //    nerdRect.Width + 2 * padding,
        //    nerdRect.Height + 2 * padding);

        private int size = Globals.studentSize;

        public TheNerd(int x, int y, Texture2D texture)
        {
            nerdRect = new Rectangle(x, y, size, size);
            this.texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, nerdRect, Color.Brown);
        }
    }
}
