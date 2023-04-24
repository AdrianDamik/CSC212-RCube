// Created by Megalomatt (https://github.com/Megalomatt/unity-rcube) (2020)
// Revised by hamzazmah (https://github.com/hamzazmah/RubiksCubeUnity-Tutorial) (2020)
// Revised by Elijah Gray & Adrian Damik (2023)

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  Originally created by the original developers & modified by Elijah & Adrian. Manages the majority of the program and instructs the program to execute movement commands 
///  and help determine whether the program should be moving. Additionaly it helps track the steps used in a solution used in Adrian's interface. - Elijah Gray
/// </summary>
public class Automate : MonoBehaviour
{

    /// <summary>
    ///  A list of strings to hold all the moves used by the automate object. 
    /// </summary>
    public static List<string> moveList = new List<string>();

    /// <summary>
    /// Copypasted from the original creator of the visualizer. A list of all possible rubiks cube moves in rubik's cube notation. -Elijah Gray
    /// </summary>
    private readonly List<string> allMoves = new List<string>()
    {
        "U", "D", "L", "R", "F", "B",
        "U2", "D2", "L2", "R2", "F2", "B2",
        "U'", "D'", "L'", "R'", "F'", "B'"
    }; // a list of all possible moves utilizable by this visualizer in rubiks cube notation.


    /// <summary>
    ///  used to grab the function to get the cubestate functionality.
    /// </summary>
    private CubeState cubeState;

    /// <summary>
    ///  used to grab the function to get the readcube functionality.
    /// </summary>
    private ReadCube readCube;


    /// <summary>
    ///  an array to hold all the pivots in the scene. 
    /// </summary>
    public static PivotRotation[] pivot_array; //= GameObject.FindObjectsOfType(PivotRotation);


    /// <summary>
    /// the number of shuffles in the user interface specified by the user.
    /// </summary>
    public InputField inShuffleSize;

    /// <summary>
    ///  an error message for the interface
    /// </summary>
    public Text txtErrorMessage;

    /// <summary>
    /// Traverses through the list of Pivot objects in the scene and checks if any of them are active by checking their magnitude & autorotating boolean. This however is redundant and could be removed.
    /// -Elijah Gray
    /// </summary>
    /// <returns> Returns a boolean, true if any pivots in the scene are active, false if no pivots are active. </returns>
    public static bool CheckForMovement()
    {

        if (pivot_array == null)
        {
            return false;
        }

        for (int i = 0; i < pivot_array.Length; i++)
        {
            if (pivot_array[i].autoRotating || pivot_array[i].rotation.magnitude > 0)
            {
                return true;
            }
        }

        return false;
    }


    /// <summary>
    ///  called at the beginning of the program startup to initialize the variables by finding the other objects in the unity scene. Additionaly, sets the framerate cap of the program 
    ///  as when the framerate is too high the program can break, this is an issue for futurework to solve. -Elijah Gray
    /// </summary>
    private void Start()
    {
        cubeState = FindObjectOfType<CubeState>();
        readCube = FindObjectOfType<ReadCube>();
        pivot_array = GameObject.FindObjectsOfType<PivotRotation>();

        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 120;
    }

 
    /// <summary>
    ///  Updates every frame in the program. Slightly modified function of the original creator(s). When the program isn't moving a cube already it will tell it to execute one move
    ///  in the automate.movelist variable then remoeve said move from the list in order. If the cube is mid-animation, it will skip this. - Elijah Gray
    /// </summary>
    private void Update()
    {
        if (CheckForMovement()) return; // prevents the program from scheduling another move while the cube already has a movement happening.
        // - Elijah Gray

        if (moveList.Count > 0 && !CubeState.autoRotating && CubeState.started)
        {
            CubeState.autoRotating = true;
            Debug.Log("automate() going to next step");
            DoMove(moveList[0]);

            moveList.Remove(moveList[0]);

        }
    }


