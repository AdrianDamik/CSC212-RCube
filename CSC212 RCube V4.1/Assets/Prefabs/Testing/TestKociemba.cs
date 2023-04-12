// Copyright 2023
// Adrian Damik, Elijah Gray, & Aryan Pothanaboyina

using CubeXdotNET;
using Kociemba;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

using seedHash = System.Collections.Generic.Queue<System.Tuple<string, int>>; // adrian 4/4/23

// test base created by Elijah Gray
public class TestKociemba : MonoBehaviour
{
    private CubeState cubeState;
    private ReadCube readCube;
    private SolveTwoPhase solver;
    string static_test = "17 13 1 11 5 0 9 11 11 9 17 14 0 11 4 15 17 14 8 10 5 15 16 6 6 16 3 5 2 12 15 9 8 3 6 4 3 9 4 7 1 2 10 15 0 2 8 6 13 10 5 6 8 8 17 2 8 10 5 2 9 5 17 7 16 15 2 8 3 9 5 16 17 2 4 8 14 5 12 10 16 9 0 11 16 0 0 0 13 11 17 14 9 15 4 8 9 14 4 4";
    List<string> static_list;

    seedHash seedQueueKociemba = new seedHash(); // adrian 4/4/23

    //SolveTwoPhase Method_Solver;

    //GameObject.find("PivotRotation");

    // list of moves, copypasted from the Kociemba solver file. - Elijah Gray
    private readonly List<string> allMoves = new List<string>()
    {
        "U", "D", "L", "R", "F", "B",
        "U2", "D2", "L2", "R2", "F2", "B2",
        "U'", "D'", "L'", "R'", "F'", "B'"
    };


    int solves_left; // number of cubes to solve
    int current_step; // current instruction step  0 - > scramble,  1 -> solve,  2 -> check if solved properly.
    int wait_frames; // how many frames should the program wait before going to the next step. Currently a solution but an ineffecient one because sometimes the rubiks cube will try to go to the next step while its still movingm causing the program
    // to crash 
    // - Elijah Gray

    bool test; //boolean to determine whether a test should happen - Elijah Gray, if true the program will begin a test process.

    // Start is called before the first frame update
    void Start()
    {
        AddedSeedGenerator SG = new AddedSeedGenerator();
        seedQueueKociemba = SG.ReadSeeds(); // Added by Adrian 4/4/23
        current_step = 0;
        wait_frames = 0;

        cubeState = FindObjectOfType<CubeState>();
        readCube = FindObjectOfType<ReadCube>();
        solver = FindObjectOfType<SolveTwoPhase>();
        static_list = StringToLis2(static_test);
    }

    // initates a test to solve a number of cubes as per the variable "solves_left" -Elijah Gray
    // Update is called once per frame
    void Update()
    {
        if(CubeState.autoRotating)
        {
            Debug.Log("cube currently autorotating, skipping");
            return;
        }

        if(Automate.CheckForMovement())
        {
            Debug.Log("cube currently busy, skipping");
            return;
        }

        if ((UnityEngine.Input.GetKeyUp(KeyCode.F1)))
        {
            test = true;
            solves_left = 100; // how many cubes should be solved for the test.
            //Time.timeScale = 20;
        }


        if (test)
        {
            if (wait_frames > 0) // part of a stop-gap solution to prevent the rubiks cube from breaking. Tries to act as a buffer between steps by giving the rubiks cube time to finish its animation so we don't cause an error when trying to read the cube.
            {
                --wait_frames;
                return;
            }

            if (CubeState.autoRotating || Automate.moveList.Count > 0) // if the cube is currently going through moves to solve the cube we shouldn't try to go to the next step.
            {
                return;
            }

            if (solves_left != 0) // if there are more cubes to solve, keep going.
            {
                test_step();
            }
            else
            {
                Debug.Log("No solves left!");
                test = false;
            }
        }

    }

    // scrambles a rubiks cube by generating a list of steps and sending it to the automate object  - Elijah Gray
    // todo: make the scrambles smarter so they cant undo their last move.
    void Scramble()
    {
        // string test = "";
        // List<string> scramble_moves = new List<string>();
        // for (int i = 0; i < 100; i++)
        // {
        //     int randomMove = UnityEngine.Random.Range(0, allMoves.Count);
        //     scramble_moves.Add(allMoves[randomMove]);
        //     test += allMoves[randomMove].ToString();
        //     test += " ";
        // }
        // Debug.Log(test); // outpuuts scramble used

        //Automate.moveList = static_list;

        AddedSeedGenerator SG = new AddedSeedGenerator();

        if (seedQueueKociemba.TryPeek(out System.Tuple<string, int> pair))
        {
            pair = seedQueueKociemba.Dequeue();
            Debug.Log("Pair: " + pair.Item1);
            AddedCounter.instance.ResetSteps();
            Automate.moveList = SG.hash(pair.Item1, pair.Item2); 
        }
    }

