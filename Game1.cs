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

        float nextDX = 0;
        float nextDY = 0;

        // The Nerd
        Rectangle nerdRect;
        Rectangle nerdCopyZone; 

        // The Teacher
        Rectangle teacherRect;

        Texture2D pixel;
        int cheatBarX = Globals.WindowWidth - 50;
        int cheatBarY = 100;
        //int cheatBarWidth = 35;
        //int cheatBarHeight = Globals.WindowHeight - 160;
        //int cheatBarFillY = cheatBarY + cheatBarHeight;
        //int cheatBarFillHeight = 10;
        float cheatBarFillPercentage = 0.0f;
        bool cheatBarCanFill = false;
        //int growthRateCheatBar = 1;

        Boolean isPlayerMoving = false;

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public void DrawCheatBar(SpriteBatch spriteBatch, Rectangle outlineRect, float fillPercentage, Color outlineColor, Color fillColor)
        {
            // Draw the outline rectangle
            spriteBatch.Draw(pixel, outlineRect, outlineColor);

            // Calculate the height of the filling rectangle based on the percentage
            int filledHeight = (int)((outlineRect.Height - 4) * fillPercentage); // Subtracting 4 to keep space for top and bottom outline

            // Reduce the width of the filled rectangle slightly to ensure the outline is visible on the sides
            int fillWidth = outlineRect.Width - 4; // Subtracting 4 to keep space for the outline on both sides

            // Adjust the X and Y positions so the fill rect is properly inside the outline
            int fillX = outlineRect.X + 2; // Offset X to center the fill rect
            int fillY = outlineRect.Y + 2 + (outlineRect.Height - 4 - filledHeight); // Offset Y to account for the top outline and filling

            // Create the filled rectangle inside the outline
            Rectangle fillRect = new Rectangle(fillX, fillY, fillWidth, filledHeight);

            // Draw the filling rectangle
            spriteBatch.Draw(pixel, fillRect, fillColor);
        }

        protected override void Initialize()
        {
            playerRect = new Rectangle(655, 825, playerSize, playerSize);
            nextPlayerPositionRect = new Rectangle(655, 825, playerSize, playerSize); // might not need this

            /*
             * Lowkey playerSize might be teacher size and then 
             * playerSize might need to be a little smaller, it depends on desks too so just experiment
             *
             */
            int padding = 15;
            nerdRect = new Rectangle(600, 825, playerSize, playerSize); // 100, 195 ???
            nerdCopyZone = new Rectangle(
                nerdRect.X - padding,
                nerdRect.Y - padding,
                nerdRect.Width + 2 * padding,
                nerdRect.Height + 2 * padding);

            teacherRect = new Rectangle(250, 100, playerSize + 5, playerSize + 5);

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

            // Cheat bar stuff
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Get the current keyboard currentKeyboardState
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
   
            // Store next position deltas
            nextDX = 0;
            nextDY = 0;

            cheatBarCanFill = false;

            // EWW Fix this crap
            // Movement input handling
            if (currentKeyboardState.IsKeyDown(Keys.W))
            {
                nextDY = -playerSpeed; // Move up
                isPlayerMoving = true;
            } 
            else if (previousKeyboardState.IsKeyDown(Keys.W) && currentKeyboardState.IsKeyUp(Keys.W)) 
            {
                isPlayerMoving = false;
            }
            if (currentKeyboardState.IsKeyDown(Keys.A))
            {
                nextDX = -playerSpeed; // Move left
                isPlayerMoving = true;
            }
            else if (previousKeyboardState.IsKeyDown(Keys.A) && currentKeyboardState.IsKeyUp(Keys.A))
            {
                isPlayerMoving = false;
            }
            if (currentKeyboardState.IsKeyDown(Keys.S))
            {
                nextDY = playerSpeed; // Move down
                isPlayerMoving = true;
            }
            else if (previousKeyboardState.IsKeyDown(Keys.S) && currentKeyboardState.IsKeyUp(Keys.S))
            {
                isPlayerMoving = false;
            }
            if (currentKeyboardState.IsKeyDown(Keys.D))
            {
                nextDX = playerSpeed; // Move right
                isPlayerMoving = true;
            }
            else if (previousKeyboardState.IsKeyDown(Keys.D) && currentKeyboardState.IsKeyUp(Keys.D))
            {
                isPlayerMoving = false;
            }

            // Handle movement in X direction first
            bool collisionDetectedX = false;
            Rectangle nextXPositionRect = playerRect;
            nextXPositionRect.X += (int)nextDX;

            // Collision with desks
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

            // Collision with nerd 
            if (nextXPositionRect.Intersects(nerdRect))  
            {
                collisionDetectedX = true;
               // cheatBarCanFill = true;

                // Stop horizontal movement by snapping to edge of the nerd
                if (nextDX > 0) // Moving right
                {
                    playerRect.X = nerdRect.Left - playerRect.Width;
                }
                else if (nextDX < 0) // Moving left
                {
                    playerRect.X = nerdRect.Right;
                }
            }

            // Collision with nerdCopyZone - so the player can see the nerd's test so they can now copy
            if (nextXPositionRect.Intersects(nerdCopyZone))
            {
                cheatBarCanFill = true; 
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

            // Collision with Desks
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

            // Collision with nerd
            if (nextYPositionRect.Intersects(nerdRect))
            {
                collisionDetectedY = true;
                //cheatBarCanFill = true;

                // Stop vertical movement by snapping to edge of the nerd
                if (nextDY > 0) // Moving right
                {
                    playerRect.Y = nerdRect.Top - playerRect.Height;
                }
                else if (nextDY < 0) // Moving left
                {
                    playerRect.Y = nerdRect.Bottom;
                }
            }

            // Collision with nerdCopyZone - so the player can see the nerd's test so they can now copy
            if (nextYPositionRect.Intersects(nerdCopyZone))
            {
                cheatBarCanFill = true;
            }

            // If no Y-axis collision was detected, update player's Y position
            if (!collisionDetectedY)
            {
                playerRect.Y += (int)nextDY;
            }

            // Fill the cheat bar up if needed
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed && cheatBarCanFill)
            {
                if (cheatBarFillPercentage <= 0.99f)
                {
                    cheatBarFillPercentage += 0.01f;
                }
                else // so, the cheatBar is full meaning, the player has won the level
                {
                    // Level complete!
                }
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
            // spriteBatch.Draw(rectTexture, new Rectangle((int)playerRect.X, (int)playerRect.Y, playerSize, playerSize), Color.Red);
            spriteBatch.Draw(rectTexture, playerRect, Color.Red);

           // spriteBatch.Draw(rectTexture, nerdCopyZone, Color.Yellow); // Eventually this will be invisible of course
            spriteBatch.Draw(rectTexture, nerdRect, Color.Blue);

            spriteBatch.Draw(rectTexture, teacherRect, Color.Green);

            // Draw desks
            for (int i = 0; i < deskRows; i++)
            {
                for (int j = 0; j < deskCols; j++)
                {
                    desks[i, j].Draw(spriteBatch);
                }
            }

            // Define the cheat bar outline and fill percentage
            Rectangle outlineRect = new Rectangle(cheatBarX - 25, cheatBarY - 40, 40, 730);  // Position, width, height
            //float fillPercentage = 0.75f;  // This would represent the player's progress (75% filled)

            // Draw the cheat bar with outline and filling
            DrawCheatBar(spriteBatch, outlineRect, cheatBarFillPercentage, Color.Red, Color.Green);


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
