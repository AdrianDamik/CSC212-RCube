// COPYRIGHT 2023 ELIJAH GRAY, FEEL FREE TO REUSE! :)
// Revised by Adrian Damik (2023)

using CubeXdotNET;
using Kociemba;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using seedHash = System.Collections.Generic.Queue<System.Tuple<string, int>>; // adrian 4/4/23


/// <summary>
/// blackbox test base & data collection system created by Elijah Gray. - Elijah Gray
/// revised by Adrian for use with seed generator - Adrian
/// this test is for the CFOP method.
/// </summary>
public class testCFOP : MonoBehaviour
{
    /// <summary>
    /// used to grab the cube state functionality.
    /// </summary>
    private CubeState cubeState;

    private ReadCube readCube;

    /// <summary>
    /// the solver used in this test, in this case the CFOP method.
    /// </summary>
    private CFOPSolver solver;

    /// <summary>
    /// 
    /// </summary>
    seedHash seedQueueCFOP = new seedHash(); // adrian 4/4/23 

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

    bool test; 


    // Start is called before the first frame update
    void Start()
    {
        AddedSeedGenerator SG = new AddedSeedGenerator();
        seedQueueCFOP = SG.ReadSeeds(); // Added by Adrian 4/4/23

        current_step = 0;
        wait_frames = 0;

        cubeState = FindObjectOfType<CubeState>();
        readCube = FindObjectOfType<ReadCube>();
        solver = FindObjectOfType<CFOPSolver>();
    }

    // initates a test to solve a number of cubes as per the variable "solves_left" -Elijah Gray
    // Update is called once per frame
    void Update()
    {

        if(CubeState.autoRotating)
        {
            Debug.Log("cube is currently automating, skipping");
            return;
        }

        if ((UnityEngine.Input.GetKeyUp(KeyCode.F2)))
        {
            test = true;
            solves_left = 1000; // how many cubes should be solved for the test.
            Time.timeScale = 20;
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

        // List<string> scramble_moves = new List<string>();
        // for (int i = 0; i < 100; i++)
        // {
        //     int randomMove = UnityEngine.Random.Range(0, allMoves.Count);
        //     scramble_moves.Add(allMoves[randomMove]);
        // }
        // Automate.moveList = scramble_moves;

        AddedSeedGenerator SG = new AddedSeedGenerator();

        Debug.Log("Test");
        if (seedQueueCFOP.TryPeek(out System.Tuple<string, int> pair)) // Added by Adrian 4/4/23
        {
            pair = seedQueueCFOP.Dequeue();
            Debug.Log("Pair: " + pair.Item1);
            AddedCounter.instance.ResetSteps();
            Automate.moveList = SG.hash(pair.Item1, pair.Item2); 
        }

    }



    // checks the rubiks cube object and checks if it is in a solved state. If it's not, throw an error.
    // - ELIJAH Gray
    void Check_Solution()
    {
        Assert.IsTrue(cubeState.GetStateString() == "UUUUUUUUURRRRRRRRRFFFFFFFFFDDDDDDDDDLLLLLLLLLBBBBBBBBB");
        if (cubeState.GetStateString() != "UUUUUUUUURRRRRRRRRFFFFFFFFFDDDDDDDDDLLLLLLLLLBBBBBBBBB")
        {
            Debug.Log("ERROR, RUBIKS CUBE NOT SOLVED" + cubeState.GetStateString());
        }


    }


    // copypasted from the original twophase solver. Converts a string input into a list of string objects representing the various moves.
    // - Elijah GRay
    List<string> StringToList(string solution)
    {
        List<string> solutionList = new List<string>(solution.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries));
        return solutionList;
    }



    // handles the process of going step to step for the tests. The first step is to scramble the cube. The second step is to generate a list of moves to solve the cube. The third step is to check if the cube is properly solved
    // - Elijah Gray
    void test_step ()
    {

        try
        {

            cubeState = FindObjectOfType<CubeState>();
            readCube = FindObjectOfType<ReadCube>();

            Debug.Log("Cubes left:" + solves_left + "and current step is: " + current_step);
            switch (current_step)
            {
                case 0:
                    Debug.Log("1st phase:");
                    wait_frames = 10; // * Convert.ToInt32(Time.timeScale);
                    //bad_Wait_function();
                    Scramble();
                    ++current_step;
                    break;

                case 1:
                    Debug.Log("2nd phase:");
                    wait_frames = 10; // * Convert.ToInt32(Time.timeScale);
                    //bad_Wait_function();
                    solver.Solver2();
                    ++current_step;
                    break;

                case 2:
                    wait_frames = 10; // * Convert.ToInt32(Time.timeScale);
                    Debug.Log("3rd phase @ cube_solve: " + solves_left);
                    --solves_left;
                    //bad_Wait_function();
                    Check_Solution();
                    current_step = 0;
                    break;

            }
        }
        catch (Exception e)
        {
            throw(e);
        }

    }








}