    // a debug function to check a specific scramble to determine if it can or cannot be properly solved.
    // was used to determine an issue with our program wasn't an error in the solving itself but how the animations
    // for the rubix cubes were being executed.
    // - Elijah Gray
    void Do_Specific_Scramble()
    {
        //string test = "D' B' F U F' R R' B U' B2 R2 R' D B2 L' B D' L' R R2 F' R' R' R R2 F D L L2 D2 U B R' F2 B2 U' L2 B' D' D2 U' D F F U L2 B' U U2 R U F B' F2 R B D2 L' U2 F R' L2 R2 B2 D R B2 D2 F2 F B2 L R' R2 D2 F' U' U' D F2 R' F F' B' U' L B' D' L2 R F' R R' L' D2 U' D R2 D' B'";
        //string test = "L' F' F B D F' F B U2 D2 R R' D2 F2 L2 U2 L' R' D2 R L U' U' B2 R' D2 B F' D' F2 L2 U2 U' U' D2 R D2 F U D D' U2 F' L' D' R D' U2 R' U L F L' F2 D U R2 F' D D' D2 U D F U2 F D B F B' R L' R2 B' F2 F2 D U D' D2 R' B D2 L R' D' L' F2 B2 F B2 F B2 D' B' L2 L2 U' F' D' ";

        // cube_state = LLRBUFBLFLRURRFURRDUUDFBBURLFBDDDDDFBURULLFFDFBULBRDBL
        // shuffle = "D2 U2 F' R2 U' U' R2 F' U2 B' D F' D B2 D' D2 U' B2 R U' R L2 U R2 R D F2 U U D' F2 D2 D B2 L B U B2 D2 D' U' L2 D2 U2 U L2 R' F' L2 U' F2 U D' U' R2 B R D2 B' U' F' B' F2 D' R2 L' B R2 B2 U D B L2 F2 R2 F L D' L' F2 R2 L' U2 R2 L' D' U' R F' B B2 R R2 U2 D' U F U2 U' L "
        string test = "D2 U2 F' R2 U' U' R2 F' U2 B' D F' D B2 D' D2 U' B2 R U' R L2 U R2 R D F2 U U D' F2 D2 D B2 L B U B2 D2 D' U' L2 D2 U2 U L2 R' F' L2 U' F2 U D' U' R2 B R D2 B' U' F' B' F2 D' R2 L' B R2 B2 U D B L2 F2 R2 F L D' L' F2 R2 L' U2 R2 L' D' U' R F' B B2 R R2 U2 D' U F U2 U' L ";

        // Solution is: F2 D R' B' D2 L F' L F2 U F' D R2 U2 R2 D F2 B2 D' F2 
        //List<string> scramble = StringToList(test);
        Automate.moveList = StringToList(test);


    }

    // checks the rubiks cube object and checks if it is in a solved state. If it's not, throw an error.
    // - ELIJAH Gray
    void Check_Solution()
    {

        string result = cubeState.GetStateString();

        if (result != "UUUUUUUUURRRRRRRRRFFFFFFFFFDDDDDDDDDLLLLLLLLLBBBBBBBBB")
        {
            Debug.Log("ERROR, RUBIKS CUBE NOT SOLVED" + cubeState.GetStateString());
            // "DUFUUDFDURRRRRRRRRUBBFFFDFFDDUDDUUFLLLLLLLLRBBFBBBBD"
            throw (new Exception("bad result"));
        }
        Assert.IsTrue(result == "UUUUUUUUURRRRRRRRRFFFFFFFFFDDDDDDDDDLLLLLLLLLBBBBBBBBB");

    }

    /// <summary>
    /// 10533139131712141016126441297111512111615131216117015115011141261131481815177120166617141717161321711177186168871313101031241541011110114161390144391751
    /// // 17 13 1 11 5 0 9 11 11 9 17 14 0 11 4 15 17 14 8 10 5 15 16 6 6 16 3 5 2 12 15 9 8 3 6 4 3 9 4 7 1 2 10 15 0 2 8 6 13 10 5 6 8 8 17 2 8 10 5 2 9 5 17 7 16 15 2 8 3 9 5 16 17 2 4 8 14 5 12 10 16 9 0 11 16 0 0 0 13 11 17 14 9 15 4 8 9 14 4 4 
    /// </summary>
    /// <param name="solution"></param>
    /// <returns></returns>


    // copypasted from the original twophase solver. Converts a string input into a list of string objects representing the various moves.
    // - Elijah GRay
    List<string> StringToList(string solution)
    {
        List<string> solutionList = new List<string>(solution.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries));
        return solutionList;
    }

    List<string> StringToLis2(string solution)
    {
        List<string> solutionList = new List<string>();

        for (int i = 0; i < solution.Length; i++)
        {
            solutionList.Add(solution[i].ToString());
        }

        return solutionList;
    }


    // CASES TO EXAMINE:
    // 11 7 8 8 17 8 2 11 8 12 1 15 2 8 13 2 9 7 15 15 16 2 17 14 14 5 1 5 4 8 9 12 10 1 2 11 5 17 9 10 15 15 12 3 3 14 17 2 14 7 13 1 4 0 13 8 7 7 16 9 0 15 8 5 15 9 9 15 12 1 9 8 11 2 3 1 9 8 0 6 12 14 9 3 4 14 10 11 14 9 2 17 15 6 5 5 14 6 0 1 



    // handles the process of going step to step for the tests. The first step is to scramble the cube. The second step is to generate a list of moves to solve the cube. The third step is to check if the cube is properly solved
    // - Elijah Gray
    void test_step ()
    {

        try
        {

            Debug.Log("Cubes left:" + solves_left + "and current step is: " + current_step);
            switch (current_step)
            {
                case 0:
                    Debug.Log("1st phase:");
                    wait_frames = 10; //  240 * Convert.ToInt32(Time.timeScale);
                    Scramble();
                    //Do_Specific_Scramble();
                   ++current_step;
                    break;

                case 1:
                    Debug.Log("2nd phase:");
                    wait_frames = 10; // * Convert.ToInt32(Time.timeScale);
                    //Solve();
                    solver.Solver();
                    ++current_step;
                    break;

                case 2:
                    wait_frames = 10; // 240; // * Convert.ToInt32(Time.timeScale);
                    Debug.Log("3rd phase @ cube_solve: " + solves_left);
                    --solves_left;
                    Check_Solution();
                    current_step = 0;
                    break;

            }
        }
    
        catch (Exception e)
        {
            Time.timeScale = 0;
            throw (e);
        }
    

    }


}


