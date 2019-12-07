# SpotifyCurrentlyPlaying
A tool to show the current playing song on OBS (for streamers). How to show spotify music name on OBS.

## Download
[Download Here](https://github.com/bruxo00/SpotifyCurrentlyPlaying/releases/latest)

## Screenshots
##### Web Server
![WebS](https://i.imgur.com/d1lI4O0.png)

#### Text File
![Text](https://i.imgur.com/UwgEZrA.png)


## How to set up
There are two ways of using this. The first one is via Web Server. You just need to open the program, choose Web Server, then go on OBS and add a browser source on **http://localhost:5454/**, **width**: 1920, **height**: 1080, **Shutdown source when not visible** and **Refresh browser when scene becomes active** ticked. If you want to have the music to update quickly, just add **?refresh=1000** in front of the URL. This will update the music every **1000 ms (1 second)**. Now right click on the browser source, then Filters and add a new effect filter **Chroma Key**. Then on **Key Color Type** choose **Custom** and choose **Red (255, 0, 0)**. Also, depending on the design you choose, you may want to set **Smoothness** to 1.

The second one is by a text file. Open the program, choose Text File, enter the update time in seconds. Then go on OBS, create a new Text source, click on **Read From File**, go to the program folder, open the file **playing.txt** and click **OK**.


## Design
Edit the **display.html** and the **default.html** as you wish. The first one is shown when some music is playing, the second is shown when there is no music playing. Also, on the **display.html** use the tag **{MUSIC_NAME}** to let the program know where you want to display the music name in your page design.
