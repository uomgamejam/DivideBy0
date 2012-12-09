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
        public static ContentManager content;
        Obstacle[] obstacles = new Obstacle[3];

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            content = Content;
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
            obstacles[0] = new Obstacle(Obstacle.ObjectType.MIRROR, new Vector2(150.0f, 30f), 5, 15, (float)Math.PI / 6);
            obstacles[1] = new Obstacle(Obstacle.ObjectType.MIRROR, new Vector2(300, 30), 5, 15, 0f);
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
            KeyboardState keyboardStateCurrent, keyboardStatePrevious;
            keyboardStateCurrent = Keyboard.GetState();

            int index = 0;
            if (keyboardStateCurrent.IsKeyDown(Keys.A))
                index = 0;
            if (keyboardStateCurrent.IsKeyDown(Keys.S))
                index = 1;
            if (keyboardStateCurrent.IsKeyDown(Keys.D))
                index = 2;

            if (keyboardStateCurrent.IsKeyDown(Keys.Left))
                obstacles[index].Rotate(0.015f);

            if (keyboardStateCurrent.IsKeyDown(Keys.Right))
                obstacles[index].Rotate(-0.015f);

            base.Update(gameTime);
        }

        //for debugging
        private void DrawLine(Vector2 source, Vector2 destination, Color c)
        {
            float distance = Vector2.Distance(source, destination);
            float angle = (float)Math.Atan2(destination.Y - source.Y, destination.X - source.X);

            //spriteBatch.Begin();
            spriteBatch.Draw(lightTexture, new Rectangle((int)source.X, (int)source.Y, (int)distance, 2), null, c, angle, Vector2.Zero, SpriteEffects.None, 0);
            //spriteBatch.Draw(lightTexture, source, null, Color.White);
            //spriteBatch.End();
        }

        private void DrawLine(Vector2 source, Vector2 destination)
        {
            float distance = Vector2.Distance(source, destination);
            float angle = (float)Math.Atan2(destination.Y - source.Y, destination.X - source.X);

            //spriteBatch.Begin();
            spriteBatch.Draw(lightTexture, new Rectangle((int)source.X, (int)source.Y, (int)distance, 2), null, Color.White, angle, Vector2.Zero, SpriteEffects.None, 0);
            //spriteBatch.Draw(lightTexture, source, null, Color.White);
            //spriteBatch.End();
        }

        /// <summary>
        /// Will find the next obstacle in the beam's way
        /// </summary>
        /// <param name="obstacles">the array where to look for objects</param>
        /// <param name="start">the starting point of the beam</param>
        /// <param name="direction">the way in which we go on the direction</param>
        /// <param name="angle">the angle of the path to look for an object</param>
        /// <returns>the closest obstacle on the given path</returns>
        private Obstacle FindObject(Obstacle[] obstacles, Vector2 start, short direction, float angle)
        {
            //update 1300 with MaxWidth
            //the beam of light
            RotatedRectangle beam = new RotatedRectangle(new Rectangle((int)start.X, (int)start.Y, 1300, 2), angle);
            float dmin = 1300;
            float distance;
            Obstacle colidedObstacle = null;
            foreach (var obstacle in obstacles)
            {
                distance = Vector2.Distance(start, obstacle.Center);
                if (beam.Intersects(obstacle) && distance < dmin && distance > 0)
                {
                    dmin = Vector2.Distance(start, obstacle.Center);
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
            spriteBatch.Begin();
            Vector2 start = new Vector2(30);

            Obstacle nextObstacle = FindObject(obstacles, start, 1, 0);
            Obstacle currentObstacle = nextObstacle;
            DrawLine(start, nextObstacle.Center);

            this.IsMouseVisible = true;
            while (true)
            {
                nextObstacle = FindObject(obstacles, currentObstacle.Center, 1, currentObstacle.Rotation + (float)Math.PI / 2);
                if (nextObstacle == null)
                {
                    break;
                }
                DrawLine(currentObstacle.Center, nextObstacle.Center);
                currentObstacle = nextObstacle;
            }
            DrawLine(currentObstacle.Center, new Vector2(currentObstacle.Center.X * currentObstacle.Rotation, 1300));
            
            foreach (var obstacle in obstacles)
                spriteBatch.Draw(obstacle.Texture, new Rectangle(obstacle.X, obstacle.Y, 15, 45), null, Color.White, obstacle.Rotation, new Vector2(2.5f, 7.5f), SpriteEffects.None, 0);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private float Reflect(Vector2 ray, float objAngle)
        {
            float result = 0f;



            return result;
        }
    }
}
