using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

// test commit

namespace StickClassroom
{
    public class Game1 : Game // Essentially Game1 is your LevelManager class, obviously we'll fix this later, but for now this is assumed.
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        Texture2D rectTexture;

        Desk[,] desks;

        // Desk grid size
        int deskRows;
        int deskCols; 

        // The Nerd
        TheNerd nerd;

        CheatBar cheatBar;

        Player player;

        // The Teacher
        Rectangle teacherRect;

        KeyboardState currentKeyboardState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Should be based on which Level
            deskRows = 4;
            deskCols = 6;

            rectTexture = new Texture2D(GraphicsDevice, 1, 1);
            rectTexture.SetData(new[] { Color.White });

            // People
            //player.playerRect = new Rectangle(655, 825, Globals.StudentSize, Globals.StudentSize);
            player = new(655, 825, rectTexture);
            nerd = new(600, 825, rectTexture);
            teacherRect = new Rectangle(250, 100, Globals.StudentSize + 5, Globals.StudentSize + 5);

            // Cheat Bar
            cheatBar = new(rectTexture);

            // Set Window Size
            graphics.PreferredBackBufferWidth = Globals.WindowWidth;  
            graphics.PreferredBackBufferHeight = Globals.WindowHeight;
            graphics.ApplyChanges(); // Apply the changes to the window size

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            desks = new Desk[deskRows, deskCols];
            for (int i = 0; i < deskRows; i++)
            {
                for (int j = 0; j < deskCols; j++)
                {
                    // Calculate desk positions
                    int x = i * 158 + 80;
                    int y = j * 120 + 170;

                    // Create and place a new desk at the calculated position
                    desks[i, j] = new Desk(x, y, rectTexture);
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            currentKeyboardState = Keyboard.GetState();

            // Handle movement in X direction first
            bool collisionDetectedX = false;
            Rectangle nextXPositionRect = player.playerRect;
            nextXPositionRect.X += (int)player.nextDX;

            player.Update(currentKeyboardState);

            // Collision with desks
            for (int i = 0; i < deskRows; i++)
            {
                for (int j = 0; j < deskCols; j++)
                {
                    if (nextXPositionRect.Intersects(desks[i, j].DeskRect))
                    {
                        collisionDetectedX = true;

                        // Stop horizontal movement by snapping to edge of desk
                        if (player.nextDX > 0) // Moving right
                        {
                            player.playerRect = new Rectangle(desks[i, j].DeskRect.Left - player.playerRect.Width, player.Position.Y, Globals.StudentSize, Globals.StudentSize);
                        }
                        else if (player.nextDX < 0) // Moving left
                        {
                            player.playerRect = new Rectangle(desks[i, j].DeskRect.Right, player.Position.Y, Globals.StudentSize, Globals.StudentSize);
                        }
                        break;
                    }
                }
            }

            // Collision with nerd 
            if (nextXPositionRect.Intersects(nerd.NerdRect))  
            {
                collisionDetectedX = true;

                // Stop horizontal movement by snapping to edge of the nerd
                //if (player.nextDX > 0) // Moving right
                //{
                //    player.playerRect.X = nerd.NerdRect.Left - player.playerRect.Width;
                //}
                //else if (player.nextDX < 0) // Moving left
                //{
                //    player.playerRect.X = nerd.NerdRect.Right;
                //}
            }

            // If no X-axis collision was detected, update player's X position
            if (!collisionDetectedX)
            {
                //player.playerRect += (int)player.nextDX;
                player.Position = new Point(player.Position.X + (int)player.nextDX, player.Position.Y);
            }

            // Handle movement in Y direction separately
            bool collisionDetectedY = false;
            Rectangle nextYPositionRect = player.playerRect;
            nextYPositionRect.Y += (int)player.nextDY;

            // Collision with Desks
            for (int i = 0; i < deskRows; i++)
            {
                for (int j = 0; j < deskCols; j++)
                {
                    //if (nextYPositionRect.Intersects(desks[i, j].DeskRect))
                    //{
                    //    collisionDetectedY = true;

                    //    // Stop vertical movement by snapping to edge of desk
                    //    if (player.nextDY > 0) // Moving down
                    //    {
                    //        player.playerRect.Y = desks[i, j].DeskRect.Top - player.playerRect.Height;
                    //    }
                    //    else if (player.nextDY < 0) // Moving up
                    //    {
                    //        player.playerRect.Y = desks[i, j].DeskRect.Bottom;
                    //    }
                    //    break;
                    //}
                }
            }

            // Collision with nerd
            //if (nextYPositionRect.Intersects(nerd.NerdRect))
            //{
            //    collisionDetectedY = true;

            //    // Stop vertical movement by snapping to edge of the nerd
            //    if (player.nextDY > 0) // Moving right
            //    {
            //        player.playerRect.Y = nerd.NerdRect.Top - player.playerRect.Height;
            //    }
            //    else if (player.nextDY < 0) // Moving left
            //    {
            //        player.playerRect.Y = nerd.NerdRect.Bottom;
            //    }
            //}

            // Collision with nerd.nerdCopyZone - so the player can see the nerd's test so they can now copy
            if (nextXPositionRect.Intersects(nerd.NerdCopyZone) || nextYPositionRect.Intersects(nerd.NerdCopyZone))
            {
                this.cheatBar.cheatBarCanFill = true;
            }
            MouseState mouseState = Mouse.GetState();
            this.cheatBar.Update(mouseState);

            this.cheatBar.cheatBarCanFill = false;

            // If no Y-axis collision was detected, update player's Y position
            if (!collisionDetectedY)
            {
                //player.playerRect.Y += (int)player.nextDY;
                player.Position = new Point(player.Position.X + (int)player.nextDX, player.Position.Y + (int)player.nextDY);
            }

            //System.Diagnostics.Debug.WriteLine($"Player Position: X = {player.playerRect.X}, Y = {player.playerRect.Y}");

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            this.player.Draw(spriteBatch);

            this.nerd.Draw(spriteBatch);

            // Draw desks
            for (int i = 0; i < deskRows; i++)
            {
                for (int j = 0; j < deskCols; j++)
                {
                    desks[i, j].Draw(spriteBatch);
                }
            }

            this.cheatBar.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
