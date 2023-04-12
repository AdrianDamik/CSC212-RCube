// Copyright 2023
// Adrian Damik, Elijah Gray, & Aryan Pothanaboyina

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CubeXdotNET;
using Kociemba;
using UnityEngine.Windows;
using System;
using System.Linq;
using UnityEngine.UI;
// I am including this because the original creator had included the code of the visualizer in the Kociemba solver.

public class CFOPSolver : MonoBehaviour
{
    // Start is called before the first frame update

    private ReadCube readCube;
    private CubeState cubeState;

    void Start()
    {
        /*
       ReadCube readCube;
       CubeState cubeState;
       
       // obtains initial data from the cube upon startup.
       readCube = FindObjectOfType<ReadCube>();
       cubeState = FindObjectOfType<CubeState>();

       string moveString = new string(cubeState.GetStateString());

        //translatess the initial data into a format the CFOP solver can handle.

        FridrichSolver Solver2 = new FridrichSolver("gggggggggooooooooobbbbbbbbbrrrrrrrrryyyyyyyyywwwwwwwww");
        // this is the solved state of the rubix cube per the original solver's documentation.
        // We want it initalized as an untouched cube.
        // I am referring it to as solver2 because it is the second solver implemented.
        */

        readCube = FindObjectOfType<ReadCube>(); // obtains the data from the rubix cube -Elijah Gray 2/21/2023
        cubeState = FindObjectOfType<CubeState>();

    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("function 2 updated");

        // if (CubeState.started)
        //{
        //   Solver2();
        // }




        //if ((UnityEngine.Input.GetKeyUp(KeyCode.X)))
        //{
            //do stuff for testing, a more standard testing system is needed. I used this to manually rotate the cube within the CFOP's solver to see how the string representation 
            //changed so I could translate it properly.  -Elijah 2/
            

            // The visualizer will solve when X is pressed by the user. This is temporary. 
            /// this is for me testing things, I am just trying to figure out how to map out the solutions.
            //16, 1 = F
            //18, 1 = L
            //14, 1 = R
            /*

            FridrichSolver cube_to_solve2 = new FridrichSolver("gggggggggooooooooobbbbbbbbbrrrrrrrrryyyyyyyyywwwwwwwww");
            Debug.Log("initial state of solved cube: " + getString(cube_to_solve2.Cube));

            //CubeXdotNET.Tools.RotateCube(cube_to_solve2, 16, 1); // F
            //CubeXdotNET.Tools.RotateCube(cube_to_solve2, 18, 1); // L
            CubeXdotNET.Tools.RotateCube(cube_to_solve2, 14, 1); // R
            CubeXdotNET.Tools.RotateCube(cube_to_solve2, 16, 1); // F
           // CubeXdotNET.Tools.RotateCube(cube_to_solve2, 18, 1); // L
            //CubeXdotNET.Tools.RotateCube(cube_to_solve2, 16, 1); // F


            Debug.Log("moves used: " + cube_to_solve2.Solution);
            Debug.Log("state of turned cube: " + getString(cube_to_solve2.Cube));

            cube_to_solve2.Solve();

            Debug.Log("solution of solved starter cube: " + cube_to_solve2.Solution);
            //Debug.Log("state of turned cube: " + getString(cube_to_solve2.Cube));


            FridrichSolver cube_to_solve = new FridrichSolver("gggggggggooooooooobbbbbbbbbrrrrrrrrryyyyyyyyywwwwwwwww");
            */
            // solver 2 is called when X is pressed.

            //Solver2();
        //}

    }

