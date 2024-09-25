using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

// test commit

namespace StickClassroom
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        Texture2D rectTexture;
        Texture2D deskTexture;

        Desk[,] desks;

        // Desk grid size
        int deskRows = 4;
        int deskCols = 6;

        // Player position and speed
        Vector2 playerPosition;
        const float playerSpeed = 5f;
        float dx = 0;
        float dy = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            playerPosition = new Vector2(655, 825);

            // Set Window Size
            graphics.PreferredBackBufferWidth = 800;  // Width of the window
            graphics.PreferredBackBufferHeight = 900; // Height of the window
            graphics.ApplyChanges(); // Apply the changes to the window size

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            rectTexture = new Texture2D(GraphicsDevice, 1, 1);
            rectTexture.SetData(new[] { Color.White });

            deskTexture = new Texture2D(GraphicsDevice, 1, 1);
            deskTexture.SetData(new[] { Color.White });

            desks = new Desk[deskRows, deskCols];
            for (int i = 0; i < deskRows; i++)
            {
                for (int j = 0; j < deskCols; j++)
                {
                    // Calculate desk positions
                    int x = i * 180 + 100;
                    int y = j * 120 + 170;

                    // Create and place a new desk at the calculated position
                    desks[i, j] = new Desk(x, y, deskTexture);
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Get the current keyboard state
            KeyboardState state = Keyboard.GetState();

            // Reset dx and dy
            dx = 0;
            dy = 0;

            // Check for movement input
            if (state.IsKeyDown(Keys.W))
            {
                dy = -playerSpeed; // Move up
            }
            if (state.IsKeyDown(Keys.A))
            {
                dx = -playerSpeed; // Move left
            }
            if (state.IsKeyDown(Keys.S))
            {
                dy = playerSpeed; // Move down
            }
            if (state.IsKeyDown(Keys.D))
            {
                dx = playerSpeed; // Move right
            }


            playerPosition.X += dx;
            playerPosition.Y += dy;

            // Print the player's current position to the Debug window
            System.Diagnostics.Debug.WriteLine($"Player Position: X = {playerPosition.X}, Y = {playerPosition.Y}");

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            Texture2D rectTexture = new Texture2D(GraphicsDevice, 1, 1);
            rectTexture.SetData(new[] { Color.White });
            spriteBatch.Draw(rectTexture, new Rectangle((int)playerPosition.X, (int)playerPosition.Y, 40, 40), Color.Red);

            // Draw desks
            for (int i = 0; i < deskRows; i++)
            {
                for (int j = 0; j < deskCols; j++)
                {
                    desks[i, j].Draw(spriteBatch);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
