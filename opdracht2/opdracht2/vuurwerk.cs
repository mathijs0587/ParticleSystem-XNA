using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace opdracht2
{
    public class vuurwerk : ParticleSystem
    {
        public vuurwerk(Game1 game, int howManyEffects)
            : base(game, howManyEffects)
        {
            
        }

        protected override void InitializeConstants()
        {
            string vuurwerkKleur = "blueSpark";
            Random random = new Random();
            int randomInt = random.Next(1, 9);
            switch (randomInt)
            {
                case 1:
                    vuurwerkKleur = "blueSpark";
                    break;
                case 2:
                    vuurwerkKleur = "goldSpark";
                    break;
                case 3:
                    vuurwerkKleur = "greenSpark";
                    break;
                case 4:
                    vuurwerkKleur = "orangeSpark";
                    break;
                case 5:
                    vuurwerkKleur = "purpleSpark";
                    break;
                case 6:
                    vuurwerkKleur = "redSpark";
                    break;
                case 7:
                    vuurwerkKleur = "whiteSpark";
                    break;
                case 8:
                    vuurwerkKleur = "yellowSpark";
                    break;
            }

            textureFilename = vuurwerkKleur;       

            minInitialSpeed = 40;
            maxInitialSpeed = 500;

            minAcceleration = 0;
            maxAcceleration = 0;

            minLifetime = .5f;
            maxLifetime = 1.0f;

            minScale = .3f;
            maxScale = 1.5f;

            minNumParticles = 500;
            maxNumParticles = 750;

            minRotationSpeed = -MathHelper.PiOver4;
            maxRotationSpeed = MathHelper.PiOver4;

            blendState = BlendState.Additive;

            DrawOrder = AdditiveDrawOrder;
        }

        protected override void InitializeParticle(Particle p, Vector2 where)
        {
            base.InitializeParticle(p, where);

            p.Acceleration = -p.Velocity / p.Lifetime;
        }
    }
}
