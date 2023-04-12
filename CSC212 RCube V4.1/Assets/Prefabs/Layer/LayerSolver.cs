// Copyright 2023
// Adrian Damik, Elijah Gray, & Aryan Pothanaboyina

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UziRubiksCube;
using Kociemba;
using UnityEngine.Windows;
using System;
using System.Linq;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

// I am including this because the original creator had included the code of the visualizer in the Kociemba solver.


public class LayerSolver : MonoBehaviour
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



        // executes the solve when the Z key is pressed.
        //if ((UnityEngine.Input.GetKeyUp(KeyCode.Z)))
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

            //Solver3();
        //}

    }


    // Solves the rubik's cube using the Layer method after translating the current unity rubik's cube visualization into a format the beginner cube can input
    //  for example the layer method here does not require the middle facelet as it is implicit and the facelets are ordered clockwise. This is then translated so
    // that it uses the characters expected the visualzier. This is then inputted into the initializer that accepts a user string from the beginner method
    // that calculate all the cubies utilized by the solver. It then will loop as long as the cube remains unsolved. Each iteration through the unsolved loop it will determine
    // what step it is on, produce steps to further solve the cube if possible, execute those steps and repeat until the cube is solved. I added a feature to repeatedly append these steps
    // to a complete list. This complete list is what is utilized to send to the visualizer's automate.movelist which controls what moves the program will follow to solve the visualizer's cube.
    // -Elijah GRay
    public void Solver3()
    {

        AddedCounter.instance.ResetSteps();              // Added 2/16/23 - Adrian

        //Debug.Log("Current visualizer cube string: " + cubeState.GetStateString());
        Cube TestCube = new Cube(); // initializes beginner cube - Elijah

        // 0000 0000  1111111122222222333333334444444455555555
        /*
        int[] test =    { 0, 0, 0, 0, 0, 0, 0, 0, 
                          1, 1, 1, 1, 1, 1, 1, 1,    
                          2, 2, 2, 2, 2, 2, 2, 2,
                          3, 3, 3, 3, 3, 3, 3, 3,
                          4, 4, 4, 4, 4, 4, 4, 4,
                          5, 5, 5, 5, 5, 5, 5, 5 
                         };
        */
        //int[] test; // = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5 };

        //StepCode StepNo = StepCode.WhiteEdges;
        //int StepCounter = 0;
        String str = "";

        
        TestCube.Reset();
        //int[] test = TestCube.FaceArray;
        int[] test = TestCube.ColorArray;


        for (var i = 0; i < test.Length; i++)
        {
            str += test[i].ToString();
        }
        Debug.Log("unmodified string: " + str);





        //Debug.Log("reset() str: " + str);
        //Debug.Log("reset() str: " + string_translation(str));


        //TestCube.RotateArray(2);
        //TestCube.RotateArray(17);

        //TestCube.RotateArray(0);
        //TestCube.RotateArray(15);
        //TestCube.RotateArray(3);
        //TestCube.RotateArray(0);


        //List<string> solutionList = new List<string> {"U"};
        //Automate.moveList = solutionList;


        str = "";
        str = cubeState.Clockwisefaces(); // reads in the rubiks cube with the facelets in the order expected by the beginner solver. - Elijah Gray



        int[] translated_array = new int[48];
        string_translation(str, translated_array); // translates the string representation of the rubiks cube into an array format used by the beginner solver. - Elijah Gray

        // testing - Elijah Gray
        /* 
        string final_output = "";
        for (var i = 0; i < translated_array.Length; i++)
        {
            final_output += translated_array[i].ToString();
        }

        Debug.Log("final output: " + final_output);
        */

        TestCube.ColorArray = translated_array; // both initializes and checks if the cube is valid. 

        int[] check = TestCube.FaceArray;
        string faceArrayString = "";
        for (var i = 0; i < check.Length; i++)
        {
            faceArrayString += check[i].ToString();
        }

        //Debug.Log("resulting face array: " + faceArrayString);


        List<int> all_steps_combined = new List<int>(); // initializes a list of ints that the beginner method uses in codes to represent moves to solve the cube


        SolutionStep solution = TestCube.NextSolutionStep(); // begins the process of solving the rubiks cube for the beginner method, determing which step should be taken first and/or if the cube is already solved.
        

        if (solution.StepCode == StepCode.CubeIsSolved)
        {
            Debug.Log("Cube is solved");
            return;
        }

        //AppendArrayToList(solution.Steps, all_steps_combined);


        // the rubiks cube will generate steps to follow in chunks, then execute them, then continue until the cube is solved. I have it so continues to append these moves to the rubiks cube list until the cube is solved.
        while (solution.StepCode != StepCode.CubeIsSolved)
        {
            if (solution.StepCode == StepCode.CubeIsSolved)
            {
                Debug.Log("Cube is solved");
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
                Debug.Log("Cube is not solved: " + solution.StepCode);
            }
        }


        if (solution.StepCode == StepCode.CubeIsSolved)
        {
            Debug.Log("Cube is DARN solved");
        }

        string all_solutin_steps = new string("");
        for (int i = 0; i < all_steps_combined.Count; i++)
        {
            all_solutin_steps += all_steps_combined[i].ToString();
            all_solutin_steps += " ";

        }
        Debug.Log(all_solutin_steps);


        List<string> solution_steps = new List<string>();

        translate_list(solution_steps, all_steps_combined);

        string translated_steps = "";
        for (int i = 0; i < solution_steps.Count; i++)
        {
            translated_steps += solution_steps[i].ToString();
            translated_steps += " ";

        }


        Debug.Log("follow these steps: " + translated_steps);
        Automate.moveList = solution_steps;










        //TestCube.NextSolutionStep();

        //TestCube


        //FaceArray = TestUserColorArray(value)



        /*
        //int[] test2 = TestCube.FaceArray;
        //int[] test2 = TestCube.ColorArray;


        for (var i = 0; i < test2.Length; i++)
        {
            str += test2[i].ToString();
        }



        Debug.Log("Current visualizer cube string: " + cubeState.GetStateString());

        Debug.Log("reset() after clockwise UP str: " + str);

        string output = "";
        output = cubeState.Clockwisefaces();

        Debug.Log("initial translated str: " + output);
        string string_result = "";

        int[] test3 = new int[48]; 

        string_translation(output, test3);
        for (var i = 0; i < test3.Length; i++)
        {
            string_result += test3[i].ToString();
        }

        Debug.Log("translated str: " + string_result);
        Debug.Log("translated str length: " + string_result.Length);

        Debug.Log(TestCube.AllBlocksInPlace);

        SolutionStep SolveStep = TestCube.NextSolutionStep();

        string solution_output = "";


        //DOESN'T WORK. Followed how they executed the solving.
        while (SolveStep.StepCode != StepCode.CubeIsSolved)
        {

            if(SolveStep.Steps.Length > 0)
            {
                for (int i = 0; i < SolveStep.Steps.Length; i++)
                {
                    solution_output += SolveStep.Steps[i] + " ";
                }

                solution_output += '\n';

            }


            TestCube.RotateArray(SolveStep.Steps);
            Debug.Log(SolveStep.Message);
            SolveStep = TestCube.NextSolutionStep();
            solution_output += SolveStep.Message;


        }

        Debug.Log(TestCube.AllBlocksInPlace);

        Debug.Log("outputted solution: " + solution_output);
        */




    }


    // converts the string into a list of steps to be used by the visualizer rotating automaton
    List<string> StringToList(string solution)
        {
            List<string> solutionList = new List<string>(solution.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries));
            return solutionList;
        }


    // translates the visualizer facelets into a format the layer solver uses
    // -Elijah Gray 2/21/2023
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


  
    // given an input array, the function then adds its elements to the input list.
    // - Elijah Gray
    public static void AppendArrayToList<T>(T[] array, List<T> list)
    {
        for (int i = 0; i < array.Length; i++)
        {
            list.Add(array[i]);
        }
    }


    // acts as a lookup table to convert the number moves to string moves utilizes by the rubiks cube visualizer.
    // Given more time I would have just made it an array of pairs since the number elements are in a neat order.
    // - Elijah Gray
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

    // given an list of integers and a list of strings, it takes the integer list and goes element by elemenet to append its translation to the string list.
    // - Elijah Gray
    void translate_list(List<string> output,  List<int> input) 
    {
        for(int i = 0; i < input.Count;i++)
        {

            output.Add(Move_Lookup[input[i]]);
            //output.Append(Move_Lookup[input[i]]);
        }
    
    }






}




