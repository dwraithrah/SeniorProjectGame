# SeniorProjectGame
## WHY THIS WAS CREATED: 
This game was created during my final semester at the University of Houston-Downtown. It was created to work in conjunction with a data
gathering experiment. The player would wear an Empatica wrist device (similar to a watch) that gathered various bio-rhythmic readings from 
the player.  The game was designed to either be really easy or get frustratingly hard so I could take the readings and see if there was 
an obvious pattern that could potentially be used for future experiments. 

## GAME OVERVIEW: 
The game itself spawns two enemy types, a runner and a warrior, whose goal is to get behind the player and destroy a wall the player is
guarding. If the wall is destroyed, it's game over. If the player is killed, it's game over.  The runner is faster than the warrior and only inflicts one unit of damage on either the player or the wall. The Warrior is slower and inflicts two units of damage. There is a scoreboard that keeps track of how many goblins the player kills. Once the enemy gets behind the player they inflict their damage and disappear. The player moves on a fixed arc and must move right or left to intercept the enemy and kill them.  
## INSTALLATION:
If you download this repository the game requires no installation. Simply go into the GoblinAssaultVersions folder, choose which version you wish to play (Easy mode or hard mode), go in that respective folder and double click on either GoblinAssaultE (if you go into the easy folder) or GoblinAssaultH (if you go into the hard folder).  From there a Game configuration screen will come up. You can choose screen resolution (its recommended you play at the default resolution), whether you want the game in full screen or windowed, and graphics quality.  You can also choose which monitor, if you have multiple, you want to play from. Then hit play and enjoy. 

## CONTROLS: 
The space bar is used to swing the club.
Movement is done using the arrow keys, left arrow moving left, right arrow moving right. 
 
## DEVELOPMENT INFORMATION:
This game was created in a one and a half month period with some help from a classmate and friend, Eloy Perez.  The game difficulty
settings were packaged separately, rather than make it an option in a game menu, so the participants in the resulting data collection
exercise wouldn't know which difficulty they were playing.

Game Design, Asset creation, player and enemy attack code, score code, health code and data transferrance for Empatica code, adding sound effects, music, coding the animation, debugging: 
Robert Jackson

Enemy and player movement and Difficulty Settings: Eloy Perez

Goblin models and animations were created using blender. Background was created in Unity. 

The music and sound effects were found on sound-effects .org and created using effects uploaded with open licenses.

The GoblinGame folder holds all the information needed to open the game in unity and view all of the assets and scripts. 
Note: When working on the game with Eloy we found that he could not see the models or animations unless he had Blender installed on his computer.  Blender is a free to install software.  The C# scripts should be viewable in Microsoft Visual Studio. 

CREDITS: 
The music:
URL: http://freemusicarchive.org/music/Visager/Songs_From_An_Unmade_World/Visager_-_Songs_from_an_Unmade_World_-_06_Pyramid_Level
Comments: http://freemusicarchive.org/
Curator: ccCommunity
Copyright: Attribution: http://creativecommons.org/licenses/by/4.0/
