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
    //The tiles that comprise the traversable map
    //The constructor sets up its rectangle, its texture, and if it is collidable
    class GameWorldTile : GameObject
    {
        // Constructor
        // The GameWorldTile Constructor takes in a game,  Texture2D, a Rectangle, and a Boolean
        public GameWorldTile(Game game, Texture2D tileText, Rectangle tileRect, bool canCollide)
            : base(game)
        {
            // Sets default texture, rectangle, and makes it collidable
            objectTexture = tileText;
            objectRectangle = tileRect;
            collidable = canCollide;
        }
    }
}
