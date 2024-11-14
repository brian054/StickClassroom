using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace StickClassroom
{
    internal class TheNerd
    {
        public Rectangle NerdRect { get; private set; }
        public Rectangle NerdCopyZone { get; private set; }
        private readonly Texture2D texture;
        private readonly int size = Globals.StudentSize;

        public TheNerd(int x, int y, Texture2D texture) 
        {
            //NerdRect = new Rectangle(x, y, size, size);

            //// Copy Zone
            //int padding = 15;
            //NerdCopyZone = new Rectangle(
            //    x - padding,
            //    y - padding,
            //    size + 2 * padding,
            //    size + 2 * padding);

            //this.texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, NerdCopyZone, Color.Yellow);
            spriteBatch.Draw(texture, NerdRect, Color.Blue);
        }
    }
}
