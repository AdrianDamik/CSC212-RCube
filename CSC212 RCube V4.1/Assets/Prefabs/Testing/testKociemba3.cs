// COPYRIGHT 2023 ELIJAH GRAY, FEEL FREE TO REUSE! :)

using CubeXdotNET;
using Kociemba;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Windows;


// test base created by Elijah Gray. Added more automation features - Elijah Gray
public class testKociemba3 : MonoBehaviour
{
    private CubeState cubeState;
    private SolveTwoPhase solver;

    List<List<string>> list_of_shuffles = new List<List<string>>(); // initialized by the improved shuffle generator.
    int list_shuffles_index = 0; // index of the shuffle generator for when we want to compare.

    List<List<string>> list_of_solutions = new List<List<string>>();

    List<List<string>> saved_list_of_solutions = new List<List<string>>(); // initialized by load function. Used to check the results of a test already done before.

    List<int> solution_counts = new List<int>();

    // list of moves, copypasted from the Kociemba solver file. - Elijah Gray
    private readonly List<string> allMoves = new List<string>()
    {
        "U", "D", "L", "R", "F", "B",
        "U2", "D2", "L2", "R2", "F2", "B2",
        "U'", "D'", "L'", "R'", "F'", "B'"
    };

    int current_step; // current instruction step  0 - > scramble,  1 -> solve,  2 -> check if solved properly.
    int wait_frames; // how many frames should the program wait before going to the next step. Currently a solution but an ineffecient one because sometimes the rubiks cube will try to go to the next step while its still movingm causing the program
    // to crash 
    // - Elijah Gray

    bool test; // boolean to determine whether a test should occur.
    bool test_already_happened; // determines whether the same test has already been saved in a file solution. Will use these to check results. - Elijah Gray


    // SET TEST VALUES HERE
    // used to specify specific scrambles. These MUST be initialized.
    const int seed = 64000;
    const int number_of_shuffles = 100;
    const int length_of_shuffles = 100;

    int solves_left = number_of_shuffles; // number of cubes to solve. 
    int[] data_gathering = new int[500]; // used an array in place of a bar graph. Will increment an index at the count of moves in solutions.

    // Start is called before the first frame update
    void Start()
    {
        current_step = 0;
        wait_frames = 0;

        cubeState = FindObjectOfType<CubeState>();
        solver = FindObjectOfType<SolveTwoPhase>();
        test_already_happened = false;
    }

