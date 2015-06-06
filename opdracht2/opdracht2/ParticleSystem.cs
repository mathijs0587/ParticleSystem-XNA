using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace opdracht2
{
    public abstract class ParticleSystem : DrawableGameComponent
    {
        public const int AlphaBlendDrawOrder = 100;
        public const int AdditiveDrawOrder = 200;

        private Game1 game;

        private Texture2D texture;

        private Vector2 origin;

        private int howManyEffects;

        Particle[] particles;

        Queue<Particle> freeParticles;

        public int FreeParticleCount
        {
            get { return freeParticles.Count; }
        }

        #region constants to be set by subclasses

        protected int minNumParticles;
        protected int maxNumParticles;

        public string textureFilename;

        protected float minInitialSpeed;
        protected float maxInitialSpeed;

        protected float minAcceleration;
        protected float maxAcceleration;

        protected float minRotationSpeed;
        protected float maxRotationSpeed;

        protected float minLifetime;
        protected float maxLifetime;

        protected float minScale;
        protected float maxScale;

		protected BlendState blendState;

        #endregion

        protected ParticleSystem(Game1 game, int howManyEffects)
            : base(game)
        {            
            this.game = game;
            this.howManyEffects = howManyEffects;
        }

        public override void Initialize()
        {
            InitializeConstants();

            particles = new Particle[howManyEffects * maxNumParticles];
            freeParticles = new Queue<Particle>(howManyEffects * maxNumParticles);
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i] = new Particle();
                freeParticles.Enqueue(particles[i]);
            }
            base.Initialize();
        }

        protected abstract void InitializeConstants();

        protected override void LoadContent()
        {
            texture = game.Content.Load<Texture2D>("particles\\" + textureFilename);

            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;

            base.LoadContent();
        }

        public void AddParticles(Vector2 where)
        {
            int numParticles = 
                Game1.Random.Next(minNumParticles, maxNumParticles);

            for (int i = 0; i < numParticles && freeParticles.Count > 0; i++)
            {
                Particle p = freeParticles.Dequeue();
                InitializeParticle(p, where);               
            }
        }

        protected virtual void InitializeParticle(Particle p, Vector2 where)
        {
            Vector2 direction = PickRandomDirection();

            float velocity = 
                Game1.RandomBetween(minInitialSpeed, maxInitialSpeed);
            float acceleration =
                Game1.RandomBetween(minAcceleration, maxAcceleration);
            float lifetime =
                Game1.RandomBetween(minLifetime, maxLifetime);
            float scale =
                Game1.RandomBetween(minScale, maxScale);
            float rotationSpeed =
                Game1.RandomBetween(minRotationSpeed, maxRotationSpeed);

            p.Initialize(
                where, velocity * direction, acceleration * direction,
                lifetime, scale, rotationSpeed);
        }

        protected virtual Vector2 PickRandomDirection()
        {
            float angle = Game1.RandomBetween(0, MathHelper.TwoPi);
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (Particle p in particles)
            {
                
                if (p.Active)
                {
                    p.Update(dt);

                    if (!p.Active)
                    {
                        freeParticles.Enqueue(p);
                    }
                }   
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
			game.SpriteBatch.Begin(SpriteSortMode.Deferred, blendState);
            
            foreach (Particle p in particles)
            {
                if (!p.Active)
                    continue;

                float normalizedLifetime = p.TimeSinceStart / p.Lifetime;

                float alpha = 4 * normalizedLifetime * (1 - normalizedLifetime);
				Color color = Color.White * alpha;

                float scale = p.Scale * (.75f + .25f * normalizedLifetime);

                game.SpriteBatch.Draw(texture, p.Position, null, color,
                    p.Rotation, origin, scale, SpriteEffects.None, 0.0f);
            }

            game.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
