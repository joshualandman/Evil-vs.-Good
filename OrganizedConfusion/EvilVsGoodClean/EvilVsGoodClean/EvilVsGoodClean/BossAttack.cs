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
    //The class for the attack that is created by the boss to kill the player.
    //The boss class sets it where the attack should be placed while the actual attack class sets the boolean that instigates or removes an attack to true or false
    class BossAttack : GameObject
    {
        //Attributes
        private bool activeAttack = false;
        private bool isPaused = false;
        private int startPosition;
        private SpriteBatch attackSpriteBatch;

        //Properties
        public bool ActiveAttack
        {
            get { return activeAttack; }
            set { activeAttack = value; }
        }

        public bool IsPaused
        {
            get { return isPaused; }
            set { isPaused = value; }
        }


        //Constructor
        public BossAttack(Game game)
            : base(game)
        {
            this.objectRectangle.Width = 50;
            this.objectRectangle.Height = 50;
        }

        // Load the texture and spritebatch
        protected override void LoadContent()
        {
            // Set the attack's texture
            this.objectTexture = Game.Content.Load<Texture2D>("Flower Attack");
            attackSpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        // The method that resets the attack entity to the top of the screen and the desired X position
        // The attack is also set to true so that it may fire downward
        public void UseAttack(int posit)
        {
            this.objectRectangle.Y = 0;
            startPosition = posit;
            this.objectRectangle.X = startPosition;
            activeAttack = true;
        }

        // The Update Method
        public override void Update(GameTime gameTime)
        {
            // While the attack is active and not paused, the object will move down until it reaches bottom of the screen and is set to inactive
            if (activeAttack == true && isPaused == false)
            {
                this.objectRectangle.Y = this.objectRectangle.Y + 6;
            }

            //Once the attack goes off screen, it will be removed from the world by virtue of its "activeAttack" being set to false
            //This will allow the attack to be moved to the top of the screen and spawned again after it leaves the screen.
            if (this.objectRectangle.Y > 600)
            {
                activeAttack = false;
            }
        }

        // Draw method
        public override void Draw(GameTime gameTime)
        {
            attackSpriteBatch.Begin();

            // It will only draw when active and the game is not paused
            if (activeAttack == true && isPaused == false)
            {
                attackSpriteBatch.Draw(this.objectTexture, this.objectRectangle, Color.White);
            }

            attackSpriteBatch.End();
        }
    }
}
