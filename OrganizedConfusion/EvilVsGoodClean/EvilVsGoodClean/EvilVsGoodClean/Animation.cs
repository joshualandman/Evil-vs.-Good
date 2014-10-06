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
    class Animation
    {
        #region Attributes
        private Texture2D spriteSheet;
        private Character currentCharacter;
        //const int sprite_Y = 58;
        //const int sprite_Height = 31;
        //const int sprite_Width = 48;
        //const int sprite_Xoffset = 2;
        private int frameCount;
        protected Point currentFrame = new Point(0, 0);
        protected Point frameSize = new Point(32, 48);
        protected Point bossFrame = new Point(100, 150);
        bool active;
        int delayTimer;

        Rectangle sourceRectangle;

        protected TimeSpan nextFrameInterval = TimeSpan.FromSeconds((float)1 / 16);

        #endregion

        public Animation(Texture2D spritePicture, Character character)
        {
            currentCharacter = character;
            spriteSheet = spritePicture;
            active = true; // Sets animation to play at first.     
        }

        // Uses the source rectangle to move between frames.
        public Rectangle ChangeAnimation(GameTime gameTime)
        {
            delayTimer++; // Counter for the delay between frames.

            // Moves the frame count along as long as the animation is active.
            if (active)
            {
                frameCount += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            else
            {
                frameCount = 0;
            }

            if (delayTimer == 5)
            {
                //if the animation is set to walk left, cycle through frames
                if (currentCharacter.CurrentSpriteAnimationProp == Character.spriteState.WalkLeft)
                {
                    currentFrame.X++;
                    if (currentFrame.X >= 4)
                        currentFrame.X = 0;
                    if (currentFrame.Y != 1)
                    {
                        currentFrame.Y = 1;
                    }
                }

                //if the animeation is right, cycle through frames
                else if (currentCharacter.CurrentSpriteAnimationProp == Character.spriteState.WalkRight)
                {
                    currentFrame.X++;
                    if (currentFrame.X >= 4)
                        currentFrame.X = 0;
                    if (currentFrame.Y != 0)
                    {
                        currentFrame.Y = 0;
                    }
                }

                //if the animation is idle, cycle through frames
                else if (currentCharacter.CurrentSpriteAnimationProp == Character.spriteState.RightIdle)
                {
                    if (currentFrame.X <= 4)
                        currentFrame.X = 0;
                    if (currentFrame.Y != 0)
                    {
                        currentFrame.Y = 0;
                    }
                }

                //if the animation is idle left side, cycle through frames
                else if (currentCharacter.CurrentSpriteAnimationProp == Character.spriteState.LeftIdle)
                {
                    if (currentFrame.X <= 4)
                        currentFrame.X = 0;
                    if (currentFrame.Y != 1)
                    {
                        currentFrame.Y = 1;
                    }
                }

                else if (currentCharacter.CurrentSpriteAnimationProp == Character.spriteState.Princess)
                {
                    currentFrame.X++;
                    if (currentFrame.X <= 7)
                        currentFrame.X = 0;
                    if (currentFrame.Y != 0)
                    {
                        currentFrame.Y = 0;
                    }
                }
                delayTimer = 0;
            }

            if (currentCharacter is Boss)
            {
                return sourceRectangle = new Rectangle(bossFrame.X * currentFrame.X, bossFrame.Y * currentFrame.Y, bossFrame.X, bossFrame.Y);
            }
            else
            {
                return sourceRectangle = new Rectangle(frameSize.X * currentFrame.X, frameSize.Y * currentFrame.Y, frameSize.X, frameSize.Y);
            }
        }
    }
}

