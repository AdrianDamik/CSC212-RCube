# CSC212-RCube
**Rubik's Cube Simulation**

Project made in collaboration by Elijah Gray, Adrian Damik, & Aryan Pothanaboyina.

This program is a Rubik's cube solver visualization w/ three Rubik’s cube solving algorithm implementations integrated: the Fridrich/C.F.O.P method, the Kociemba method, and the Beginner/Layer method. The visualization also allows the user to manually pan and rotate the cube themselves using the mouse.

This program was utilized in a study comparing the three different algorithm implementations in a comparison study. One of our expectations was that the Kociemba algorithm would provide the shortest solutions, followed by the C.F.O.P method, and then the Beginner method. This expectation was met.

***An executable for this project (named "My Project.exe") is included in the "RCube Simulator" folder***


___


**Credits:** 

The visualization developed for this project was originally created by the github user Megalomatt (https://github.com/Megalomatt/unity-rcube). We were originally following his tutorial (https://www.youtube.com/watch?v=JN9vx0veZ-c); however we first found this user’s implementation of the tutorial from the user Hamzazmah (https://github.com/hamzazmah/RubiksCubeUnity-Tutorial). We built off this project and gradually used more code from the original project after finding the original implementation. This project also included an c# translation of the Kociemba algorithm translated by the original visualization creator.

The C.F.O.P method utilized in this project was integrated from a project created by the user Divinsmathew: https://github.com/divinsmathew/CubeXdotNet-Rubiks-Cube-Solver.

The Layer method utilized in this project was integrated from a project created by Uzi Granot from codeproject.org at: https://www.codeproject.com/Articles/1199528/Rubik-s-Cube-for-Beginners-Version-2-0-Csharp-Appl


___


**EXE Controls:**

Before shuffling the cube, a number of moves must be entered into the "Enter No of Shuffles..." text field. The minimum value entered should be 11 for accuracy.

After the cube is shuffled, select any of the three solvers (Kociemba, CFOP, or Layer) to begin solving the cube.

To enter a shuffle seed, enter a seed into the "Enter seed..." text field, then enter the number of moves into the "Enter steps..." field and press Enter key.

Speed controls are located at the lower right corner of the screen. Move the slider handle to the left to slow down, and to the right to speed up (default value for "real time" is 1).

Press the Pause button to pause the model, and press the Pause button again to resume. 

Press the Debug button to enable debug speed, and press the Debug button again to disable.

Click and drag on the cube model with Mouse Left Click to rotate the layers of the model selected.

Click and drag around the cube model with Mouse Right Click to pan around the model.

Press the Esc key to exit from the EXE.


___


**Unity installation for windows:**
1) Install Unity Hub via the Unity website (https://unity.com/download).

2) When Unity Hub has finished installation, open it. Unity hub will prompt you to log into a Unity account. Either sign into an existing account, or create a new one.

3) After signing into the Unity Hub, the Unity editor installation prompt will appear. SKIP THIS FOR NOW. 

4) After Unity Hub fully opens, click the "projects" tab, then click the "open" button at the top right corner of the Unity Hub window. This will open a file explorer window. Navigate to the location where the RCube simulation is installed, and navigate to the following folder: 
   <your installation location>\CSC212-RCube-main\CSC212-RCube-main\CSC212 RCube V4.1

5) A prompt to install Unity Editor Version 2021.3.18f1 will appear. Select the version labeled "missing version" from the options, and install. Uncheck "microsoft visual studio community 2019" from the Dev tools option, and proceed with install.

6) After installation of Editor is complete, open the project file in Unity Hub.

7) The initial scene for the project will be blank. To open the correct scene, click File > Open scene. In the file explorer window that opens, navigate to <your installation location>\CSC212 project\CSC212 RCube V4.1\Assets\Scenes, and select the scene named "SampleScene".

**DONE! The Unity half of the project should be set up at this point.**


___


**Visual Studio Code installation for windows:**

1) Install Visual Studio Code via the Visual Studio Website (https://code.visualstudio.com/download).

2) After installation, add the following extension from the Marketplace in Visual Studio Code:
   
   C# v1.25.4 (or newer) by Microsoft

**DONE! The visual studio code half of the project should be set up at this point.**
