using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace StickClassroom
{
    internal class Desk
    {
        public Vector2 Position {  get; private set; }
        private Texture2D texture;
        public Desk(int x, int y, Texture2D texture) 
        {
            Position = new Vector2(x, y);
            this.texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)Position.X, (int)Position.Y, 100, 40), Color.Brown);
        }
    }
}
