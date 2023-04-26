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
/// blackbox test base & data collection system created by Elijah Gray.
/// revised by Adrian Damik for use with seed generator - Adrian
/// this test is for the Beginner method.
/// </summary>
public class TestLayer : MonoBehaviour
{
    /// <summary>
    /// used to grab the cube state functionality.
    /// </summary>
    private CubeState cubeState;

    /// <summary>
    /// the solver used in this test, in this case the beginner/layer method.
    /// </summary>
    private LayerSolver solver;

    /// <summary>
    /// declare the queue used for automated testing.
    /// </summary>
    seedHash seedQueueLayer = new seedHash(); // adrian 4/4/23

    /// <summary>
    /// list of moves in rubik's cube notation, copypasted from the Kociemba solver file.
    /// </summary>
    private readonly List<string> allMoves = new List<string>()
    {
        "U", "D", "L", "R", "F", "B",
        "U2", "D2", "L2", "R2", "F2", "B2",
        "U'", "D'", "L'", "R'", "F'", "B'"
    };

    /// <summary>
    /// the number of cubes to solve left in a test. Initially set to the number of cubes to solve via the number of shuffles variable.
    /// </summary>
    int solves_left;

    /// <summary>
    /// current instruction step  0 - > scramble,  1 -> solve,  2 -> check if solved properly.
    /// </summary>
    int current_step;

    /// <summary>
    /// how many frames should the program wait before going to the next step.
    /// </summary>
    int wait_frames; 

    /// <summary>
    /// boolean to determine whether a test should occur. Set to true when an test is initiated by a button press.
    /// </summary>
    bool test; 

    /// <summary>
    /// called at the beginning of the program to initialize the test.
    /// </summary>
    void Start()
    {

        AddedSeedGenerator SG = new AddedSeedGenerator();
        seedQueueLayer = SG.ReadSeeds(); // Added by Adrian 4/4/23

        current_step = 0;
        wait_frames = 0;

        cubeState = FindObjectOfType<CubeState>();
        solver = FindObjectOfType<LayerSolver>();
    }

    /// <summary>
    /// initates a test if the F3 button is pressed to solve a number of cubes as per the variable "solves_left" -Elijah Gray
    /// the variable number_of_shuffles determiens how many shuffles should be done. The length_of_shuffles variable determines how many moves should be in each shuffle.
    /// the variable seed determines the seed in the shuffle command's number generator.
    /// Update is called once per frame
    /// </summary>
    void Update()
    {

        if (CubeState.autoRotating)
        {
            Debug.Log("cube is currently automating, skipping");
            return;
        }

        if ((UnityEngine.Input.GetKeyUp(KeyCode.F3)))
        {
            test = true;
            solves_left = 1000; // how many cubes should be solved for the test.
            Debug.Log("layer test initiated");
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

    /// <summary>
    /// instead of using original scramble, this will use the seed generator and seeds txt file to generate shuffles.
    /// </summary>
    void Scramble()
    {
        AddedSeedGenerator SG = new AddedSeedGenerator();

        if (seedQueueLayer.TryPeek(out System.Tuple<string, int> pair)) // Added by Adrian 4/4/23
        {
            pair = seedQueueLayer.Dequeue();
            Debug.Log("Pair: " + pair.Item1);
            AddedCounter.instance.ResetSteps();
            Automate.moveList = SG.hash(pair.Item1, pair.Item2); 
        }

    }

    /// <summary>
    /// checks the rubiks cube object to check if it is in a solved state. If the cube is not solved, throw a unity console message stating the cube was not solved. -Elijah Gray
    /// </summary>
    void Check_Solution()
    {

        Assert.IsTrue(cubeState.GetStateString() == "UUUUUUUUURRRRRRRRRFFFFFFFFFDDDDDDDDDLLLLLLLLLBBBBBBBBB");
        if (cubeState.GetStateString() != "UUUUUUUUURRRRRRRRRFFFFFFFFFDDDDDDDDDLLLLLLLLLBBBBBBBBB")
        {
            Debug.Log("ERROR, RUBIKS CUBE NOT SOLVED" + cubeState.GetStateString());
        }


    }


    /// <summary>
    /// copypasted from the original twophase solver. Converts a string input into a list of string objects representing the various moves.
    /// </summary>
    /// <param name="solution"> string input. </param>
    /// <returns> returns list of string objects for moves. </returns>
    List<string> StringToList(string solution)
    {
        List<string> solutionList = new List<string>(solution.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries));
        return solutionList;
    }



    /// <summary>
    /// handles the process of going step to step for the tests. The first step is to scramble the cube. The second step is to generate a list of moves to solve the cube. The third step is to check if the cube is properly solved. - Elijah Gray
    /// </summary>
    void test_step ()
    {

        try
        {

            cubeState = FindObjectOfType<CubeState>();

            Debug.Log("Cubes left:" + solves_left + "and current step is: " + current_step);
            switch (current_step)
            {
                case 0:
                    Debug.Log("1st phase:");
                    wait_frames = 10;
                    Scramble();
                    ++current_step;
                    break;

                case 1:
                    Debug.Log("2nd phase:");
                    wait_frames = 10; 
                    solver.Solver3();
                    ++current_step;
                    break;

                case 2:
                    wait_frames = 10; 
                    Debug.Log("3rd phase @ cube_solve: " + solves_left);
                    --solves_left;
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


