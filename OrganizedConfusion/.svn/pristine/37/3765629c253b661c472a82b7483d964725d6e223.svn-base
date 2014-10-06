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
    //The class that every tangible object in the game inherits from.
    //This class creates the ability for each object to have an object, position, rectangle, and if it is collidable or not, which the child classes can then alter to fit their specific operations. 
    class GameObject : DrawableGameComponent
    {
        #region Attributes

        // Attributes needed for every GameObject
        protected Texture2D objectTexture;
        protected Vector2 objectPosition;
        protected Rectangle objectRectangle;
        protected Boolean collidable;
        protected Boolean shouldDraw;

        #endregion

        #region Properties

        // Property for returning and assigning the objectTexture value
        public Texture2D ObjectTexture
        {
            get { return objectTexture; }
            set { objectTexture = value; }
        }

        // Property for returning and assigning the objectPosition value
        public Vector2 ObjectPosition
        {
            get { return objectPosition; }
            set { objectPosition = value; }
        }

        // Property for returning and assigning the objectRectangle value
        public Rectangle ObjectRectangle
        {
            get { return objectRectangle; }
            set { objectRectangle = value; }
        }

        // Property for returning and assigning the collidable value
        public Boolean CollidableProp
        {
            get { return collidable; }
            set { collidable = value; }
        }

        // Property for returning and assigning the shouldDraw value
        public bool ShouldDraw
        {
            get { return shouldDraw; }
            set { shouldDraw = value; }
        }
        #endregion

        // Constructor
        // The GameObject Constructor takes in a game
        public GameObject(Game game)
            : base(game)
        {

        }

        // Checks collision and returns boolean based on collision
        public bool CheckCollision(GameObject collidingGameObject)
        {
            // If the game object intersects a collidableGameObject's rectangle and the collidableGameObject's CollidableProp is true, the method returns true
            if (this.ObjectRectangle.Intersects(collidingGameObject.ObjectRectangle))
            {
                return true;
            }

            // Otherwise the method returns false
            else
            {
                return false;
            }
        }

        // The handleCollision virtual method handles a game object's collision
        public virtual void HandleCollision(GameObject collision)
        {
        }
    }
}

