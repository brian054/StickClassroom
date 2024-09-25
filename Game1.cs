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

        // Player object - abstract this out
        Rectangle playerRect;
        int playerSize = 40;
        const int playerSpeed = 5;
        int playerDX = 0;
        int playerDY = 0;
        bool playerCollision = false;
        Rectangle nextPlayerPositionRect;

        //int cheatBarX = Globals.WindowWidth - 50;
        //int cheatBarY = 100;
        //int cheatBarWidth = 35;
        //int cheatBarHeight = Globals.WindowHeight - 160;
        //int cheatBarFillY = cheatBarY + cheatBarHeight;
        //int cheatBarFillHeight = 10;
        //bool cheatBarFilling = false;
        //int growthRateCheatBar = 1;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            playerRect = new Rectangle(655, 825, playerSize, playerSize);
            nextPlayerPositionRect = new Rectangle(655, 825, playerSize, playerSize); // might not need this


            // Set Window Size
            graphics.PreferredBackBufferWidth = Globals.WindowWidth;  
            graphics.PreferredBackBufferHeight = Globals.WindowHeight;
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
                    int x = i * 158 + 80;
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

            // Store next position deltas
            float nextDX = 0;
            float nextDY = 0;

            // Movement input handling
            if (state.IsKeyDown(Keys.W))
            {
                nextDY = -playerSpeed; // Move up
            }
            if (state.IsKeyDown(Keys.A))
            {
                nextDX = -playerSpeed; // Move left
            }
            if (state.IsKeyDown(Keys.S))
            {
                nextDY = playerSpeed; // Move down
            }
            if (state.IsKeyDown(Keys.D))
            {
                nextDX = playerSpeed; // Move right
            }

            // Handle movement in X direction first
            bool collisionDetectedX = false;
            Rectangle nextXPositionRect = playerRect;
            nextXPositionRect.X += (int)nextDX;

            for (int i = 0; i < deskRows; i++)
            {
                for (int j = 0; j < deskCols; j++)
                {
                    if (nextXPositionRect.Intersects(desks[i, j].DeskRect))
                    {
                        collisionDetectedX = true;

                        // Stop horizontal movement by snapping to edge of desk
                        if (nextDX > 0) // Moving right
                        {
                            playerRect.X = desks[i, j].DeskRect.Left - playerRect.Width;
                        }
                        else if (nextDX < 0) // Moving left
                        {
                            playerRect.X = desks[i, j].DeskRect.Right;
                        }
                        break;
                    }
                }
            }

            // If no X-axis collision was detected, update player's X position
            if (!collisionDetectedX)
            {
                playerRect.X += (int)nextDX;
            }

            // Handle movement in Y direction separately
            bool collisionDetectedY = false;
            Rectangle nextYPositionRect = playerRect;
            nextYPositionRect.Y += (int)nextDY;

            for (int i = 0; i < deskRows; i++)
            {
                for (int j = 0; j < deskCols; j++)
                {
                    if (nextYPositionRect.Intersects(desks[i, j].DeskRect))
                    {
                        collisionDetectedY = true;

                        // Stop vertical movement by snapping to edge of desk
                        if (nextDY > 0) // Moving down
                        {
                            playerRect.Y = desks[i, j].DeskRect.Top - playerRect.Height;
                        }
                        else if (nextDY < 0) // Moving up
                        {
                            playerRect.Y = desks[i, j].DeskRect.Bottom;
                        }
                        break;
                    }
                }
            }

            // If no Y-axis collision was detected, update player's Y position
            if (!collisionDetectedY)
            {
                playerRect.Y += (int)nextDY;
            }

            // Print the player's current position to the Debug window
            System.Diagnostics.Debug.WriteLine($"Player Position: X = {playerRect.X}, Y = {playerRect.Y}");
            System.Diagnostics.Debug.WriteLine($"Last Desk Pos: X = {desks[deskRows - 1, deskCols - 1].DeskRect.X}, Y = {desks[deskRows - 1, deskCols - 1].DeskRect.Y}");

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            Texture2D rectTexture = new Texture2D(GraphicsDevice, 1, 1);
            rectTexture.SetData(new[] { Color.White });
            spriteBatch.Draw(rectTexture, new Rectangle((int)playerRect.X, (int)playerRect.Y, playerSize, playerSize), Color.Red);

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
