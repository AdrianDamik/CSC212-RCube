// Created by Elijah Gray (2023)
// Revised by Adrian Damik (2023)
// Uses original work from divinsmathew (https://github.com/divinsmathew/CubeXdotNet-Rubiks-Cube-Solver) (2021)

using CubeXdotNET;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///  manages the CFOP solver system to be used with front-end unity scripts. - Elijah Gray
/// </summary>
public class CFOPSolver : MonoBehaviour
{

    /// <summary>
    ///  used to grab the function to get the readcube functionality.
    /// </summary>
    private ReadCube readCube;

    /// <summary>
    ///  used to grab the function to get the CubeState functionality.
    /// </summary>
    private CubeState cubeState;


    /// <summary>
    ///  called at the beginning of the program startup to initialize the variables by finding the other objects in the unity scene.
    /// </summary>
    private void Start()
    {
        readCube = FindObjectOfType<ReadCube>(); 
        cubeState = FindObjectOfType<CubeState>();
    }

    // Update is called once per frame
    private void Update()
    {

    }

     /// <summary>
    /// Produces a solution based on how the Kociemba method setup by the original creator setup their solver. The solver first translates the state of the rubiks cube into an order the cfop solver can use by rearranging the facelets in the cube representationstring. It then translates each individual facelet if needed because both programs use different character representations for colors. It then produces a list of moves to follow in rubiks cube notation which are partially translated after due to both programs using different character representations. It then sends these moves in a list of strings to the automate.movelist so that it may be executed. - Elijah Gray
    /// </summary>
    public void Solver2()
    {

        AddedCounter.instance.ResetSteps();              // Added 2/16/23 - Adrian

        string solution = "";

        //translate the Cube
        string translation = string_translation(cfop_initialize());
        Debug.Log("CFOP translated cube: " + translation);

        FridrichSolver cube_to_solve = new FridrichSolver(translation);

        cube_to_solve.Solve();

        solution = cube_to_solve.Solution; // obtains the solution from the CFOP solver

        //Convert the Solved Moves from a string to a list
        List<string> solutionList = StringToList(solution); // converts the CFOP solver's string into a list of moves.

        // converts the solution list into a set of moves that make sense for the visualizer as each use different colors for the faces.
        // for example, the CFOP solver views the green face as the front and the visualizer sees the orange face as the front.
        // -Elijah Gray 2/21/2023
        for (int i = 0; i < solutionList.Count; i++)
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

        //Automate the List
        Automate.moveList = solutionList;

    }


    /// <summary>
    ///  converts the string into a list of strings/steps to be used by the visualizer movement system. Created by the original owner of the twophase solver, I just copypasted it here. - Elijah Gray
    /// </summary>
    /// <param name="solution"> a string representation of all the steps required to solve the cube </param>
    /// <returns> returns a list of strings </returns>
    private List<string> StringToList(string solution)
    {
        List<string> solutionList = new List<string>(solution.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries));
        return solutionList;
    }

    /// <summary>
    /// returns a string representation of the rubiks cube in the visualizer in the way the CFOP solver expects by converting the ascii characters - Elijah Gray
    /// </summary>
    /// <param name="to_translate"> the string representation of the visualizer's cube data  </param>
    /// <returns> returns a string representation of the visualization's CFOP translation </returns>
    private string string_translation(string to_translate)
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


    /// <summary>
    /// returns a string repsentation of the visualizer's cube data in the order the CFOP solver expects so that it may solve it - Elijah Gray
    /// </summary>
    /// <returns> returns a string in the order required for the visualization's CFOP translation </returns>
    private string cfop_initialize()
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




