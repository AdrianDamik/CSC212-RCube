// Copyright 2023
// Adrian Damik, Elijah Gray, & Aryan Pothanaboyina

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;
using UnityEngine.UI;

// part of the original developer's code, the main part of automates update loop is unmodified.
// turned into main driver function of the program
// Updates every frame to manage the rubiks cube's functionality, determining if it should be rotating
// or not.
// - Elijah Gray

public class Automate : MonoBehaviour
{
    public static List<string> moveList = new List<string>();

    private readonly List<string> allMoves = new List<string>() 
    {
        "U", "D", "L", "R", "F", "B",
        "U2", "D2", "L2", "R2", "F2", "B2",
        "U'", "D'", "L'", "R'", "F'", "B'"
    }; // a list of all possible moves utilizable by this visualizer in rubiks cube notation.

    private CubeState cubeState;
    private ReadCube readCube;
    public static PivotRotation[] pivot_array; //= GameObject.FindObjectsOfType(PivotRotation);

    Automate()
    {
        Debug.Log("Automate object created");
    }

    public InputField inShuffleSize;
    public Text txtErrorMessage;

    public int targetFrameRate = 120; // desired framerate. 

    // Goes through a list of pivot game objects in the unity to see if any are currently autorotating
    // this is checked by seeing if their autorotate boolean value is true.
    // if any of the pivots are autorotating then the function will return true, otherwise it will return false.
    // - Elijah Gray
    public static bool CheckForMovement()
    {

        if(pivot_array == null)
        {
            return false;
        }


        for(int i = 0; i < pivot_array.Length; i++ )
        {
            if (pivot_array[i].autoRotating || pivot_array[i].rotation.magnitude > 0)
            {
                return true;
            }
        }

        return false;
    }

    // a debug function that was used to count how many pivot points were active at a given time in the rubiks cube simulation
    // -Elijah Gray
    // returns number of pivots whose auto-rotation boolean is true.
    /*
    public int Number_of_active_Pivots()
    {

        if (pivot_array == null)
        {
            return 0;
        }

        int number = 0;
        for (int i = 0; i < pivot_array.Length; i++)
        {
            if (pivot_array[i].autoRotating)
            {
                ++number;
            }
        }

        return number;
    }
    */


