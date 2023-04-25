// COPYRIGHT 2023 ELIJAH GRAY, FEEL FREE TO REUSE! :)
// Revised by Adrian Damik (2023)

using CubeXdotNET;
using Kociemba;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

using seedHash = System.Collections.Generic.Queue<System.Tuple<string, int>>; // adrian 4/4/23

/// <summary>
/// blackbox test base & data collection system created by Elijah Gray.
/// revised by Adrian Damik for use with seed generator - Adrian
/// this test is for the Kociemba method.
/// </summary>
public class TestKociemba : MonoBehaviour
{
    /// <summary>
    /// used to grab the cube state functionality.
    /// </summary>
    private CubeState cubeState;


    private ReadCube readCube;

    /// <summary>
    /// the solver used in this test, in this case the Kociemba method.
    /// </summary>
    private SolveTwoPhase solver;


    string static_test = "17 13 1 11 5 0 9 11 11 9 17 14 0 11 4 15 17 14 8 10 5 15 16 6 6 16 3 5 2 12 15 9 8 3 6 4 3 9 4 7 1 2 10 15 0 2 8 6 13 10 5 6 8 8 17 2 8 10 5 2 9 5 17 7 16 15 2 8 3 9 5 16 17 2 4 8 14 5 12 10 16 9 0 11 16 0 0 0 13 11 17 14 9 15 4 8 9 14 4 4";
    List<string> static_list;

    /// <summary>
    /// declare the queue used for automated testing.
    /// </summary>
    seedHash seedQueueKociemba = new seedHash(); // adrian 4/4/23

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

    bool test;

    /// <summary>
    /// called at the beginning of the program to initialize the test.
    /// </summary>
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

    /// <summary>
    /// initates a test if the F1 button is pressed to solve a number of cubes as per the variable "solves_left" -Elijah Gray
    /// the variable number_of_shuffles determiens how many shuffles should be done. The length_of_shuffles variable determines how many moves should be in each shuffle.
    /// the variable seed determines the seed in the shuffle command's number generator.
    /// Update is called once per frame
    /// </summary>
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

    /// <summary>
    /// instead of using original scramble, this will use the seed generator and seeds txt file to generate shuffles.
    /// </summary>
    void Scramble()
    {
        AddedSeedGenerator SG = new AddedSeedGenerator();

        if (seedQueueKociemba.TryPeek(out System.Tuple<string, int> pair))
        {
            pair = seedQueueKociemba.Dequeue();
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
    /// copypasted from the original twophase solver. Converts a string input into a list of string objects representing the various moves.
    /// </summary>
    /// <param name="solution"> string input. </param>
    /// <returns> returns list of string objects for moves. </returns>
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

    /// <summary>
    /// handles the process of going step to step for the tests. The first step is to scramble the cube. The second step is to generate a list of moves to solve the cube. The third step is to check if the cube is properly solved. - Elijah Gray
    /// </summary>
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


