# Erda Link Simulator
# Erda Link WebGL Build

This is the Unity Project File of the Erda Link Simulator found [here](https://phantasmicsky.github.io/Erda-Link-WebGL-Build/).

## Bugfix
* \[2025-07-05\] Fixed an issue where deleveling a node does not update the Cost to Upgrade. 

## Features
* Allows users to simulate filling up their Erda Link board
* Allows users to calculate how many Sol Erdas and Sol Erda Fragments they would need to finish a chosen selection

## How to Use
This was directly taken from the Help Section of the Program.
* Hold Right Click and drag to move the board (Don't move too far I didn't set any bounds)
* Use the scroll wheel to zoom in or out of the board

The Simulator has 2 modes, <b>Link Mode</b> and <b>Free Calc Mode</b>. Press the "Link Mode" or "Free Calc Mode" button on the top right to switch modes.

### Link Mode
* Acts just like the one in game except that you can remove nodes you have invested erda in.
* Click on a stone to set it to active. Once active, the ring around the stone will glow the same color as the stone type and the stone's information will be displayed to the right.*
* If you left click on a currently active stone that is also unlocked, the stone's level will go up.
* If you right click on a currently active stone that is also unlocked, the stone's level will go down.

#### Known Issue in Link Mode
* Setting an early stone back to Lv.0 while having a very far stone activated/leveled might cause the very far stonee to remain active even though it shouldn't as the prerequisite stones haven't been activated. I honestly don't know how best to check this. I am just bad at programming those Q_Q.

### Free Calc Mode
* All stones can be selected regardless if the prerequisite stones are locked. This is for people who just need to know how much erda they would need.
* A single left click (there is no active state) will include the stonee in the computation \[Indicated by a yellow bar below the icon\].
* A single right click removes the stone from the computation \[Yellow bar disappears\]
* If a stone has a max level greater than 1, you can click the yellow bar and set the starting and ending level you would want to consider the cost for. (ex. You just want to get the cost to get Sol Janus from Lv. 17 to Lv. 24). The yellow bar contains the current level considerations for the stone. (ex. seeing a "2 > 7" means that only the cost to get from Lv.2 to Lv.7 is considered)