    // produces a solution based on how the Kociemba method setup by the original creator 
    // setup their solver.
    // the solver first translates the state of the rubiks cube into an order
    // the cfop solver can use by rearranging the facelets in the cube representation
    // string. It then translates each individual facelet if needed because both programs
    // use different character representations for colors.
    // It then produces a list of moves to follow in rubiks cube notation which are 
    // partially translated after due to both programs using different character
    // representations. It then sends these moves in a list of strings to the automate.movelist
    // so that it may be executed.
    // - Elijah Gray
    public void Solver2()
    {

        AddedCounter.instance.ResetSteps();              // Added 2/16/23 - Adrian

        string solution = "";

        //translate the Cube
        string translation = string_translation(cfop_initialize());
        Debug.Log("CFOP translated cube: " + translation);

        FridrichSolver cube_to_solve = new FridrichSolver(translation);

        cube_to_solve.Solve();
        //Debug.Log("solved?: " + cube_to_solve.IsSolved);
        //Debug.Log("Error code: " + cube_to_solve.ErrorCode);
        //Debug.Log("output solution: " + cube_to_solve.Solution);


        solution = cube_to_solve.Solution; // obtains the solution from the CFOP solver

        //Convert the Solved Moves from a string to a list
        List<string> solutionList = StringToList(solution); // converts the CFOP solver's string into a list of moves.

        // converts the solution list into a set of moves that make sense for the visualizer as each use different colors for the faces.
        // for example, the CFOP solver views the green face as the front and the visualizer sees the orange face as the front.
        // -Elijah Gray 2/21/2023
        for(int i = 0; i < solutionList.Count; i++)
        {
            switch (solutionList[i].First())
            {
                case 'L':
                    solutionList[i] = solutionList[i].Replace(solutionList[i][0], 'B');
                    //Debug.Log("L -> B");
                    break;

                case 'F':
                    solutionList[i] = solutionList[i].Replace(solutionList[i][0], 'L');
                   //Debug.Log("F -> L");
                    break;

                case 'R':
                    solutionList[i] = solutionList[i].Replace(solutionList[i][0], 'F');
                    //Debug.Log("R -> F");
                    break;

                case 'B':
                    solutionList[i] = solutionList[i].Replace(solutionList[i][0], 'R');
                    //Debug.Log("B -> R");
                    break;

                default:
                    break;

            }
        }


        // debug for checking list elements

        /*
        string output_test = "";

        for (int i = 0; i < solutionList.Count; i++)
        {
            output_test += " " + solutionList[i];
        }
        Debug.Log("Move list: " + output_test);
        */


        //Debug.Log("after state of solved cube: " + getString(cube_to_solve.Cube));


        //Automate the List
        Automate.moveList = solutionList;

    }


        // converts the string into a list of steps to be used by the visualizer rotating automaton
        // created by the original owner, I just copypasted it here.
        // - Elijah Gray
        //returns the list of strings created using the string.
        List<string> StringToList(string solution)
        {
            List<string> solutionList = new List<string>(solution.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries));
            return solutionList;
        }


    // translates the visualizer facelets into a format the CFOP solver uses
    // -Elijah Gray 2/21/2023
    string string_translation(string to_translate)
        {
            string translated_string = "";

            for (int i = 0; i < to_translate.Length; i++)
            {

                switch (to_translate.ElementAt(i))
                {
                    case 'U':
                        translated_string += "y";
                        break;

                    case 'D':
                        translated_string += "w";
                        break;

                    case 'L':
                        translated_string += "g";
                        break;

                    case 'R':
                        translated_string += "b";
                        break;

                    case 'F':
                        translated_string += "o";
                        break;

                    case 'B':
                        translated_string += "r";
                        break;

                    default:
                        break;

                }
            }

            return translated_string;
        }


    // returns a string representation of the rubiks cube in the visualizer
    //  in the way the CFOP solver
    // expects so that it may solve it.
    // - Elijah Gray
    string cfop_initialize()
    {
        string moveString = "";

        //Get the State of the cube as a string
        moveString += cubeState.get_left(); // the visualizer has a function to get a face of the cube as a string. Most of the cube's faces's facelets are ordered the same so little change is needed for these ones
        moveString += cubeState.get_front(); // other than reordering them
        moveString += cubeState.get_right();
        moveString += cubeState.get_back();

        // the up and down face's facelets are ordered differently unlike the other faces, we are adding each individual facelet's data bit by bit.
        string up = cubeState.get_up();
        string handle_up = "";

        handle_up += up.ElementAt(8);
        handle_up += up.ElementAt(7);
        handle_up += up.ElementAt(6);
        handle_up += up.ElementAt(5);

        handle_up += up.ElementAt(4);

        handle_up += up.ElementAt(3);
        handle_up += up.ElementAt(2);
        handle_up += up.ElementAt(1);
        handle_up += up.ElementAt(0);

        //Debug.Log("resulting up string 1: " + handle_up);
        moveString += handle_up; // add the new up face's string to the string representation of the rubix cube.

        // the up and down face's facelets are ordered differently unlike the other faces, we are adding each individual facelet's data bit by bit.
        string down = cubeState.get_down();
        string handle_down = "";

        handle_down += down[8];
        handle_down += down[7];
        handle_down += down[6];
        handle_down += down[5];

        handle_down += down[4];

        handle_down += down[3];
        handle_down += down[2];
        handle_down += down[1];
        handle_down += down[0];

        //+ up[5] + up[8] + up[1] + up[4] + up[7] + up[0] + up[3] + up[6];
        //Debug.Log("resulting up string 2: " + handle_down);

        moveString += handle_down; // add the new down face's string to the string representation of the rubix cube.

        //this could be made more effecient with stringbuilder, I just didn't have time.
        // - Elijah
        return moveString;
    }




}




