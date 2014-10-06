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
    //The class that handles the player's behavior, such as movement, jumping, and what happens when there is a collision
    class Player : Character
    {
        #region Attributes
        // Create variables for jumping, how high and a boolean for whether they are jumping or not
        private Magic magic;
        private UserInput userInputObject;
        private static double jumpForce = 5;
        private static int jumpTime;
        private int previousY;

        //The bool to check if the player is paused
        //isPaused is used to determin if the player should be able to move or jump due to the game being paused or not
        private bool isPaused = true;

        // Boolean values to check the current state of Jump
        private Boolean jumping = false;
        private Boolean airborne = false;

        //The booleans of the player to detemine everything that the player needs for the world class operations
        private Boolean playerDeath = false;
        private Boolean playerWin = false;
        private Boolean leftScroll = false;
        private Boolean rightScroll = false;
        private Boolean leftCollide = false;
        private Boolean rightCollide = false;
        #endregion

        #region Properties

        // Property for returning the jumpForce value
        public double JumpForce
        {
            get { return jumpForce; }
        }

        // Property for returning and assigning the isPaused value
        public Boolean IsPaused
        {
            get { return isPaused; }
            set { isPaused = value; }
        }

        // Property for returning and assigning the leftScroll value
        public Boolean LeftScroll
        {
            get { return leftScroll; }
            set { leftScroll = value; }
        }

        // Property for returning and assigning the rightScroll value
        public Boolean RightScroll
        {
            get { return rightScroll; }
            set { rightScroll = value; }
        }

        // Property for returning and assigning the leftCollide value
        public Boolean LeftCollide
        {
            get { return leftCollide; }
            set { leftCollide = value; }
        }

        // Property for returning and assigning the rightCollide value
        public Boolean RightCollide
        {
            get { return rightCollide; }
            set { rightCollide = value; }
        }

        // Property for returning and assigning the jumping value
        public Boolean Jumping
        {
            get { return jumping; }
            set { jumping = value; }
        }

        // Property for returning and assigning the airborne value
        public Boolean Airborne
        {
            get { return airborne; }
            set { airborne = value; }
        }

        // Property for returning and assigning the playerDeath value
        public Boolean PlayerDeath
        {
            get { return playerDeath; }
            set { playerDeath = value; }
        }

        // Property for returning and assigning the playerWin value
        public Boolean PlayerWin
        {
            get { return playerWin; }
            set { playerWin = value; }
        }

        #endregion

        // Property for returning the userInputObject value
        public UserInput CurrentUserInput
        {
            get { return userInputObject; }
        }

        // Constructor for the player
        public Player(Game game, UserInput uI, Magic m)
            : base(game)
        {
            // Sets the character's rectangle and their animation
            magic = m;
            userInputObject = uI;
            characterAnimation = new Animation(objectTexture, this);
            objectRectangle = new Rectangle(200, 400, 40, 50);
            previousY = objectRectangle.Y;
        }

        // This method is for handling any collison with the player
        public override void HandleCollision(GameObject collidingGameObject)
        {
            // When the player collides with an enemy, the player will die
            if (collidingGameObject is Enemy || collidingGameObject is BossAttack)
            {
                this.playerDeath = true;
            }

            // When the player collides with a tile, collision is dealt with
            else if (collidingGameObject is GameWorldTile)
            {
                // Collision Handling for Tiles

                // If the characters top is below the top of the colliding block
                // The characters is below the tile
                if (this.ObjectRectangle.Y < collidingGameObject.ObjectRectangle.Y)
                {
                    // If the characters bottom is higher than or touching the top of the other tiles
                    if (this.ObjectRectangle.Bottom >= collidingGameObject.ObjectRectangle.Top)
                    {
                        Rectangle tempRect = this.ObjectRectangle;
                        // Set the character's top to the bottom of the tile
                        tempRect.Y = (collidingGameObject.ObjectRectangle.Top) - (tempRect.Height);
                        this.ObjectRectangle = tempRect;
                        this.Airborne = false; // When the player makes contact with the ground, the player will be able to jump again when jumping is set to false.
                    }
                }

                // If the character's top is above the tiles top
                else if (this.ObjectRectangle.Y > collidingGameObject.ObjectRectangle.Y)
                {
                    // If the character's top is below or touching the tile's bottom
                    if (this.ObjectRectangle.Top <= collidingGameObject.ObjectRectangle.Bottom)
                    {
                        Rectangle tempRect = this.ObjectRectangle;
                        // Set the character's top to the bottom of the tile
                        tempRect.Y = (collidingGameObject.ObjectRectangle.Bottom);
                        this.ObjectRectangle = tempRect;
                        this.Jumping = false; // Player will stop jumping when head hits top of block.
                    }
                }

                // If the character's left is left of the tile's left
                else if (this.ObjectRectangle.X < collidingGameObject.ObjectRectangle.X)
                {
                    // If the character's right is right of or touching the tile's left-hand side.
                    // If the character is past the point of intercestion with the tile but not so past it that the tile is behind the character.
                    if (this.ObjectRectangle.Right >= collidingGameObject.ObjectRectangle.Left)
                    {
                        this.RightCollide = true;
                        // The map will scroll in the direction opposite of how it should be scrolling, reducing the movement to zero.
                        //gameWorld.checkMapScrollRight();
                    }
                }

                // If the character's left is right of the tile's left
                else if (this.ObjectRectangle.X > collidingGameObject.ObjectRectangle.X)
                {
                    // If the character's left is right of or touching the tile's right-hand side.
                    if (this.ObjectRectangle.Left <= collidingGameObject.ObjectRectangle.Right)
                    {
                        this.LeftCollide = true;
                        // The exact same thing as above
                        // To prevent the tile from falling through the floor, the tile will be raised up, cancelling the downward movement.
                        if (!Airborne)
                        {
                            Rectangle tempRectY = this.ObjectRectangle;
                            tempRectY.Y = (collidingGameObject.ObjectRectangle.Top) - tempRectY.Height;
                            this.ObjectRectangle = tempRectY;
                        }
                    }
                }
            }
        }

        public void CheckMovementInput()
        {
            //The player can only move when the game is not paused
            if (isPaused == false)
            {
                // If the input is the right arrow
                if (userInputObject.UserInputState.IsKeyDown(Keys.Right))
                {
                    //While the player isn't colliding
                    if (rightCollide != true)
                    {
                        //The world will only scroll right if the player is on the right edge of the screen
                        if (objectRectangle.X >= 650)
                        {
                            // When the user wants to move right, the character does not move. Instead, the world moves in the opposite direction, left, to give the appearance of character movement 
                            leftScroll = true;
                            rightScroll = false;
                            // Sets the animation to walk right              
                        }
                        //Otherwise, the player will actually move
                        else
                        {
                            Rectangle tempRect = objectRectangle;
                            tempRect.X = tempRect.X + 5;
                            objectRectangle = tempRect;
                        }
                        //The opposite side's collision boll will be set to false
                        leftCollide = false;
                    }
                }

                //Same as with the above code
                // If the user input is left
                if (userInputObject.UserInputState.IsKeyDown(Keys.Left))
                {
                    if (leftCollide != true)
                    {
                        if (objectRectangle.X <= 350)
                        {
                            // The same as when pressing "right" but in the opposite direction
                            rightScroll = true;
                            leftScroll = false;
                            // Sets the animation to walk left  
                        }
                        else
                        {
                            Rectangle tempRect = objectRectangle;
                            tempRect.X = tempRect.X - 5;
                            objectRectangle = tempRect;
                        }
                        rightCollide = false;
                    }
                }
            }
        }

        // Checks the input of the player to determine the player's currentSpriteState
        public void ChangeAnimationState()
        {
            // If the key is held left, the player's sprite will face left
            if (userInputObject.UserInputState.IsKeyDown(Keys.Left))
            {
                previousSpriteState = currentSpriteState;
                this.currentSpriteState = Character.spriteState.WalkRight;
            }

            // If the key is held right, the player's sprite will face right
            if (userInputObject.UserInputState.IsKeyDown(Keys.Right))
            {
                previousSpriteState = currentSpriteState;
                this.currentSpriteState = Character.spriteState.WalkLeft;
            }

            // If neither left or right is being held, the player's previousSpriteState is checked
            if ((userInputObject.UserInputState.IsKeyUp(Keys.Left)) && (userInputObject.UserInputState.IsKeyUp(Keys.Right)))
            {
                // Sets the previous sprite state to the current sprite state
                previousSpriteState = currentSpriteState;

                // If the player was previously walking right, the currentSpriteState is set to RightIdle
                if (previousSpriteState == spriteState.WalkRight)
                {
                    this.currentSpriteState = spriteState.RightIdle;
                }

                // If the player was previously walking left, the currentSpriteState is set to LeftIdle
                else if (previousSpriteState == spriteState.WalkLeft)
                {
                    this.currentSpriteState = spriteState.LeftIdle;
                }
            }
        }

        //The method to check if the player has completed the level
        //This will trigger if the player touches the right edge of the screen
        public void CheckWin()
        {
            if (this.objectRectangle.Right >= 1000)
            {
                this.playerWin = true;
            }
        }

        //The method to check if the player has died
        //This will triggerr either if the player has fallen off the map or if enemies have killed him or her
        public void CheckPlayerDeath()
        {
            if (this.objectRectangle.Top >= 600)
            {
                this.playerDeath = true;
            }
        }

        //The method to check if the player is colliding with the edges of the screen needs to stop moving
        public void CheckScreenCollision()
        {
            //Left-hand side
            if (this.objectRectangle.Left <= 0)
            {
                Rectangle tempRect = this.objectRectangle;
                tempRect.X = tempRect.X + 5;
                this.objectRectangle = tempRect;
            }

            //Right-hand side (allows the player to go offscreen and "win"
            if (this.objectRectangle.Right >= 1025)
            {
                Rectangle tempRect = this.objectRectangle;
                tempRect.X = tempRect.X - 5;
                this.objectRectangle = tempRect;
            }

            //Top (allows the player to jump above the screen a little bit)
            if (this.objectRectangle.Top <= -45)
            {
                Rectangle tempRect = this.objectRectangle;
                tempRect.Y = tempRect.Y + 5;
                this.objectRectangle = tempRect;
            }

            // No bottom check due to player death
        }

        // The jump method increments the player rectangle's Y value based on the jumpforce value
        // The jump height decrements based on how long the player is in the air
        public void Jump(int jumpTime)
        {
            for (double i = jumpForce; i > 0; i -= .4 / (jumpTime * .01))
            {
                this.objectRectangle.Y -= (int)i;
            }
        }

        // This method sets the boolean values of Jumping and Airborne based on user input
        public void checkJump(GameTime gameTime)
        {
            if (userInputObject.UserInputState.IsKeyDown(Keys.Space))
            {
                if (this.jumping == false)
                {
                    this.Airborne = true;
                    this.Jumping = true;
                }
            }
        }

        //checks to see if the previos y is lower than the current y, if so they are airborne
        public void CheckIfAirborne()
        {
            if (previousY < objectRectangle.Y)
            {
                airborne = true;
            }
        }

        //If the player needs to have his collision reset, both of the booleans that relate to collision are reset to false, meaning that the player is no longer colliding
        public void ResetCollision()
        {
            rightCollide = false;
            leftCollide = false;
        }

        // This Update method checks the state of the players jump.
        public override void Update(GameTime gameTime)
        {
            ChangeAnimationState();
            CheckIfAirborne();
            checkJump(gameTime); // this will check if the player is currently jumping or not (this.Jumping == true)

            //The plauer won't jump when the game is paused
            if (this.Jumping == true && isPaused == false)
            {
                jumpTime -= (int)gameTime.ElapsedGameTime.TotalMilliseconds; // This decrements the jumpTime based on the elapsed time in milliseconds

                if (jumpTime > 0)
                {
                    Jump(jumpTime); // When the jumpTime is greater than zero, the Jump method is called (the player moves upward)
                }

                if (jumpTime <= 0)
                {
                    this.Jumping = false; // When the jump time reaches 0 (after 'jumpTime' milliseconds), the player can't jump.
                }

                if (this.ObjectRectangle.Y > 0)
                {
                    this.Jumping = false; // If the top of the character exceeds the max height that it can jump, the player can't jump.
                }

            }

            if (this.Airborne == false)
            {
                jumpTime = 195; // If the player is no longer in the air, the jumpTime resets.
            }

            if (ObjectRectangle.Y <= 0) // Makes sure player doesn't fly up off screen
            {
                this.Jumping = false;
            }

            // If the key was not pressed, pressing 'F' will activate magic and set keypressed to true.
            // If the left arrow key is also pressed, shotL is set to true so that the magic is shot left.
            if (magic.MagicActive == false)
            {
                if (this.CurrentUserInput.UserInputState.IsKeyDown(Keys.F))
                {
                    magic.MagicActive = true;

                    // If the character is facing left (whether idle or moving), shotL is true and magic's objectTexture faces left
                    if (this.CurrentSpriteAnimationProp == Character.spriteState.WalkLeft || this.CurrentSpriteAnimationProp == Character.spriteState.LeftIdle)
                    {
                        magic.ShotLeft = true;
                        magic.ObjectTexture = Game.Content.Load<Texture2D>("magic"); // The magic sprite is drawn left if the player shoots left
                    }

                    // If the character is facing right (whether idle or moving), shotR is true and magic's objectTexture faces right
                    else if (this.CurrentSpriteAnimationProp == Character.spriteState.WalkRight || this.CurrentSpriteAnimationProp == Character.spriteState.RightIdle)
                    {
                        magic.ShotRight = true;
                        magic.ObjectTexture = Game.Content.Load<Texture2D>("magicLeft"); // The magic sprite is drawn right if the player shoots right
                    }
                }
            }

            // This will check if the magic has gone offscreen so that it can be removed
            if (magic.ObjectRectangle.X > 1000 || magic.ObjectRectangle.X < 0)
            {
                // Magic is set back to the center of the player
                magic.SetToCenter(this.ObjectRectangle.X, this.ObjectRectangle.Y);

                // Magic is no longer active so magicActive is set to false and shotL = false.
                magic.MagicActive = false;
                magic.ShotLeft = false;
            }

            magic.SetToCenter(this.ObjectRectangle.X, this.ObjectRectangle.Y);
            previousY = objectRectangle.Y;
        }
    }
}
