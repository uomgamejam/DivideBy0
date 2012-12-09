using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Badumtsss.Objects;

namespace Badumtsss
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D lightTexture;
        Obstacle[] obstacles = new Obstacle[3];

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load texture
            lightTexture = Content.Load<Texture2D>("light_tex");

            // Initialize obstacles
            obstacles[0] = new Obstacle(Obstacle.ObjectType.MIRROR, new Vector2(150, 10), 5, 15, 0f);
            obstacles[1] = new Obstacle(Obstacle.ObjectType.MIRROR, new Vector2(300, 10), 5, 15, 0f);
            obstacles[2] = new Obstacle(Obstacle.ObjectType.MIRROR, new Vector2(100), 5, 15, 0f);
            

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        private void DrawLine(Vector2 source, Vector2 destination)
        {
            float distance = Vector2.Distance(source, destination);
            float angle = (float)Math.Atan2(destination.Y - source.Y, destination.X - source.X);

            spriteBatch.Begin();
            spriteBatch.Draw(lightTexture, new Rectangle((int)source.X, (int)source.Y, (int)distance, 2), null, Color.White, angle, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        private Obstacle FindObject(Obstacle[] obstacles, Vector2 start, short direction, float angle)
        {
            //update 1300 with MaxWidth
            RotatedRectangle r = new RotatedRectangle(new Rectangle((int)start.X, (int)start.Y, 1300, 2), angle);
            float dmin = 1300;
            float distance;
            Obstacle colidedObstacle = null;
            foreach (var obstacle in obstacles)
            {
                distance = Vector2.Distance(start, obstacle.A);
                if (r.Intersects(obstacle) && distance < dmin && distance > 0)
                {
                    dmin = Vector2.Distance(start, obstacle.A);
                    colidedObstacle = obstacle;
                }
            }
            return colidedObstacle;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            //  DrawLine(obstacles[0], obstacles[1]);

            Vector2 start = new Vector2(10);
            Obstacle nextObstacle = FindObject(obstacles, start, 1, 0);
            Obstacle currentObstactle = nextObstacle;
            DrawLine(start, nextObstacle.A);
            MouseState currentState = Mouse.GetState();
            MouseState previousState = new MouseState(0, 0, 0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
            this.IsMouseVisible = true;
            if (currentState.LeftButton == ButtonState.Pressed || previousState.LeftButton != ButtonState.Pressed)
            {
                while ((nextObstacle = FindObject(obstacles, nextObstacle.A, 1, nextObstacle.Rotation)) != null)
                {
                    DrawLine(currentObstactle.A, nextObstacle.A);
                    currentObstactle = nextObstacle;
                }
                previousState = currentState;
            }

            /*
            DrawLine(new Vector2(10, 10), obstacles[0].A);
            for (int index = 0; index < 2; ++index)
                DrawLine(obstacles[index].A, obstacles[index + 1].A);
             */
            
            Rectangle a = new Rectangle();
            Obstacle o = new Obstacle(Obstacle.ObjectType.MIRROR, new Vector2(10), 30, 30, 0f);
            base.Draw(gameTime);
        }
    }
}
