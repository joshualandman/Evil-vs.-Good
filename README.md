Evil-vs.-Good
=============

The game I made for my class Game Development and Algorythmic Problem Solving II (September 2013 - December 2013).

Much like NightWatch, Evil vs. Good was a five-person student project to be completed over ten weeks. Fresh from my experience with NightWatch, I was much more cautious about
the game we were making, as was the rest of my group (one of them had worked with me on the previous game as well). We decided to go a much more traditional route and created 
a side-scrolling platformer with limited AI and simple movement, jumping, and shooting mechanics. 

Evil vs. Good was the opposite of my previous game, being a more grounded, traditional game with concrete, fun gameplay and strong functionality. Our game created a map from
a text file in a grid of blocks. The player was able to navigate this grid, shoot spells at enemies, collide with the terrain, and jump to avoid death-pits. By going with a 
simpler concept, we were able to spend much more time focusing on making our gameplay as fun and challenging as possible. In addition, our game contained a Main Menu, 
Exposition/Story screen, Instruction screen, Pause screen, Game Over screen, and Level Complete screen.
 
I contributed to the game in several ways. Firstly, I implemented the level loading functionality that allowed a new level to be created by simply creating a text file. As 
mentioned above, the level loader took a text file and places a tile in a space relative to where the character was in the file. Secondly, I laid the ground work for the 
collision system between the player, enemies, spells, and terrains. Others worked on it afterword, which I also helped with, but I laid the primary ground work. The 
collisions would execute if the two objects’ sides were touching (all sides were checked every turn). Finally, I implemented the end-level boss and her attack. The boss is 
positioned at the end of the level and begins attacking the player as soon as she appears on screen. The player must avoid the attacks (falling flowers) and shoot the boss 
with spells enough times to kill her. The player can then exit and complete the level.


