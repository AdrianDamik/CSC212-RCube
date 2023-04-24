// Created by Uzi Granot (https://www.codeproject.com/Articles/1199528/Rubik-s-Cube-for-Beginners-Version-2-0-Csharp-Appl) (2022)
// Revised by Elijah Gray & Adrian Damik (2023)

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UziRubiksCube;


/// <summary>
///  manages the Layer solver system to be used with front-end unity scripts. - Elijah Gray
/// </summary>
public class LayerSolver : MonoBehaviour
{
    // Start is called before the first frame update

    /// <summary>
    ///  used to grab the function to get the readcube functionality.
    /// </summary>
    private ReadCube readCube;

    /// <summary>
    ///  used to grab the function to get the CubeState functionality.
    /// </summary>
    private CubeState cubeState;

    void Start()
    {
        readCube = FindObjectOfType<ReadCube>(); // obtains the data from the rubix cube -Elijah Gray 2/21/2023
        cubeState = FindObjectOfType<CubeState>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    /// <summary>
    /// Solves the rubik's cube using the Layer method after translating the current unity rubik's cube visualization into a format the beginner cube can input
    /// for example the layer method here does not require the middle facelet as it is implicit and the facelets are ordered clockwise. This is then translated so
    /// that it uses the characters expected the visualzier. This is then inputted into the initializer that accepts a user string from the beginner method
    /// that calculate all the cubies utilized by the solver. It then will loop as long as the cube remains unsolved. Each iteration through the unsolved loop it will determine
    /// what step it is on, produce steps to further solve the cube if possible, execute those steps and repeat until the cube is solved. I added a feature to repeatedly append these steps
    /// to a complete list. This complete list is what is utilized to send to the visualizer's automate.movelist which controls what moves the program will follow to solve the visualizer's cube.
    /// -Elijah Gray
    /// </summary>
    public void Solver3()
    {

        AddedCounter.instance.ResetSteps();              // Added 2/16/23 - Adrian

        Cube TestCube = new Cube(); // initializes beginner cube - Elijah

        String str = "";

        TestCube.Reset();
        int[] test = TestCube.ColorArray;

        str = cubeState.Clockwisefaces(); // reads in the rubiks cube with the facelets in the order expected by the beginner solver. - Elijah Gray

        int[] translated_array = new int[48];
        string_translation(str, translated_array); // translates the string representation of the rubiks cube into an array format used by the beginner solver. - Elijah Gray

        TestCube.ColorArray = translated_array; // both initializes and checks if the cube is valid. 

        List<int> all_steps_combined = new List<int>(); // initializes a list of ints that the beginner method uses in codes to represent moves to solve the cube


        SolutionStep solution = TestCube.NextSolutionStep(); // begins the process of solving the rubiks cube for the beginner method, determing which step should be taken first and/or if the cube is already solved.

        if (solution.StepCode == StepCode.CubeIsSolved)
        {
            //Debug.Log("Cube is solved");
            return;
        }

        // the rubiks cube will generate steps to follow in chunks, then execute them, then continue until the cube is solved. I have it so continues to append these moves to the rubiks cube list until the cube is solved.
        while (solution.StepCode != StepCode.CubeIsSolved)
        {
            if (solution.StepCode == StepCode.CubeIsSolved)
            {
                //Debug.Log("Cube is solved");
                return;
            }
            else
            {
                AppendArrayToList(solution.Steps, all_steps_combined);

                for (int i = 0; i < solution.Steps.Length; i++)
                {
                    TestCube.RotateArray(solution.Steps[i]);
                }

                solution = TestCube.NextSolutionStep();
                //Debug.Log("Cube is not solved: " + solution.StepCode);
            }
        }

        List<string> solution_steps = new List<string>();

        translate_list(solution_steps, all_steps_combined);

        Automate.moveList = solution_steps;
    }


    /// <summary>
    /// translates the visualizer facelets into a format the layer solver uses by changing the string into an integer array -Elijah Gray
    /// </summary>
    /// <param name="to_translate">the input string representation of the visualizer rubik's cube in the proper order used by the layer method</param>
    /// <param name="input_array">the output array of integer representation of the layer method.  </param>
    void string_translation(string to_translate, int[] input_array)
    {
        // 8 facelets stored per face per the layer solver
        // (8 * 6) - 1 = 47 (0-47)
        for (int i = 0; i < 48; i++)
        {

            switch (to_translate.ElementAt(i))
            {
                case 'U':
                    input_array[i] = 5;
                    break;

                case 'D':
                    input_array[i] = 0;
                    break;

                case 'L':
                    input_array[i] = 3;
                    break;

                case 'R':
                    input_array[i] = 1;
                    break;

                case 'F':
                    input_array[i] = 4;
                    break;

                case 'B':
                    input_array[i] = 2;
                    break;

                default:
                    break;

            }
        }

    }

    /// <summary>
    /// Given an input array, the function then adds its elements to the string input list. - Elijah Gray
    /// </summary>
    /// <typeparam name="T"> element type of list & array </typeparam>
    /// <param name="array"> the array of elements  to be used as input. The layer solver represents moves as integers.  </param>
    /// <param name="list"> the list of elements to be added onto. This is used to hold all the moves outputted by the layer solver.  </param>
    public static void AppendArrayToList<T>(T[] array, List<T> list)
    {
        for (int i = 0; i < array.Length; i++)
        {
            list.Add(array[i]);
        }
    }


    /// <summary>
    /// acts as a lookup table to convert the number moves to string moves utilizes by the rubiks cube visualizer Given more time I would have just made it an array of pairs since the number elements are in a neat order, or just a switch case. - Elijah Gray
    /// </summary>
    Dictionary<int, string> Move_Lookup = new Dictionary<int, string>()
    {

        {0, "D"},
        {1, "D2"},
        {2, "D'"},

        {3, "R"},
        {4, "R2"},
        {5, "R'"},

        {6, "B"},
        {7, "B2"},
        {8, "B'"},

        {9, "L"},
        {10, "L2"},
        {11, "L'"},

        {12, "F"},
        {13, "F2"},
        {14, "F'"},

        {15, "U"},
        {16, "U2"},
        {17, "U'"}

    };

    /// <summary>
    ///  given an list of integers and a list of strings, it takes the integer list and goes element by elemenet to append its translation to the string list. - Elijah Gray
    /// </summary>
    /// <param name="output"> the list of strings to hold all the translated moves as an output </param>
    /// <param name="input"> the list of integers to hold all the moves to be translated as an input </param>
    void translate_list(List<string> output, List<int> input)
    {
        for (int i = 0; i < input.Count; i++)
        {
            output.Add(Move_Lookup[input[i]]);
        }

    }

}
