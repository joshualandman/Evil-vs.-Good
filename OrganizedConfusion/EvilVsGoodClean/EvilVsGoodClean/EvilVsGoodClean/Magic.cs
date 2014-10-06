using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace EvilVsGoodClean
{
    //The magic class creates the fireballs that the player uses to defeat enemies as well as the behaviour of those fireballs
    class Magic : GameObject
    {
        #region Attributes
        // Attributes
        private SpriteBatch magicSpriteBatch;
        private int magicTime;

        // Boolean value to check the current state of Magic
        private Boolean magicActive = false;

        // Boolean values to determine direction of magic
        //ShotL dictates that the magic goes left while shotR mandates that it goes right
        private bool shotLeft = false;
        private bool shotRight = false;

        #endregion

        // Property for returning and assigning the magicActive value
        public Boolean MagicActive
        {
            get { return magicActive; }
            set { magicActive = value; }
        }

        public Boolean ShotLeft
        {
            get { return shotLeft; }
            set { shotLeft = value; }
        }

        public Boolean ShotRight
        {
            get { return shotRight; }
            set { shotRight = value; }
        }

        // Constructor
        // Magic Constructor takes in a game and a player
        public Magic(Game game)
            : base(game)
        {
            shouldDraw = false;
            collidable = true;
        }

        // This method eliminates magic if it collides with any collidable game object
        // It sets MagicActive to false and sets both shotL and shotR to false
        public override void HandleCollision(GameObject collidingGameObject)
        {
            magicActive = false;
            shotLeft = false;
            shotRight = false;
        }


        // Sets the magic's objectTexture to "magic" and loads the sprite batch
        protected override void LoadContent()
        {
            // Set the character's sprite to the sprite sheet
            this.objectTexture = Game.Content.Load<Texture2D>("magic");
            magicSpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        // This method will draw the magic
        public override void Draw(GameTime gameTime)
        {
            // Begin the sprite batch
            magicSpriteBatch.Begin();

            // If magicActive is true, the magic will be drawn
            if (shouldDraw == true && magicActive == true)
            {
                // The magic rectangle size is set
                this.objectRectangle.Width = 10;
                this.objectRectangle.Height = 10;

                // The magic rectangle is drawn
                magicSpriteBatch.Draw(this.objectTexture, this.objectRectangle, null, Color.Red);
            }

            // End the sprite batch
            magicSpriteBatch.End();
        }

        // Update sets magic's object rectangle as a new rectangle and checks to see if the magic is no longer active so that it's position is set back to the player
        public override void Update(GameTime gameTime)
        {
            this.objectRectangle = new Rectangle((int)this.objectPosition.X, (int)this.objectPosition.Y, this.objectTexture.Width, this.objectTexture.Height);

            magicTime -= (int)gameTime.ElapsedGameTime.TotalMilliseconds; // Sets the variable magicTime to the elapsed time (in Milliseconds)

            // For 100 milliseconds, the magic will move up by 5
            if (magicTime < 100 && magicTime > 0)
            {
                this.objectPosition.Y -= 2;
            }

            // For the next 100 milliseconds, the magic moves down by 3
            if (magicTime >= 100 && magicTime < 200)
            {
                this.objectPosition.Y += 1;
            }

            // This resets the magicTime to 200 so that the Update method continues to change the magic's Y value
            if (magicTime < 0)
            { magicTime = 200; }

            if (shouldDraw == true && magicActive == true)
            {
                // If shotL is true, magic is drawn moving left...
                if (shotLeft == true)
                {
                    this.objectPosition.X += 10;
                }

                // ...Otherwise it will move right.
                else if (shotRight == true)
                {
                    this.objectPosition.X -= 10;
                }
            }
        }

        public void SetToCenter(int x, int y)
        {
            // If magic is not active, the magic rectangle is set back to the center of the player
            if (magicActive != true)
            {
                this.objectPosition.X = x + 25;
                this.objectPosition.Y = y + 20;
            }
        }
    }
}

