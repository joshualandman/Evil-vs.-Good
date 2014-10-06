using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace EvilVsGoodClean
{
    //This is the class that primarily handles the world that the game takes place in. It also checks the collision for everything that is handled elsewhere as well creating and activating gravity
    class World : DrawableGameComponent
    {
        #region Attributes

        private static int gravity = 10; // sets the int gravity to 10 and will not change throughout the game;

        // A SpriteBatch object called worldSpriteBatch
        private SpriteBatch worldSpriteBatch;

        // The Array of Tiles that will form the 2D map the the player will traverse
        private GameWorldTile[,] gameWorldTileArray = new GameWorldTile[192, 12];

        //A temporary tile that is used to check if player should be able to overcome collision when jumping
        private GameWorldTile tempCollisTile = null;

        // Textures 
        // All textures will be loaded in to form the tiles' textures
        private Texture2D groundTile;
        private Texture2D grassTile;
        private Texture2D skyTile;
        private Texture2D enemyTile;
        private Texture2D bossTile;
        private Texture2D background; // The background texture will stand in as the background picture

        // Objects that will be implemented in the World class
        private UserInput userInputObject;
        private Rectangle worldRectangle;
        private Magic spell;
        private Boss levelBoss;
        private Player player;

        // A List of Enemies which will be loaded into the game
        List<Enemy> enemyList = new List<Enemy>();

        // The textures for the start screen, instructions screen, and pause screen, respectively
        // These will be the screens that appear when the user is navigating through the various menus
        private Texture2D titleScreen;
        private Texture2D instructScreen;
        private Texture2D pauseScreen;
        private Texture2D deathScreen;
        private Texture2D winScreen;
        private Texture2D expoScreen;

        // The booleans to see if specific keys are down
        private int keyBPressNumber = 0;
        private int keyEnterPressNumber = 0;

        //Creating a sound effect and an instance of that sound effect for the main game
        private SoundEffect gameMusic;
        private SoundEffectInstance gameMusicInstance;

        //The same as above, except for the boss music
        private SoundEffect bossMusic;
        private SoundEffectInstance bossMusicInstance;

        // All the screens that the user will encounter are set in an enum called ScreenState
        enum ScreenState
        {
            StartScreen,
            InstructScreen,
            PauseScreen,
            DeathScreen,
            WinScreen,
            ExpositScreen,
            LevelOneScreen
        }

        // The object screenState is an instance of ScreenState that is currently set to the Start Screen
        ScreenState screenState = ScreenState.StartScreen;

        // Determines whether or not the game has started or not with a Boolean value
        private bool gameStarted;
        #endregion

        #region Properties

        // Property for returning and assigning the gameWorldTileArray value
        // This will be used for loading in the game world based on the tiles in the array
        public GameWorldTile[,] MapTileArray
        {
            get { return gameWorldTileArray; }
            set { gameWorldTileArray = value; }
        }

        // Property for returning and assigning the groundTile value
        public Texture2D GroundTile
        {
            get { return groundTile; }
            set { groundTile = value; }
        }

        // Property for returning and assigning the skyTile value
        public Texture2D SkyTile
        {
            get { return skyTile; }
            set { skyTile = value; }
        }

        // Property for returning the enemyList value
        public List<Enemy> EnemyList
        {
            get { return enemyList; }
        }
        #endregion

        // Constructor
        // The World Class Constructor contains instances of Game, Player, Physics, UserInput, Magic, and Boss.
        // The worldRectangle is also set within the constructor, laying out the dimensions of the World that is to be drawn.
        public World(Game game, Player plyr, UserInput uI, Magic magicSpell, Boss bss)
            : base(game)
        {
            player = plyr;
            userInputObject = uI;
            spell = magicSpell;
            levelBoss = bss;
            worldRectangle = new Rectangle(0, 0, 1000, 600);

            gameMusic = this.Game.Content.Load<SoundEffect>("mainGameMusic");
            gameMusicInstance = gameMusic.CreateInstance();

            bossMusic = this.Game.Content.Load<SoundEffect>("bossMusic");
            bossMusicInstance = bossMusic.CreateInstance();
        }

        public override void Initialize()
        {
            //Seets the volume of both instances of sound
            //In addition, both instances are set to loop
            gameMusicInstance.Volume = 0.1f;
            gameMusicInstance.IsLooped = true;

            bossMusicInstance.Volume = 0.2f;
            bossMusicInstance.IsLooped = true;

            base.Initialize();
        }
        // Applies gravity to the characters in the game 
        // This will eventually take in an array of characters and apply it to all of them
        public void Gravity(Character character)
        {
            Rectangle tempRectangle = character.ObjectRectangle;
            tempRectangle.Y += gravity;
            character.ObjectRectangle = tempRectangle;
        }

        // Load Content
        protected override void LoadContent()
        {
            // Loading the specific textures to be used for the tiles that make up the map
            groundTile = this.Game.Content.Load<Texture2D>("dirt");
            grassTile = this.Game.Content.Load<Texture2D>("grass");
            skyTile = this.Game.Content.Load<Texture2D>("SkyTile");
            enemyTile = this.Game.Content.Load<Texture2D>("enemySheet");
            bossTile = this.Game.Content.Load<Texture2D>("princess");

            // Loading in the textures used for the menu screens
            titleScreen = this.Game.Content.Load<Texture2D>("Start Screen");
            instructScreen = this.Game.Content.Load<Texture2D>("Instruction Screen");
            pauseScreen = this.Game.Content.Load<Texture2D>("Pause Screen");
            background = this.Game.Content.Load<Texture2D>("background2");
            deathScreen = this.Game.Content.Load<Texture2D>("Death Screen");
            winScreen = this.Game.Content.Load<Texture2D>("Victory Screen");
            expoScreen = this.Game.Content.Load<Texture2D>("Exposition Screen");

            // Calling the load world method that will create the world with the file path
            this.LoadWorldIn(this.Game, @"..\..\..\Level Map Files\Level1Map.txt");

            // Setting the object 
            worldSpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        // Load up the tiles that comprise the game world
        #region Load World In

        // The LoadWorldIn method takes a text file and loads up tiles onto the screen depending on the symbols found within the text document that is loaded
        // The game loads in enemies and a boss in via the level's text file in much the same way as it does with the ground tiles
        public void LoadWorldIn(Game game, string file)
        {
            // A StreamReader object called mapReader which will be used to read the map file
            StreamReader mapReader = null;

            try
            {
                // Setting mapReader to a new StreamReader
                mapReader = new StreamReader(file);

                //Creating a string that will get and hold a specific line in ther text file.
                String line = ("");

                //Row
                int counter = 0;
                while (line != null)
                {
                    line = mapReader.ReadLine();
                    if (line != null)
                    {
                        //Parse the text file line into individual letters
                        String[] data = line.Split(',');

                        //Column
                        for (int j = 0; j < 192; j++)
                        {
                            if (data[j] == "D")
                            {
                                //The position of the tile's rectangle is assigned by multiplying the current j (X) and counter (Y)
                                gameWorldTileArray[j, counter] = new GameWorldTile(this.Game, groundTile, new Rectangle(50 * j, 50 * counter, 50, 50), true);

                            }

                            else if (data[j] == "G")
                            {
                                //The position of the tile's rectangle is assigned by multiplying the current j (X) and counter (Y)
                                gameWorldTileArray[j, counter] = new GameWorldTile(this.Game, grassTile, new Rectangle(50 * j, 50 * counter, 50, 50), true);

                            }

                            //When the map file contains an "E" at the location, an enemy will be drawn
                            else if (data[j] == "E")
                            {
                                //The enemyList will have an object (enemy) added to it for future referencing of all enemies
                                enemyList.Add(new Enemy(this.Game));

                                //The added enemy's rectangle's X and Y are set to the coordinates of the tile
                                Rectangle tempRectX = enemyList.ElementAt(enemyList.Count - 1).ObjectRectangle;
                                tempRectX.X = 50 * j;
                                enemyList.ElementAt(enemyList.Count - 1).ObjectRectangle = tempRectX;

                                Rectangle tempRectY = enemyList.ElementAt(enemyList.Count - 1).ObjectRectangle;
                                tempRectY.Y = 50 * counter;
                                enemyList.ElementAt(enemyList.Count - 1).ObjectRectangle = tempRectY;

                                //The starting position of the enemy will be copied and stored for future reloads and reference
                                enemyList.ElementAt(enemyList.Count - 1).StartPositX = enemyList.ElementAt(enemyList.Count - 1).ObjectRectangle.X;
                                enemyList.ElementAt(enemyList.Count - 1).StartPositY = enemyList.ElementAt(enemyList.Count - 1).ObjectRectangle.Y;

                                //Where the enemy would be, a sky tile is added so that the tile array is not incomplete
                                gameWorldTileArray[j, counter] = new GameWorldTile(this.Game, skyTile, new Rectangle(50 * j, 50 * counter, 50, 50), false);
                            }

                            //The load portion for the boss
                            else if (data[j] == "B")
                            {
                                //The earlier created instance of boss will be addeed to and implemented

                                //The added boss's rectangle's X and Y are set to the coordinates of the tile
                                Rectangle tempRectX = levelBoss.ObjectRectangle;
                                tempRectX.X = 50 * j;
                                levelBoss.ObjectRectangle = tempRectX;

                                Rectangle tempRectY = levelBoss.ObjectRectangle;
                                tempRectY.Y = 50 * counter;
                                levelBoss.ObjectRectangle = tempRectY;

                                //The starting position of the boss will be copied and stored for future reloads and reference
                                levelBoss.StartPositX = levelBoss.ObjectRectangle.X;
                                levelBoss.StartPositY = levelBoss.ObjectRectangle.Y;

                                //Where the boss would be, a sky tile is added so that the tile array is not incomplete
                                gameWorldTileArray[j, counter] = new GameWorldTile(this.Game, /*worldPhysics,*/ skyTile, new Rectangle(50 * j, 50 * counter, 50, 50), false);
                            }
                            else
                            {
                                //Otherwise, the tile is set as a "sky" tile that is not intersectable.
                                //Later on, there will be other tiles to be denoted by letters.
                                gameWorldTileArray[j, counter] = new GameWorldTile(this.Game, /*worldPhysics,*/ skyTile, new Rectangle(50 * j, 50 * counter, 50, 50), false);
                            }
                        }
                        counter++;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error: " + e.Message);
            }
            finally
            {
                //If the map reader has not bugged out and is null (otherwise a crash would occure)
                if (mapReader != null)
                {
                    //Regardless of a crash or not, the loader closes down
                    mapReader.Close();
                }
            }
        }
        #endregion

        // The methods that handle scrolling the world according to how the player is moving
        #region Scrolling
        // Handling moving the player (actually the map) from left to right
        public void checkMapScrollRight()
        {

            // The map will only scroll right if the left-most tiles are not past the left side of the screen
            if ((MapTileArray[0, 0].ObjectRectangle.X < 0))
            {
                for (int i = 0; i < 192; i++)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        // Create a temporary rectangle to store the current tile that needs to be moved
                        // The individul X and Y values for the tiles' rectangles cannot be altered via properties so a new, temporary rectangle must be created, assigned, altered, then reassigned to alter specific points of the tile.
                        Rectangle tempRect;
                        // Setting the temporary rectangle to the desired tile
                        tempRect = MapTileArray[i, j].ObjectRectangle;
                        // Incrementing the X value of the tempRect so that it moves right by 5;
                        tempRect.X = tempRect.X + 5;
                        // Setting each tile rectangle equal to the now advanced tempRect.
                        MapTileArray[i, j].ObjectRectangle = tempRect;
                    }
                }
                // Every enemy in the enemy list moves with the world either left or right (same for below)
                for (int i = 0; i < enemyList.Count; i++)
                {
                    Rectangle tempRectX = enemyList.ElementAt(i).ObjectRectangle;
                    tempRectX.X = tempRectX.X + 5;
                    enemyList.ElementAt(i).ObjectRectangle = tempRectX;
                }

                // The boss also moves in the same way as the enemies for both right and left scrolling
                Rectangle bossTempRectX = levelBoss.ObjectRectangle;
                bossTempRectX.X = bossTempRectX.X + 5;
                levelBoss.ObjectRectangle = bossTempRectX;

                // This will adjust the boss attack rectangles with the screen
                Rectangle bossAttackTempRectX = levelBoss.Attack.ObjectRectangle;
                bossAttackTempRectX.X = bossAttackTempRectX.X + 5;
                levelBoss.Attack.ObjectRectangle = bossAttackTempRectX;


            }

            // Otherwise, the player will actually move instead of the screen
            else
            {
                Rectangle tempRect = player.ObjectRectangle;
                tempRect.X = tempRect.X - 5;
                player.ObjectRectangle = tempRect;
            }
        }

        // Handling moving the player (actually the map) from right to left
        public void checkMapScrollLeft()
        {
            // If the right-most tiles are left of the right edge of the screen, the left scrolling will stop
            if (MapTileArray[172, 0].ObjectRectangle.X > 0)
            {
                for (int i = 0; i < 192; i++)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        // See above
                        Rectangle tempRect;
                        // See above
                        tempRect = MapTileArray[i, j].ObjectRectangle;
                        // The temporary rectangle's X will be negatively incremented so that it moves left
                        tempRect.X = tempRect.X - 5;
                        // See above
                        MapTileArray[i, j].ObjectRectangle = tempRect;
                    }
                }

                //Same as above
                for (int i = 0; i < enemyList.Count; i++)
                {
                    Rectangle tempRectX = enemyList.ElementAt(i).ObjectRectangle;
                    tempRectX.X = tempRectX.X - 5;
                    enemyList.ElementAt(i).ObjectRectangle = tempRectX;
                }

                //Same as above
                Rectangle bossTempRectX = levelBoss.ObjectRectangle;
                bossTempRectX.X = bossTempRectX.X - 5;
                levelBoss.ObjectRectangle = bossTempRectX;

                Rectangle bossAttackTempRectX = levelBoss.Attack.ObjectRectangle;
                bossAttackTempRectX.X = bossAttackTempRectX.X - 5;
                levelBoss.Attack.ObjectRectangle = bossAttackTempRectX;
            }

            //Same as above

            else
            {
                Rectangle tempRect = player.ObjectRectangle;
                tempRect.X = tempRect.X + 5;
                player.ObjectRectangle = tempRect;
            }
        }
        #endregion

        #region ScreenStates
        // The method that will handle screen transition
        public void CheckScreenState()
        {
            // Starting the game from the start screen
            if (screenState == ScreenState.StartScreen && userInputObject.UserInputState.IsKeyDown(Keys.Enter))
            {
                screenState = ScreenState.ExpositScreen;
                //The int for the key being down will be incremented to prevent the next screen from moving forward too quickly
                keyEnterPressNumber = 3;
            }

            // The Expositionscreen will show after the start screen is cleared
            if (screenState == ScreenState.ExpositScreen && userInputObject.UserInputState.IsKeyDown(Keys.Enter) && keyEnterPressNumber < 2)
            {
                screenState = ScreenState.LevelOneScreen;
                gameStarted = true;
            }

            // Getting to the instructions screen from either the pause or start screen
            if ((screenState == ScreenState.StartScreen || screenState == ScreenState.PauseScreen) && userInputObject.UserInputState.IsKeyDown(Keys.I))
            {
                screenState = ScreenState.InstructScreen;
            }

            // If the user wants to go back from the instruction screen, it will either for to the start screen or the pause screen
            if (screenState == ScreenState.InstructScreen && userInputObject.UserInputState.IsKeyDown(Keys.B))
            {
                if (gameStarted == false)
                {
                    screenState = ScreenState.StartScreen;
                }
                else
                {
                    // Just like with the start screen, the int for the B key will increment preventing back tracking from the instruction screen to the game screen
                    screenState = ScreenState.PauseScreen;
                    keyBPressNumber = 3;
                }
            }

            // If the player dies, the death screen will trigger
            if (player.PlayerDeath == true)
            {
                screenState = ScreenState.DeathScreen;
                spell.MagicActive = false;
            }

            // If the player wins and the boss is dead, the win screen will trigger
            if (levelBoss.IsAlive == false)
            {
                if (player.PlayerWin == true)
                {
                    screenState = ScreenState.WinScreen;
                }
            }

            // When the user either dies or wins the level, he or she is given the option of returning to the main menu and then playing the game again
            if ((screenState == ScreenState.DeathScreen || screenState == ScreenState.WinScreen) && userInputObject.UserInputState.IsKeyDown(Keys.B))
            {
                // Player booleans regarding win and death states are reset
                // The bool for determining if the player is playing is reset
                // The game is reset with the ResetGame function
                screenState = ScreenState.StartScreen;
                player.PlayerDeath = false;
                player.PlayerWin = false;
                gameStarted = false;
                ResetGame();
            }

            if (screenState == ScreenState.DeathScreen && userInputObject.UserInputState.IsKeyDown(Keys.Enter))
            {
                player.PlayerDeath = false;
                player.PlayerWin = false;
                gameStarted = false;
                ResetGame();
                gameStarted = true;
                screenState = ScreenState.LevelOneScreen;
            }

            // If the user presses P while playing, the game will pause
            if (screenState == ScreenState.LevelOneScreen && userInputObject.UserInputState.IsKeyDown(Keys.P))
            {
                screenState = ScreenState.PauseScreen;
            }

            // Going back from the pause screen
            if (screenState == ScreenState.PauseScreen && userInputObject.UserInputState.IsKeyDown(Keys.B) && keyBPressNumber < 2)
            {
                screenState = ScreenState.LevelOneScreen;
            }
        }
        #endregion

        // Seeing if the bool to determine to draw things should be flipped or not
        public void CheckShowAssets()
        {
            if (screenState == ScreenState.LevelOneScreen)
            {
                player.ShouldDraw = true;
                spell.ShouldDraw = true;
            }
            else
            {
                player.ShouldDraw = false;
                spell.ShouldDraw = false;
            }
        }

        public void CheckExitGame()
        {
            if (userInputObject.UserInputState.IsKeyDown(Keys.Escape))
            {
                this.Game.Exit();
            }
        }

        // Update method
        public override void Update(GameTime gameTime)
        {
            //If there is a temporary tile (as in the player is colliding with a tile)
            if (tempCollisTile != null)
            {
                //If the player is still technically colliding with a tile but is above or below that tile
                if ((player.ObjectRectangle.Bottom < tempCollisTile.ObjectRectangle.Top) || (player.ObjectRectangle.Top > tempCollisTile.ObjectRectangle.Bottom))
                {
                    //The player class will call its method to rest the collision, allowing the player to overcome collision and move left or right when he jumps or is above/below the tile
                    player.ResetCollision();
                    //The temporary tile is null, preventing collision from being reset at a time it shouldn't be
                    tempCollisTile = null;
                }
            }

            // The player will register as being paused if the screen is anything other than the level screen
            if (screenState == ScreenState.LevelOneScreen)
            {
                //The music only plays during the game's playable level
                gameMusicInstance.Play();
                player.IsPaused = false;
            }
            else
            {
                //The sound won't play if it paused but reset
                if (screenState == ScreenState.PauseScreen)
                {
                    gameMusicInstance.Pause();
                    bossMusicInstance.Pause();
                }
                //It stops when the game ends, due to player death or victory
                else if (screenState == ScreenState.DeathScreen || screenState == ScreenState.WinScreen)
                {
                    gameMusicInstance.Stop();
                    bossMusicInstance.Stop();
                }

                player.IsPaused = true;
            }

            // The B key will be registered as being down by incrementing the int value while it is down and setting that value to 0 while it is up
            //Thiis will prevent the user from skipping through multiple presses of the same key by holding it down
            if (userInputObject.UserInputState.IsKeyDown(Keys.B))
            {
                keyBPressNumber++;
            }
            else
            {
                keyBPressNumber = 0;
            }

            // Same as above but with the Enter key
            if (userInputObject.UserInputState.IsKeyDown(Keys.Enter))
            {
                keyEnterPressNumber++;
            }
            else
            {
                keyEnterPressNumber = 0;
            }
            // Check if the player is dead, victorius, screen collision, and movement
            player.CheckPlayerDeath();
            player.CheckWin();
            player.CheckScreenCollision();
            player.CheckMovementInput();

            // The boss's attack will register as being paused if the game is not in the levelone state
            if (screenState != ScreenState.LevelOneScreen)
            {
                //The game will only allow the player to exit if it is not on the Level, Instruction, or Exposition screens
                if (screenState != ScreenState.InstructScreen && screenState != ScreenState.ExpositScreen)
                {
                    CheckExitGame();
                }
                levelBoss.Attack.IsPaused = true;
            }
            else
            {
                levelBoss.Attack.IsPaused = false;
            }

            // The player movement, both left and right, (in that the screen scrolls) is triggered in the world class
            if (player.LeftScroll == true)
            {
                checkMapScrollLeft();
            }
            // To prevent the screen from continuously scrolling, the scroll boolean of the player is falsified
            player.LeftScroll = false;
            if (player.RightScroll == true)
            {
                checkMapScrollRight();
            }
            player.RightScroll = false;
            // When the game is started, collision for most of the objects are checked
            if (gameStarted == true)
            {
                // Every enemy in the enemy list is checked for collision with the player and collision with the magic
                foreach (Enemy badGuy in enemyList)
                {
                    badGuy.Walk(gameTime);
                    if ((badGuy.ObjectRectangle.X - player.ObjectRectangle.X < 150 && badGuy.ObjectRectangle.X - player.ObjectRectangle.X > -150) && (badGuy.ObjectRectangle.Y - player.ObjectRectangle.Y < 150) && badGuy.ObjectRectangle.Y - player.ObjectRectangle.Y > -150)
                    {
                        if (badGuy.CheckCollision(player) == true)
                        {
                            player.HandleCollision(badGuy);
                        }
                    }

                    // When the magic spell is active (firing), the enemy will check if it is colliding with the magic
                    if (spell.MagicActive == true)
                    {
                        // If it is, both the enemy and the spell will handle their respective collision methods
                        if (badGuy.CheckCollision(spell) == true)
                        {
                            badGuy.HandleCollision(spell);
                            spell.HandleCollision(badGuy);
                            // Magic is set back to the center of the player
                            spell.SetToCenter(player.ObjectRectangle.X, player.ObjectRectangle.Y);
                        }
                    }

                    // When the enemy is killed, his rectangle is moved off screen so it will no longer be in play
                    if (badGuy.IsAlive == false)
                    {
                        Rectangle tempRectX = badGuy.ObjectRectangle;
                        tempRectX.X = 5000;
                        badGuy.ObjectRectangle = tempRectX;
                    }
                }

                // Checks if the player collides with the tile
                foreach (GameWorldTile tileInWorld in gameWorldTileArray)
                {
                    if (tileInWorld.CollidableProp == true)
                    {
                        //The only tiles that are checked for collision with the player are those that are within 3 tile-lengths of the player
                        if ((tileInWorld.ObjectRectangle.X - player.ObjectRectangle.X < 150 && tileInWorld.ObjectRectangle.X - player.ObjectRectangle.X > -150) && (tileInWorld.ObjectRectangle.Y - player.ObjectRectangle.Y < 150) && tileInWorld.ObjectRectangle.Y - player.ObjectRectangle.Y > -150)
                        {
                            if (tileInWorld.CheckCollision(player) == true)
                            {
                                //If one of those tiles are colliding with the player, the player will handle the collision accordingly
                                player.HandleCollision(tileInWorld);
                                //In addition, the temporary tile is set to the tile the player is currently colliding with, for use with the player's "ResetCollision" method
                                tempCollisTile = tileInWorld;
                            }
                        }
                    }
                }

                // Checks if the magic collides with the tiles
                foreach (GameWorldTile tileInWorld in gameWorldTileArray)
                {
                    if (tileInWorld.CollidableProp == true)
                    {
                        if (tileInWorld.CheckCollision(spell) == true)
                        {
                            spell.HandleCollision(tileInWorld);
                            // Magic is set back to the center of the player
                            spell.SetToCenter(player.ObjectRectangle.X, player.ObjectRectangle.Y);
                        }
                    }
                }

                // When the player fires magic, it will check the boss's collision
                if (spell.MagicActive == true)
                {
                    if (levelBoss.CheckCollision(spell) == true)
                    {
                        levelBoss.HandleCollision(spell);
                        spell.HandleCollision(levelBoss);
                        // Magic is set back to the center of the player
                        spell.SetToCenter(player.ObjectRectangle.X, player.ObjectRectangle.Y);
                    }
                }

                // The boss and player will check if they are colliding and thusly kill the player
                if (levelBoss.CheckCollision(player) == true)
                {
                    player.HandleCollision(levelBoss);
                }

                // If the boss dies, it's rectangle will be sent off map to prevent collisions from the grave
                if (levelBoss.IsAlive == false)
                {
                    Rectangle tempRectX = levelBoss.ObjectRectangle;
                    tempRectX.X = 5000;
                    levelBoss.ObjectRectangle = tempRectX;
                }

                // While the boss is alive and on the screen 
                if (levelBoss.IsAlive == true && levelBoss.ObjectRectangle.X < 1000)
                {
                    // And while the game is actually playing
                    if (screenState == ScreenState.LevelOneScreen)
                    {
                        //When the player fights the boss, the game music stops and the boss music starts 
                        gameMusicInstance.Stop();
                        bossMusicInstance.Play();
                        // The boss will attack and the player will make sure if they are colliding or not
                        levelBoss.AttackPattern();

                        if (levelBoss.Attack.CheckCollision(player) == true)
                        {
                            player.HandleCollision(levelBoss.Attack);
                        }
                    }
                }

                // Check to see if the boss needs to die
                levelBoss.CheckBossDeath();
                if (levelBoss.IsAlive == false)
                {
                    //When the boss dies, the boss music stops
                    bossMusicInstance.Stop();
                }

                // Gravity will only act on the player if the game is not paused
                if (player.IsPaused == false)
                {
                    Gravity(player);
                }
            }

            //Continuously make sure that the right screens are showing and everything that should be on screen is
            CheckScreenState();
            CheckShowAssets();
        }

        // The method that places eveything in the starting position or states when the user tries to play the game more than once in the same runtime
        public void ResetGame()
        {
            // Resetting the player's x and y position 
            Rectangle tempRectX = player.ObjectRectangle;
            tempRectX.X = 200;
            player.ObjectRectangle = tempRectX;

            Rectangle tempRectY = player.ObjectRectangle;
            tempRectY.Y = 540;
            player.ObjectRectangle = tempRectY;

            player.LeftCollide = false;
            player.RightCollide = false;

            // Scrolling the map tiles back the the starting position
            while (gameWorldTileArray[0, 0].ObjectRectangle.X < 0)
            {
                checkMapScrollRight();
            }

            // When the game restarts, every enemy in the game will, if no longer at its original position, will be moved back to its initial position
            foreach (Enemy enemy in enemyList)
            {
                //For the enemies' X values
                if (enemy.StartPositX != enemy.ObjectRectangle.X)
                {
                    Rectangle enemyTempRectX = enemy.ObjectRectangle;
                    enemyTempRectX.X = enemy.StartPositX;
                    enemy.ObjectRectangle = enemyTempRectX;
                }

                //For the enemies' Y values
                if (enemy.StartPositY != enemy.ObjectRectangle.Y)
                {
                    Rectangle enemyTempRectY = enemy.ObjectRectangle;
                    enemyTempRectY.Y = enemy.StartPositY;
                    enemy.ObjectRectangle = enemyTempRectY;
                }

                // Every enemy is set to be alive
                enemy.IsAlive = true;
            }

            //Just like the enemies. the boss's position is reset to its original position
            if (levelBoss.StartPositX != levelBoss.ObjectRectangle.X)
            {
                Rectangle bossTempRectX = levelBoss.ObjectRectangle;
                bossTempRectX.X = levelBoss.StartPositX;
                levelBoss.ObjectRectangle = bossTempRectX;
            }

            if (levelBoss.StartPositY != levelBoss.ObjectRectangle.Y)
            {
                Rectangle bossTempRectY = levelBoss.ObjectRectangle;
                bossTempRectY.Y = levelBoss.StartPositY;
                levelBoss.ObjectRectangle = bossTempRectY;
            }

            //Reset the boss's health and setting her to be alive
            levelBoss.Health = 5;
            levelBoss.IsAlive = true;
        }

        // Draw everything related to the world
        public override void Draw(GameTime gameTime)
        {
            worldSpriteBatch.Begin();

            // Drawing the background image that will not be interactable
            if (screenState == ScreenState.StartScreen)
            {
                worldSpriteBatch.Draw(titleScreen, new Vector2(0, 0), Color.White);
            }
            // Draw the exposition screen
            else if (screenState == ScreenState.ExpositScreen)
            {
                worldSpriteBatch.Draw(expoScreen, new Vector2(0, 0), Color.White);
            }
            // Draw the instruction screen
            else if (screenState == ScreenState.InstructScreen)
            {
                worldSpriteBatch.Draw(instructScreen, new Vector2(0, 0), Color.White);
            }
            // Draw the pause screen
            else if (screenState == ScreenState.PauseScreen)
            {
                worldSpriteBatch.Draw(pauseScreen, new Vector2(0, 0), Color.White);
            }
            // Death screen
            else if (screenState == ScreenState.DeathScreen)
            {
                worldSpriteBatch.Draw(deathScreen, new Vector2(0, 0), Color.White);
            }
            // Win screen
            else if (screenState == ScreenState.WinScreen)
            {
                worldSpriteBatch.Draw(winScreen, new Vector2(0, 0), Color.White);
            }
            // Draw the game screen
            else if (screenState == ScreenState.LevelOneScreen)
            {
                // Drawing the background image that will not be interactable
                worldSpriteBatch.Draw(background, new Vector2(0, 0), Color.White);

                // For every tile to be found in the tile 2D array
                for (int i = 0; i < 192; i++)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        if (gameWorldTileArray[i, j].ObjectRectangle.Intersects(worldRectangle))
                        {
                            // If the texture of the current tile is that of the unrequired "Sky" texture, then the tile won't be drawn
                            if ((gameWorldTileArray[i, j].ObjectTexture != skyTile))
                            {
                                // The tiles are drawn based on what texture they have been assigned
                                // Their position is determined by their rectangle's X and Y position
                                worldSpriteBatch.Draw(gameWorldTileArray[i, j].ObjectTexture, new Vector2(gameWorldTileArray[i, j].ObjectRectangle.X, gameWorldTileArray[i, j].ObjectRectangle.Y), Color.White);
                            }
                        }
                    }
                }
                // Every enemy in the enemy list is drawn
                for (int i = 0; i < enemyList.Count; i++)
                {
                    if (enemyList.ElementAt(i).IsAlive == true)
                    {
                        worldSpriteBatch.Draw(enemyTile, enemyList.ElementAt(i).ObjectRectangle, enemyList.ElementAt(i).CharacterAnimation.ChangeAnimation(gameTime), Color.White);
                    }
                }

                // Draw the boss when needed
                if (levelBoss.IsAlive == true)
                {
                    worldSpriteBatch.Draw(bossTile, levelBoss.ObjectRectangle, levelBoss.CharacterAnimation.ChangeAnimation(gameTime), Color.White);
                }
            }

            worldSpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