    /// <summary>
    /// Slightly modified function. Executes a random number of moves specified by the user interface to shuffle the cube. Sends a list to the automate.movelist to shuffle the cube.
    /// </summary>
    public void Shuffle()
    {

        if (AddedCounter.instance) // added after to prevent it from resetting if it isn't initialized - Elijah
        {
            AddedCounter.instance.ResetSteps();              // Added 2/28/23
        }


        int shuffleLen = 0;
        int.TryParse(inShuffleSize.text, out shuffleLen);
        if (shuffleLen < 1)
        {
            txtErrorMessage.text = "Error: Input Valid Data";
            //shuffleLength = UnityEngine.Random.Range(10, 30);
        }
        else if (!inShuffleSize.text.Equals("") && shuffleLen > 10)
        {
            List<string> moves = new List<string>();
            //int shuffleLength = UnityEngine.Random.Range(0, shuffleLen);
            for (int i = 0; i < shuffleLen; i++)
            {
                //Random.seed = 5;
                int randomMove = UnityEngine.Random.Range(0, allMoves.Count);
                //int randomMove = 6;
                moves.Add(allMoves[randomMove]);
            }
            moveList = moves;
            txtErrorMessage.text = "";
        }

    }



    /// <summary>
    /// Created by the original developer and modified by Elijah and Adrian. Given a move in Rubik's cube notation, the program will execute that animation. - Elijah Gray
    /// </summary>
    /// <param name="move">the move to be executed by the program in Rubik's cube notation IE U, U' or U2</param>
    public void DoMove(string move)
    {

        readCube.ReadState();

        // turned the sequence of if statements into a switch statement to improve performance.
        // each "move" represents a step in rubiks cube notation for how the rubiks cube should be physically manipulated.
        // - Elijah Gray
        switch (move)
        {
            case "U":
                RotateSide(cubeState.up, -90);               
                break;
            case "U'":
                RotateSide(cubeState.up, 90);              
                break;
            case "U2":
                RotateSide(cubeState.up, -180);               
                break;
            case "D":
                RotateSide(cubeState.down, -90);               
                break;
            case "D'":
                RotateSide(cubeState.down, 90);              
                break;
            case "D2":
                RotateSide(cubeState.down, -180);               
                break;
            case "L":
                RotateSide(cubeState.left, -90);               
                break;
            case "L'":
                RotateSide(cubeState.left, 90);                
                break;
            case "L2":
                RotateSide(cubeState.left, -180);               
                break;
            case "R":
                RotateSide(cubeState.right, -90);               
                break;
            case "R'":
                RotateSide(cubeState.right, 90);               
                break;
            case "R2":
                RotateSide(cubeState.right, -180);               
                break;
            case "F":
                RotateSide(cubeState.front, -90);                
                break;
            case "F'":
                RotateSide(cubeState.front, 90);                
                break;
            case "F2":
                RotateSide(cubeState.front, -180);
                break;
            case "B":
                RotateSide(cubeState.back, -90);               
                break;
            case "B'":
                RotateSide(cubeState.back, 90);
                break;
            case "B2":
                RotateSide(cubeState.back, -180);
                break;
            default:
                Debug.Log("invalid move encountered!");
                return;
        }

        AddedCounter.instance.AddStep();                // Added 2/28/23 by Adrian, moved down here so it doesn't meed to be in every case, -Elijah Gray


    }


    /// <summary>
    ///  Unmodified function from the original developer. Used to execute a movements. - Elijah Gray
    /// </summary>
    /// <param name="side"> the side of the rubiks cube that should be rotated </param>
    /// <param name="angle"> the angle the cube side should be rotated, IE -90, 90, 180 </param>
    public void RotateSide(List<GameObject> side, float angle)
    {
        PivotRotation pr = side[4].transform.parent.GetComponent<PivotRotation>();
        pr.StartAutoRotate(side, angle);
    }



}
