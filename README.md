# SeniorProjectGame
## WHY THIS WAS CREATED: 
This game was created during my final semester at the University of Houston-Downtown. It was created to work in conjunction with a data
gathering experiment. The player would wear an Empatica wrist device (similar to a watch) that gathered various bio-rhythmic readings from 
the player.  The game was designed to either be really easy or get frustratingly hard so I could take the readings and see if there was 
an obvious pattern that could potentially be used for future experiments. 

## GAME OVERVIEW: 
The game itself spawns two enemy types, a runner and a warrior, whose goal is to get behind the player and destroy a wall the player is
guarding. If the wall is destroyed, it's game over. If the player is killed, it's game over.  The runner is faster than the warrior and only 
inflicts one unit of damage on either the player or the wall. The Warrior is slower and inflicts two units of data. There is a scoreboard
that keeps track of how many goblins the player kills.

Once the enemy gets behind the player they inflict their damage and disappear. The player moves on a fixed arc and must move to intercept
the enemy and kill them. 

The game is configured to run on a Windows PC.  

Controls: The space bar is used to swing the club.
Movement is done using the arrow keys, left arrow moving left, right arrow moving right. 

There are two versions of the game that were configured. One for Hard mode and one for easy mode. They were configured separately for 
the purposes of the study attached to the game.  By configuring them separately we could keep the subject from seeing which version they
were playing before they began.  

CREATION INFORMATION:
This game was created in a one and a half month period using Unity and coded in C#. Eloy Perez aided me with creating the difficulty 
settings as specified and with the enemy and player movement. 
Game Design, Asset creation, player and enemy attack code, score code, health code and data transferrance for Empatica code, adding sound effects, music, coding the animation, debugging: 
Robert Jackson

Enemy and player movement and Difficulty Settings: Eloy Perez

Goblin models and animations were created using blender. Background was created in Unity. 

The music and sound effects were found on sound-effects .org and created using effects uploaded with open licenses.

DIRECTORY INFORMATION:
The GoblinAssaultVersions folders let you download the easy and hard mode version of the game. Make sure everything is in the same directory and just double click to play the game. Note: It must be played on a windows pc. The game is so light that it should play on most versions of anything out today. 

The GoblinGame folder holds all the information needed to open the game in unity and view all of the assets and scripts. 
Note: When working on the game with Eloy we found that he could not see the models or animations unless he had Blender installed on his computer.  Blender is a free to install software.  The C# scripts should be viewable in Microsoft Visual Studio. 

CREDITS: 
The music:
URL: http://freemusicarchive.org/music/Visager/Songs_From_An_Unmade_World/Visager_-_Songs_from_an_Unmade_World_-_06_Pyramid_Level
Comments: http://freemusicarchive.org/
Curator: ccCommunity
Copyright: Attribution: http://creativecommons.org/licenses/by/4.0/
