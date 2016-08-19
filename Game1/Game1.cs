using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ECS {
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Engine engine;

        public const int Width = 1280;
        public const int Height = 720;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = Width;
            graphics.PreferredBackBufferHeight = Height;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            engine = new Engine();
            engine.AddSystem<Systems.Renderer>()
                  .LoadTexture(this, "ball", "ball");
            engine.AddSystem<Systems.BallControl>();

            Random random = new Random();

            for (int i = 0; i < 10; ++i) {
                Entity ball = engine.AddEntity()
                                    .AddComponent<ECS.Components.Position>()
                                    .AddComponent<ECS.Components.Velocity>()
                                    .AddComponent<ECS.Components.BallSprite>()
                                    .AddComponent<ECS.Components.Collidable>();
                ball.GetComponent<ECS.Components.Position>()
                    .pos = new Vector2((float)random.NextDouble() * Width,
                                       (float)random.NextDouble() * Height);

                var collider = ball.GetComponent<ECS.Components.Collidable>();
                var circle = new Colliders.Circle();

                collider.mass = random.Next(1, 10);
                circle.Radius = collider.mass * Systems.Renderer.ScaleFactor * 48;
                collider.collider = circle;

                float MaxVel = 10f / collider.mass;
                ball.GetComponent<ECS.Components.Velocity>()
                    .vel = new Vector2((float)random.NextDouble() * MaxVel,
                                       (float)random.NextDouble() * MaxVel);
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            engine.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            engine.Render(spriteBatch, gameTime);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
