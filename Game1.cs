using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

/*
 * CURRENT TASK: Teacher GridPath classes
 * 
 * Next Task: Rename DeskLayout to like LevelBuilder it's just becoming one big level builder
 */

namespace StickClassroom
{
    public class Game1 : Game // Essentially Game1 is your LevelManager class, obviously we'll fix this later, but for now this is assumed.
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        CollisionManager CollisionManager;

        DeskLayout DeskLayout;

        Texture2D rectTexture; // private or nah, yee prolly
        Desk[,] desks;
        int deskRows;
        int deskCols; 

        TheNerd nerd;
        CheatBar cheatBar;
        Player player;
        Teacher teacher;
        KeyboardState currentKeyboardState;

        SpriteFont font;

        List<Rectangle> collidables = new List<Rectangle>(); // pretty sure we can move this out into DeskLayout 

        public Game1()
        {
            IsFixedTimeStep = true; // so Update() will be called 60 times per second
            TargetElapsedTime = TimeSpan.FromMilliseconds(16.67); // 60 FPS

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
            teacher = new(250, 100, rectTexture);

            cheatBar = new(rectTexture);

            collidables.Add(nerd.NerdRect);
            // collidables.Add(teacher.teacherRect); collidables only for non-moving objects for now

            DeskLayout = new DeskLayout(rectTexture, collidables, "High"); // Middle, Elem

            CollisionManager = new(collidables.ToArray());

            // Set Window Size
            graphics.PreferredBackBufferWidth = Globals.WindowWidth;  
            graphics.PreferredBackBufferHeight = Globals.WindowHeight;
            graphics.ApplyChanges(); // Apply the changes to the window size

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Test");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            currentKeyboardState = Keyboard.GetState();

            player.Update(currentKeyboardState, CollisionManager);
            teacher.Update();

            // temp behavior
            if (teacher.IsPointInVisionCone(player.Position))
            {
                player.playerSpeed = 0;
            } 
            else
            {
                player.playerSpeed = 5;
            }

            if (CollisionManager.isCollidingRects(player.playerRect, nerd.NerdCopyZone)) 
            {
                cheatBar.cheatBarCanFill = true;
            }

            // Death trigger check
            if (CollisionManager.isCollidingRects(player.playerRect, teacher.teacherRect))
            {
                // So legit if you touch the teacher you're dead, no collision reaction necessary since you die upon touching them lol
                System.Diagnostics.Debug.WriteLine($"You're dead!");
            }

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
            //nerd.Draw(spriteBatch);
            teacher.Draw(spriteBatch);

            // Draw desks
            DeskLayout.Draw(spriteBatch);

            cheatBar.Draw(spriteBatch, font);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
