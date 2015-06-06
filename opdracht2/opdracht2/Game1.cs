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

namespace opdracht2
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        private static Random random = new Random();
        public static Random Random
        {
            get { return random; }
        }

        vuurwerk Vuurwerk;

        const float TimeBetweenExplosions = 0.5f;
        float timeTillExplosion = 0.0f;

        int MaxVuurwerkjes = 8;
        int Vuurwerkjes = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);

        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            UpdateVuurwerk(dt);


            base.Update(gameTime);
        }

        private void UpdateVuurwerk(float dt)
        {
            timeTillExplosion -= dt;
            if (timeTillExplosion < 0)
            {
                Vector2 where = Vector2.Zero;

                where.X = RandomBetween(0, graphics.GraphicsDevice.Viewport.Width);
                where.Y = RandomBetween(0, graphics.GraphicsDevice.Viewport.Height);
                Vuurwerkjes++;
                Vuurwerk = new vuurwerk(this, 1);                
                Components.Add(Vuurwerk);
                if ((Vuurwerkjes - MaxVuurwerkjes) >= 0)
                {
                    Components.RemoveAt(Vuurwerkjes - MaxVuurwerkjes);
                }
                if (Vuurwerkjes >= 15)
                {
                    Vuurwerkjes = 0;
                }
                
                Vuurwerk.AddParticles(where);

                timeTillExplosion = TimeBetweenExplosions;
            }            
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

                        

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static float RandomBetween(float min, float max)
        {
            return min + (float)random.NextDouble() * (max - min);
        }
    }
}
