Important message: The linux build is untested but could work.

# Hacker jeopardy
## _A visual presentation for Jeopardy style quiz games._

This software provides a method of creating, editing and presenting a jeopardy style quiz game up to 10 players.

## Requirements:
- A dual screen setup with a resolution of at least 1920*1080. (Will be forced) (One for the presenter, one for the audience/players. Windows and Linux supported).
- A USB or other removable storage device to save the questions to.
- (up to) 10 hardware buttons for the players to buzz.
- [optional] Something to output the sound to.

## Getting started
### Obtaining a binary to run:
- The easiest way to get going is to download the binaries straight from github (binaries_windows.zip or binaries_linux.tar) and unpack them either on disk or on a removable storage device.
- If you want to create your own build, clone the project and open it with unity3D v.2022.3.4f1 go to file->build settings, configure to your liking and create build.

### Creating a new game
- Start the game, you will be greeted with a static image on the (secondary) presentation screen and an operator view on the (primary) operator screen.
- Press 'new questions'. This will promt you to select a removable drive to save the file and a file name or a full path wherever you like to store your file, a game name and a tagline. The game name and tagline are used on the presentation screen to show while everyone waits for the game to start.
- Press create, this will take us to the question editor (which can also be accessed by pressing the 'edit questions' button)
- Now it's time to create some categories and questions. First start by creating a new category in the left part of the window by pressing the 'new' button.
- Give your category a name and a color (standard is blue) and press 'save/add'.
- Now you can add questions to your category in the right part of the screen. Give your question a: 
    - value (numeric only). 
    - type (Text/text+image/text+video).
    - A question to ask.
    - The answer to said question. 
    - [optional if type is text/image or text/video] a media parth to an image file or video file for the question and/or answer. (This has been tested with .jpg, .jpeg, .png for images, and .mp4 for videos, others might work too). 
    - [optional] A note: This is something the operator sees while presenting the question, use this to add some trivia or jokes to your presentation.
    - A question color (RGB). This is used on the board and as a background for when showing the question.
- Press 'save/add' on the right side of the screen.
- If you want to add another question, press 'New' on the questions side.
You can navigate questions and categories by selecting them from the dropdown menu's on top of the screen.
To stop editing press the X on the top right corner, This will also save the questions file.

### Loading an existing game

To load a game you made previously, press 'load questions' and select the drive/file or full path to the file you created earlier.

### Add some players

To play the game you need some players, press the 'add player' at the bottom left of the screen to add a player.

### Or teams

If you plan to play with teams and want to randomly pick some teams from a large list. Just create a file with the .teams extention on a removable drive with all team names seperated by a newline.
When the 'Team Selector' is selected, you are able to pick some ranom team names from the list. When returning to the home screen the teams are added as player names.

### Ready to start!

When ready to start the game, press 'start game' in the bottom right corner of the screen.

### first round

The first round is a bit different then other rounds, in this round we start off by presenting the categories. Optional you can wait with this to present the players to your audience.

#### Presenting the categories

Now we present the categories, The operator can now click 'Show next' on screen to show the next category, it will display a little animation on the presentation screen.
Repeat this step until all categories with their values are shown.

#### Picking a random player to choose a category

In the first round we select a random player to choose a category/value from the board. By clicking 'randomize' the game will do a little animation and choose one random player to select a category/value. The name will be lit up in the presentation screen and shows as 'player selecting:' on the operator view.

## Selecting a category and question

When the player makes his selection, the operator can select the category and value on screen. The presentation screen shows an animation when transitioning to the question.

## Showing the question

Once the question has loaded in the audience sees nothing. The operator reads the question and only when pressing 'open answers' the answer is shown on screen and the players can buzz for their answer. At this point the jeopardy waiting music start playing until someone buzzes in.

## Players answering

When a player presses their button, the operator screen shows what player is answering and 2 buttons (Wrong or correct). When pressing the WRONG button the value is deducted from the player's points, and another player may buzz in. When selecting 'correct' the player gains the value of the question. At this point the answer will be revealed. If no one answers or enough time has passed, the operator may press 'Show answer'. This will make sure no-one may answer and the answer is shown on screen.
At this point you can return to the board, if a player had the right answer, he or she may choose the next question. If not: the player that chose the previous question may choose again.

 ## Questions with video
 
 If a question has a video in it the operator is expected to press '(Re)Play media' to start the video. It will not start automatically. The question will only be revealed after pressing 'Open answers'.
 
 ## The end of the game
 
 When the last question is shown and played, returning to the board will show a screen with the winning player. The operator can see all players with their scores sorted to tell who finished in what position. When pressing 'to menu' the game goes back to the init screen where it shows the game name and tagline.
 
 ---
 
 # Creating the buttons:
 The buttons are designed to work wirelessly on a 9v battery. After using the buttons you might want to disconnect the 9v.
 
 ## The box
 In the folder hardware/boxes there is a design called boxdesign.svg. This is to be lasercutted from 3mm thick wood or plastic.
 ## The hardware
 For the harware we need:
 - 1 momentary pushbutton with a diameter of 12mm (https://s.click.aliexpress.com/e/_DFizbJV)
 - 1 ESP32 S2 mini
 - 1 5v buck converter to regulate the power going into the ESP S2 mini
 - 1 9v battery clip
 - 1 9v battery
 - 1 resistor (~10K Ohm) 
 
 ## Wiring
9V + to Buck converter IN +
9V - to Buck convertor IN -
Buck convertor OUT + to ESP32 VBUS
Buck convertor OUT - TO ESP32 GND
Button to ESP 14
Resistor between ESP14 and ESP 3.3v
Button to ESP GND

## The software
Flash the ESP32 S2 mini with the code found in the folder hardware/firmware/client, but before you do change the player index in the code (variable currentPlayer). 0-9 Player1 should be 0, player2 should be 1 etc... 

# creating a host receiver
For the host we use another ESP32 S2 mini, flash it with the code found in folder hardware/firmware/host and connect it to the system with a USB cable when in use.























