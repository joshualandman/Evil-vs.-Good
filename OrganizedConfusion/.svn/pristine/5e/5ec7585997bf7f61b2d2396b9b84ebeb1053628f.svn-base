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
    //The parent class that sets up all of the attributes for its child classes
    //The children of Character are enemy, boss, and player
    class Character : GameObject
    {
        #region Attributes

        // Attributes
        protected Animation characterAnimation;
        protected SpriteEffects currentSpriteEffects;
        protected SpriteBatch characterSpriteBatch;

        // Enumerate states for animation
        public enum spriteState { WalkRight, WalkLeft, Jump, RightIdle, LeftIdle, Attack, Princess };

        // Create a sprite state for the current sprite
        protected spriteState currentSpriteState = Character.spriteState.LeftIdle;
        protected spriteState previousSpriteState;

        #endregion

        #region Properties

        // Property for returning and assigning the Current Sprite
        public SpriteEffects CurrentSpriteEffectsProps
        {
            get { return currentSpriteEffects; }
            set { currentSpriteEffects = value; }
        }

        // Property for returning the Current Sprite's Animation
        public spriteState CurrentSpriteAnimationProp
        {
            get { return currentSpriteState; }
        }

        // Property for returning the Character's Animation object
        public Animation CharacterAnimation
        {
            get { return characterAnimation; }
        }
        #endregion

        // The constructor that takes in a game parameter
        public Character(Game game)
            : base(game)
        {
            collidable = true;
        }

        // The Load content method loads in both the player and enemy sprite sheets
        protected override void LoadContent()
        {
            // Set the character's sprite to the sprite sheet
            if (this is Player)
            {
                objectTexture = this.Game.Content.Load<Texture2D>("spriteSheet");
            }

            // Set the enemy's sprite to the sprite sheet
            if (this is Enemy)
            {
                objectTexture = this.Game.Content.Load<Texture2D>("enemySheet");
            }
            //Sets up the boss's sprite
            if (this is Boss)
            {
                objectTexture = this.Game.Content.Load<Texture2D>("princess");
            }

            characterSpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        // The Draw Method draws the character but only when shouldDraw is equal to true
        public override void Draw(GameTime gameTime)
        {
            // Begin the sprite batch
            characterSpriteBatch.Begin();
            //The game character will only draw when the world bool for show assets is true
            if (shouldDraw == true)
            {
                characterSpriteBatch.Draw(objectTexture, objectRectangle, characterAnimation.ChangeAnimation(gameTime), Color.White, 0, objectPosition, currentSpriteEffects, 0);
            }
            // End the sprite batch
            characterSpriteBatch.End();
        }
    }
}
