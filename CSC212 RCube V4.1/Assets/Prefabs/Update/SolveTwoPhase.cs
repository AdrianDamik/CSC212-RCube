// Copyright 2023
// Adrian Damik, Elijah Gray, & Aryan Pothanaboyina

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kociemba;


public class SolveTwoPhase : MonoBehaviour
{
    private ReadCube readCube;
    private CubeState cubeState;
    private bool doOnce = true;

    // Start is called before the first frame update
    void Start()
    {
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();

    }

    // Update is called once per frame
    void Update()
    {
        if(CubeState.started && doOnce)
        {
            doOnce = false;
            Solver();
        }
    }

    //initiates the Kociemba's method to solve the rubiks cube. This part of the program was unmodified by me and served as a basis of how to set up
    // the other rubiks cube solvers. Once it reads the cube it will get the string, plug that string representation into Kociemba's algorithim.
    // The algorithim will then generate a list of moves to solve that rubiks cube cube and these moves will be outputted and sent to automate.movelist
    // for the rubiks cube to execute.
    // - Elijah Gray
    public void Solver()
    {

        AddedCounter.instance.ResetSteps();              // Added 2/16/23 - Adrian

        //Time.timeScale = 10;
        Debug.Log("function called");
        //Time.timeScale = 10;
        readCube.ReadState();

        //Get the State of the cube as a string
        string moveString = cubeState.GetStateString();

        Debug.Log(moveString);

        //Solve the Cube

        string info = "";

        //First Time Build the Tables
        //string solution = SearchRunTime.solution(moveString, out info, buildTables: true);

        //Every other time
        string solution = Search.solution(moveString, out info);
        Debug.Log("Solution is: " + solution);

        //Convert the Solved Moves from a string to a list
        List<string> solutionList = StringToList(solution);

        //Automate the List
        Automate.moveList = solutionList;

    }

    List<string> StringToList(string solution)
    {
        List<string> solutionList = new List<string>(solution.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries));
        return solutionList;
    }
}