    // Start is called before the first frame update
    void Start()
    {
        cubeState = FindObjectOfType<CubeState>();
        readCube = FindObjectOfType<ReadCube>();
        pivot_array = GameObject.FindObjectsOfType<PivotRotation>();
        //manager = FindFirstObjectByType<CubeEventManager>();

        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    // written by original developer/tutorial creator
    // determines if cube should execute a move every frame
    // if the Cubestate.started condition is true, it can begin.
    // if the movelist is 0 no action will be taken.
    // if the rubiks cube is already rotating it will take no action.
    // - Elijah Gray
    void Update()
    {
        if (CheckForMovement()) return; // prevents the program from scheduling another move while the cube already has a movement happening.
        // - Elijah Gray

        if(moveList.Count > 0 && !CubeState.autoRotating && CubeState.started)
        {
            CubeState.autoRotating = true;
            Debug.Log("automate() going to next step");
            DoMove(moveList[0]);

            moveList.Remove(moveList[0]);

        }
    }

    // created by original developer/tutorial creator.
    // updates the list of strings movelist to have a series of moves that will be executed to shuffle the rubiks cube.
    // - Elijah Gray
    public void Shuffle()
    {

        AddedCounter.instance.ResetSteps();              // Added 2/28/23

        int shuffleLen = 0;
        int.TryParse(inShuffleSize.text, out shuffleLen);
        if(shuffleLen < 1)
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

    // takes in a string representing a rubiks cube move in rubiks cube notation. Performs a full rotation on a cube as requested in the visualizer.
    // - Elijah Gray
    public void DoMove(string move)
    {
        
        Debug.Log("MOVE EXECUTED: " + move);
  
        if(CheckForMovement())
        {
            Debug.Log("executing move while cube is already moving!");
        }


        //doing_something = true;
        readCube.ReadState();

        // turned the sequence of if statements into a switch statement to improve performance.
        // each "move" represents a step in rubiks cube notation for how the rubiks cube should be physically manipulated.
        // - Elijah Gray
        switch (move)
        {
            case "U":
                RotateSide(cubeState.up, -90);
                AddedCounter.instance.AddStep();                // Added 2/28/23
                break;
            case "U'":
                RotateSide(cubeState.up, 90);
                AddedCounter.instance.AddStep();                // Added 2/28/23
                break;
            case "U2":
                RotateSide(cubeState.up, -180);
                AddedCounter.instance.AddStep();                // Added 2/28/23
                break;
            case "D":
                RotateSide(cubeState.down, -90);
                AddedCounter.instance.AddStep();                // Added 2/28/23
                break;
            case "D'":
                RotateSide(cubeState.down, 90);
                AddedCounter.instance.AddStep();                // Added 2/28/23
                break;
            case "D2":
                RotateSide(cubeState.down, -180);
                AddedCounter.instance.AddStep();                // Added 2/28/23
                break;
            case "L":
                RotateSide(cubeState.left, -90);
                AddedCounter.instance.AddStep();                // Added 2/28/23
                break;
            case "L'":
                RotateSide(cubeState.left, 90);
                AddedCounter.instance.AddStep();                // Added 2/28/23
                break;
            case "L2":
                RotateSide(cubeState.left, -180);
                AddedCounter.instance.AddStep();                // Added 2/28/23
                break;
            case "R":
                RotateSide(cubeState.right, -90);
                AddedCounter.instance.AddStep();                // Added 2/28/23
                break;
            case "R'":
                RotateSide(cubeState.right, 90);
                AddedCounter.instance.AddStep();                // Added 2/28/23
                break;
            case "R2":
                RotateSide(cubeState.right, -180);
                AddedCounter.instance.AddStep();                // Added 2/28/23
                break;
            case "F":
                RotateSide(cubeState.front, -90);
                AddedCounter.instance.AddStep();                // Added 2/28/23
                break;
            case "F'":
                RotateSide(cubeState.front, 90);
                AddedCounter.instance.AddStep();                // Added 2/28/23
                break;
            case "F2":
                RotateSide(cubeState.front, -180);
                AddedCounter.instance.AddStep();                // Added 2/28/23
                break;
            case "B":
                RotateSide(cubeState.back, -90);
                AddedCounter.instance.AddStep();                // Added 2/28/23
                break;
            case "B'":
                RotateSide(cubeState.back, 90);
                AddedCounter.instance.AddStep();                // Added 2/28/23
                break;
            case "B2":
                RotateSide(cubeState.back, -180);
                AddedCounter.instance.AddStep();                // Added 2/28/23
                break;
            default:
                Debug.Log("invalid move encountered!");
                break;
        }



        /*
        if(move == "U" )
        {
            RotateSide(cubeState.up, -90);
        }
        if (move == "U'")
        {
            RotateSide(cubeState.up, 90);
        }
        if (move == "U2")
        {
            RotateSide(cubeState.up, -180);
        }
        if (move == "D")
        {
            RotateSide(cubeState.down, -90);
        }
        if (move == "D'")
        {
            RotateSide(cubeState.down, 90);
        }
        if (move == "D2")
        {
            RotateSide(cubeState.down, -180);
        }
        if (move == "L")
        {
            RotateSide(cubeState.left, -90);
        }
        if (move == "L'")
        {
            RotateSide(cubeState.left, 90);
        }
        if (move == "L2")
        {
            RotateSide(cubeState.left, -180);
        }
        if (move == "R")
        {
            RotateSide(cubeState.right, -90);
        }
        if (move == "R'")
        {
            RotateSide(cubeState.right, 90);
        }
        if (move == "R2")
        {
            RotateSide(cubeState.right, -180);
        }
        if (move == "F")
        {
            RotateSide(cubeState.front, -90);
        }
        if (move == "F'")
        {
            RotateSide(cubeState.front, 90);
        }
        if (move == "F2")
        {
            RotateSide(cubeState.front, -180);
        }
        if (move == "B")
        {
            RotateSide(cubeState.back, -90);
        }
        if (move == "B'")
        {
            RotateSide(cubeState.back, 90);
        }
        if (move == "B2")
        {
            RotateSide(cubeState.back, -180);
        }
        */

        //doing_something = false;

    }
    
    //instantiates a pivot point and begins the rubiks cube rotation process when given the side of a rubiks cube from the list of possible sides
    // and given an angle the turn should be.
    // - Elijah Gray
    void RotateSide(List<GameObject> side, float angle)
    {
        PivotRotation pr = side[4].transform.parent.GetComponent<PivotRotation>();
        pr.StartAutoRotate(side, angle);
    }

 

}
