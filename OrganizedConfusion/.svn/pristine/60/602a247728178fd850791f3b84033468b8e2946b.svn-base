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
    class UserInput : GameComponent
    {
        // Attributes
        // Create a field for the world and the player
        private KeyboardState userInputState;

        public KeyboardState UserInputState
        {
            get { return userInputState; }
        }

        // Constructor
        // The constructor that takes in a game 
        public UserInput(Game game)
            : base(game)
        {
        }

        // The Update method updates the keyboard state by updating the check input method
        public override void Update(GameTime gameTime)
        {
            // Calls user input
            CheckInput(gameTime);
        }

        // Method that checks the user input
        public void CheckInput(GameTime gameT)
        {
            // Sets a keyboard state
            userInputState = Keyboard.GetState();
        }
    }
}
