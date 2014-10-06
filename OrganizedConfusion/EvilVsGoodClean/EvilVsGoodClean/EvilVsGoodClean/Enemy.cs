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
    //This class handles the behavior for the enemies, such as walking and animating
    //The constructor of this class sets up if the enemy is alive, collidable, its animation, and its size.
    class Enemy : Character
    {
        #region Attributes

        // Attributes
        // A boolean determining if the enemy is alive or not
        private Boolean isAlive;

        //The starting position to be referenced by the world class
        private int startPositX;
        private int startPositY;

        // Attributes dealing with walking 
        private static int initialWalkTime = 2100;
        private int walkTime = initialWalkTime;
        private bool walkRight = true;

        #endregion

        #region Properties

        // Property for returning and assigning the isAlive value
        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }

        // Property for returning and assigning the startPositX value
        public int StartPositX
        {
            get { return startPositX; }
            set { startPositX = value; }
        }

        // Property for returning and assigning the startPositY value
        public int StartPositY
        {
            get { return startPositY; }
            set { startPositY = value; }
        }
        #endregion

        // The handleCollision method sets isAlive to false if Magic collides with the enemy
        public override void HandleCollision(GameObject collidingGameObject)
        {
            //The enemy will die when it collides with magic
            if (collidingGameObject is Magic)
            {
                isAlive = false;
            }
        }

        // Checks if the Enemy is active. If so, the current sprite state is walk left, otherwise, it is idle.
        public void ChangeStates()
        {
            if (IsAlive == true)
            {
                this.currentSpriteState = Character.spriteState.WalkLeft;
            }
            else
            {
                this.currentSpriteState = Character.spriteState.LeftIdle;
            }
        }

        // Moves the Enemy left and right for a specified period of time
        public void Walk(GameTime gameTime)
        {
            walkTime -= (int)gameTime.ElapsedGameTime.TotalMilliseconds; // sets walkTime to the elapsed time (in Milliseconds)

            // If the walkTime is half way through the initialWalkTime, the enemy stops walking right
            if (walkTime <= initialWalkTime / 2)
            {
                walkRight = false;
            }

            // If the walkTime reaches 0, the enemy walks right
            if (walkTime <= 0)
            {
                walkRight = true;
                walkTime = initialWalkTime;
            }

            // When walkRight is true, the enemy's rectangle moves right and the enemy's sprite state is set to WalkRight
            if (walkRight == true)
            {
                this.objectRectangle.X += 1;
                this.currentSpriteState = Character.spriteState.WalkLeft;
            }

            // When walkRight is false, the enemy's rectangle moves left and the enemy's sprite state is set to WalkLeft
            if (walkRight == false)
            {
                this.objectRectangle.X -= 1;
                this.currentSpriteState = Character.spriteState.WalkRight;
            }
        }

        // Constructor
        // The Enemy Constructor takes in a game
        public Enemy(Game game)
            : base(game)
        {
            // Sets the character's rectangle and their animation in the Constructor
            // IsAlive is also initially set to true
            collidable = true;
            characterAnimation = new Animation(objectTexture, this);
            objectRectangle = new Rectangle(200, 400, 40, 50);
            IsAlive = true;
        }
    }
}
