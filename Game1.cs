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

        CollisionManager CollisionManager;

        Texture2D rectTexture; // private or nah, yee prolly
        Desk[,] desks;
        int deskRows;
        int deskCols; 

        TheNerd nerd;
        CheatBar cheatBar;
        Player player;
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

            CollisionManager = new();

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

            // collisionManager object here, then pass this into player.Update() ???

            player.Update(currentKeyboardState, desks, nerd);

            //bool collisionOccurred = CollisionManager.HandlePlayerCollision(player, desks, nerd, deskRows, deskCols);

            //// Handle Player-Desk collision
            //if (!(collisionOccurred))
            //{
            //    player.Position = new Point(player.Position.X + (int)player.nextDX, player.Position.Y + (int)player.nextDY);
            //}

            //// Handle collision with nerd's copy zone
            //if (CollisionManager.CheckCollision(player.playerRect, nerd.NerdCopyZone))
            //{
            //    cheatBar.cheatBarCanFill = true;
            //}

            MouseState mouseState = Mouse.GetState();
            cheatBar.Update(mouseState);

            cheatBar.cheatBarCanFill = false;

            //System.Diagnostics.Debug.WriteLine($"Player Position: X = {player.playerRect.X}, Y = {player.playerRect.Y}");

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            player.Draw(spriteBatch);
            nerd.Draw(spriteBatch);

            // Draw desks
            for (int i = 0; i < deskRows; i++)
            {
                for (int j = 0; j < deskCols; j++)
                {
                    desks[i, j].Draw(spriteBatch);
                }
            }

            cheatBar.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
