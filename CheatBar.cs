using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace StickClassroom
{
    internal class CheatBar
    {
        private readonly int cheatBarX = Globals.WindowWidth - 50;
        private readonly int cheatBarY = 100;
        public float cheatBarFillPercentage = 0.0f;
        public bool cheatBarCanFill = false;
        private Rectangle outlineRect;
        private Rectangle fillRect;

        private Texture2D emptyTexture;

        private int fillHeight;
        private readonly int fillWidth;

        // Adjust the X and Y positions so the fill rect is properly inside the outline
        private readonly int fillX;
        private int fillY;

        public CheatBar(Texture2D texture)
        {
            this.emptyTexture = texture;

            // Define the cheat bar outer rect
            outlineRect = new Rectangle(cheatBarX - 25, cheatBarY - 40, 40, 730);  // Position, width, height

            fillWidth = outlineRect.Width - 4; // Subtracting 4 to keep space for the outline on both sides
            fillX = outlineRect.X + 2; // Offset X to center the fill rect
        }

        public void Update(MouseState mouseState)
        {
            // Fill the cheat bar up if needed
            if (mouseState.LeftButton == ButtonState.Pressed && this.cheatBarCanFill)
            {
                if (this.cheatBarFillPercentage <= 0.99f)
                {
                    this.cheatBarFillPercentage += 0.01f;
                }
                else // so, the cheatBar is full meaning, the player has won the level
                {
                    // Level complete!
                }
            }

            fillHeight = (int)((outlineRect.Height - 4) * cheatBarFillPercentage); // Subtracting 4 to keep space for top and bottom outline
            fillY = outlineRect.Y + 2 + (outlineRect.Height - 4 - fillHeight); // Offset Y to account for the top outline and filling
            fillRect = new(fillX, fillY, fillWidth, fillHeight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(emptyTexture, outlineRect, Color.Red);
            spriteBatch.Draw(emptyTexture, fillRect, Color.Green);
        }
    }
}
