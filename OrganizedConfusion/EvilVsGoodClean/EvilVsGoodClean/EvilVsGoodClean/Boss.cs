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
    //The boss class is the child of the enemy class and takes all the attributes and methods of the class
    class Boss : Enemy
    {
        //The bosses health
        private int health = 5;
        //The instance of the attack
        private BossAttack attack;
        //An instance of the player used for spawninn in the attack on the right position, above the player
        private Player playerInstance;

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public BossAttack Attack
        {
            get { return attack; }
        }

        //The method that decrements the bosses health
        public void TakeDamage()
        {
            health--;
        }

        //If the boss's health is reduced to zero, the boss is marked as dead
        public void CheckBossDeath()
        {
            if (health <= 0)
            {
                this.IsAlive = false;
            }
        }

        //Constructor
        //The constructor takes an instance of BossAttack so that it can spawn in an instance of the attack when it (the boss) is on screen
        //It takes in an instance of the player so that the BossAttack can be moved to the player's X-value each time it is created
        public Boss(Game game,/* Physics phys,*/ BossAttack bsAttk, Player plyr)
            : base(game)//, phys)
        {
            collidable = true;
            attack = bsAttk;
            this.objectRectangle = new Rectangle(50, 50, 100, 150);
            characterAnimation = new Animation(objectTexture, this);
            this.IsAlive = true;
            playerInstance = plyr;
            currentSpriteState = spriteState.Princess;
        }

        //Overriding the handle collision method of the enemy so that the boss takes damage when magic hits it
        public override void HandleCollision(GameObject collidingGameObject)
        {
            if (collidingGameObject is Magic)
            {
                TakeDamage();
            }
        }

        //The method to attack the player
        public void AttackPattern()
        {
            //As long as the attack is not false
            if (attack.ActiveAttack == false)
            {
                //The attack is set to the player's position, preventing the player from hiding from the from the attacks
                attack.UseAttack(playerInstance.ObjectRectangle.X);
            }
        }
    }
}