    // initates a test to solve a number of cubes as per the variable "solves_left" -Elijah Gray
    // the variable number_of_shuffles determiens how many shuffles should be done. The length_of_shuffles variable determines how many moves should be in each shuffle.
    // the variable seed determines the seed in the shuffle command's number generator.
    // Update is called once per frame
    void Update()
    {

        if (CubeState.autoRotating)
        {
            Debug.Log("cube is currently automating, skipping");
            return;
        }

        if ((UnityEngine.Input.GetKeyUp(KeyCode.K)))
        {
            test = true;
            //solves_left = 1000; // how many cubes should be solved for the test.
            list_of_shuffles = GenerateShuffles(number_of_shuffles, length_of_shuffles, seed);
            solves_left = number_of_shuffles;
            Debug.Log("layer test initiated");
            Time.timeScale = 100;

            string filename = "Kociemba method test( " + seed.ToString() + " " + number_of_shuffles.ToString() + " " + length_of_shuffles.ToString() + ").txt";
            string filePath = Path.Combine(Application.persistentDataPath, filename);

            if (System.IO.File.Exists(filePath))
            {
                Debug.LogWarning("File already exists, comparing results...");
                saved_list_of_solutions = LoadSolutions(filename);
                test_already_happened = true;

            }




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
                string filename = "Kociemba method test( " + seed.ToString() + " " + number_of_shuffles.ToString() + " " + length_of_shuffles.ToString() + ").txt"; // file format: test parameters( # # # ).txt
                test = false; // sets test to false to prevent an infinite loop and to tell the program the test has finished.
                list_shuffles_index = 0; // resets index back to zero for another test.

                if (!test_already_happened)
                {
                    SaveSolutions(filename, list_of_solutions); // saves the solution to a file for future testing and comparison.
                    Debug.Log("Solution saved to file!");
                } else
                {
                    Debug.Log("file already exists, not saving.");
                }

                display_data();

            }
        }

    }

    // sets the automate.movelist to the list of shuffles at the index in the list of shuffles generated by the improved scramble generator. - Elijah Gray
    void seeded_scramble()
    {

        if (list_of_shuffles.Count == 0)
        {
            return;
        }

        Automate.moveList = list_of_shuffles[list_shuffles_index];
        ++list_shuffles_index;

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


    // handles the process of going step to step for the tests. The first step is to scramble the cube. The second step is to generate a list of moves to solve the cube. The third step is to check if the cube is properly solved
    // - Elijah Gray
    void test_step()
    {

       try
        {


            Debug.Log("Cubes left:" + solves_left + "and current step is: " + current_step);
            switch (current_step)
            {
                case 0:
                    Debug.Log("1st phase:");
                    wait_frames = 10; // 240 * Convert.ToInt32(Time.timeScale);
                    //Scramble();
                    seeded_scramble();
                    ++current_step;
                    break;

                case 1:
                    Debug.Log("2nd phase:");
                    wait_frames = 10; // * Convert.ToInt32(Time.timeScale);
                    solver.Solver();
              
                    if (test_already_happened)
                    {
                        bool sequence_equal = saved_list_of_solutions[list_shuffles_index - 1].SequenceEqual(Automate.moveList);
                        Assert.IsTrue(sequence_equal); // checks if the solution is the same as the old solution for the same scramble at the current index. -Elijah Gray
                    } else
                    {
                        List<string> current_solution = new List<string>(Automate.moveList);
                        list_of_solutions.Add(current_solution); // append current solution to solution_list to be saved later. - E
                    }

                    solution_counts.Add(Automate.moveList.Count);
                    ++data_gathering[Automate.moveList.Count]; // increment data gathering array at index equal to the number of moves used to produce a solution. - Elijah Gray
                    
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
            throw (e);
        }


    }

    // generates a number of shuffle sequences of variable length using a seed for the random number generation.
    // numshuffles determines the number of shuffles that should be done.
    // shuffleLength determines how many moves each shuffle should have.
    // the seed determines the random number generation.
    // - Elijah Gray
    public List<List<string>> GenerateShuffles(int numShuffles, int shuffleLength, int seed)
    {
        UnityEngine.Random.InitState(seed);
        int moveIndex = 0;
        List<List<string>> shuffles = new List<List<string>>();

        for (int i = 0; i < numShuffles; i++)
        {
            List<string> shuffle = new List<string>();
            for (int j = 0; j < shuffleLength; j++)
            {
                moveIndex = UnityEngine.Random.Range(0, allMoves.Count);
                shuffle.Add(allMoves[moveIndex]);
            }
            shuffles.Add(shuffle);
        }

        return shuffles;
    }

    // Saves the solutions produced by the solver into a file that can be accessed later.
    // the filename is what the file being saved should be named as.
    // the solutions parameter's lists of strings are saved line by line after in the program to be accessed later.
    // - Elijah Gray
    public static void SaveSolutions(string filename, List<List<string>> solutions)
    {
        string filePath = Path.Combine(Application.persistentDataPath, filename);

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // Write the first line with seed, num shuffles, and shuffle length
            //writer.WriteLine(string.Format("{0} {1} {2}", seed, numShuffles, shuffleLength));

            // Write each solution as a separate line in the file
            foreach (List<string> solution in solutions)
            {
                foreach (string element in solution)
                {
                    writer.Write(element + " ");
                }
                writer.WriteLine();
            }
        }

        //solutions.Clear(); // wipe list of solutions, old solutions no longer needed.

        Debug.Log("Saved solutions to " + filePath);
    }


    // loads a set of solutions by a string for the filename.
    // returns the list of list of strings used for all the shuffles in a test.
    // used to initialize the saved_solution_list for the purpose of verifying results. 
    // - Elijah Gray
    public static List<List<string>> LoadSolutions(string filename)
    {
        string filePath = Path.Combine(Application.persistentDataPath, filename);

        List<List<string>> solutions = new List<List<string>>();

        using (StreamReader reader = new StreamReader(filePath))
        {
            // Read each solution line by line and add it to the list of solutions
            while (!reader.EndOfStream)
            {
                string[] elements = reader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                solutions.Add(new List<string>(elements));
            }

        }

        Debug.Log("Loaded solutions from " + filePath);

        return solutions;
    }


    // displays all the data gathered during a test and displays the average moves used in the test.
    // - Elijah Gray
    void display_data()
    {


  

        string output = "testing parameters--(Seed: " + seed + ".  Number of shuffles: " + number_of_shuffles + ". Length of shuffles: " + length_of_shuffles + ")." +  "\n";
        double sum = 0;


        for(int i = 0; i < solution_counts.Count; i++)
        {
            output += solution_counts[i];
            output += ", ";
            sum += solution_counts[i];
        }
        output += "\n";

        /*
        for (int i = 0; i < this.data_gathering.Length; i++)
        {
            if (data_gathering[i] > 0)
            {
                output += i + ": " + this.data_gathering[i] + "\n";
                sum += this.data_gathering[i] * (i);
            }
        }
        */

        output += "Sum of data: " + sum + "\n"; // total number of moves used in test.

        double average = Math.Ceiling((double)sum / (double)solution_counts.Count); // use double to calculate the average then round up because moves cannot be fractional.
        output += "And the average number of moves is: " + average.ToString() + "\n";

        output += "And the total number of moves is: " + solution_counts.Count + "\n";

        Debug.Log(output);
    }



}
















