// COPYRIGHT 2023 ELIJAH GRAY, FEEL FREE TO REUSE! :)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// blackbox test base & data collection system created by Elijah Gray. Added more automation features & file saving/loading in v4 - Elijah Gray
/// this test is for the Beginner method.
/// </summary>
public class testLayer4 : MonoBehaviour
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
    /// initialized by the improved shuffle generator.
    /// </summary>
    List<List<string>> list_of_shuffles = new List<List<string>>();

    /// <summary>
    /// index of the shuffle generator for when we want to compare.
    /// </summary>
    int list_shuffles_index = 0;

    /// <summary>
    /// used to save the data after a test.
    /// </summary>
    List<List<string>> list_of_solutions = new List<List<string>>();

    /// <summary>
    /// initialized by load function. Used to check the results of a test already done before.
    /// </summary>
    List<List<string>> saved_list_of_solutions = new List<List<string>>();

    /// <summary>
    /// a list of integers added onto during a test for data collection w/ the number of moves in a solution.
    /// </summary>
    List<int> solution_counts = new List<int>();

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
    /// used to determine whether the same test has already been saved in a file solution. Will use these to check results. - Elijah Gray
    /// </summary>
    bool test_already_happened; 

    //////////////////////////////////////////////////////////////////
    // SET TEST VALUES HERE
    // used to specify specific scrambles. These MUST be initialized.

    /// <summary>
    /// the number used to initialize the random number generator in the test.
    /// </summary>
    const int seed = 64000;

    /// <summary>
    /// how many shuffles should be in a test.
    /// </summary>
    const int number_of_shuffles = 100;

    /// <summary>
    /// how many moves should be in each shuffle.
    /// </summary>
    const int length_of_shuffles = 100;

    //////////////////////////////////////////////////////////////////

    /// <summary>
    /// the number of cubes to solve left in a test. Initially set to the number of cubes to solve via the number of shuffles variable.
    /// </summary>
    int solves_left = number_of_shuffles; // number of cubes to solve. 


    /// <summary>
    /// used to gather data post-test. An index is incremented when a cube solution's length of the same number is the result.
    /// </summary>
    int[] data_gathering = new int[500]; // used an array in place of a bar graph. Will increment an index at the count of moves in solutions.


    /// <summary>
    /// called at the beginning of the program to initialize the test.
    /// </summary>
    void Start()
    {
        current_step = 0;
        wait_frames = 0;

        cubeState = FindObjectOfType<CubeState>();
        solver = FindObjectOfType<LayerSolver>();
        test_already_happened = false;
    }

    /// <summary>
    /// initates a test if the F6 button is pressed to solve a number of cubes as per the variable "solves_left" -Elijah Gray
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

        if ((UnityEngine.Input.GetKeyUp(KeyCode.F6)))
        {
            test = true;
            //solves_left = 1000; // how many cubes should be solved for the test.
            list_of_shuffles = GenerateShuffles(number_of_shuffles, length_of_shuffles, seed);
            solves_left = number_of_shuffles;
            Debug.Log("layer test initiated");
            Time.timeScale = 100;

            string filename = "Layer method test( " + seed.ToString() + " " + number_of_shuffles.ToString() + " " + length_of_shuffles.ToString() + ").txt";
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
                string filename = "Layer method test( " + seed.ToString() + " " + number_of_shuffles.ToString() + " " + length_of_shuffles.ToString() + ").txt"; // file format: test parameters( # # # ).txt
                test = false; // sets test to false to prevent an infinite loop and to tell the program the test has finished.
                list_shuffles_index = 0; // resets index back to zero for another test.

                if (!test_already_happened)
                {
                    SaveSolutions(filename, list_of_solutions); // saves the solution to a file for future testing and comparison.
                    Debug.Log("Solution saved to file!");
                }
                else
                {
                    Debug.Log("file already exists, not saving.");
                }

                display_data();

            }
        }

    }

    /// <summary>
    /// sets the automate.movelist to the list of shuffles at the index in the list of shuffles generated by the improved scramble generator. - Elijah Gray
    /// </summary>
    void seeded_scramble()
    {

        if (list_of_shuffles.Count == 0)
        {
            return;
        }

        Automate.moveList = list_of_shuffles[list_shuffles_index];
        ++list_shuffles_index;

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
    /// handles the process of going step to step for the tests. The first step is to scramble the cube. The second step is to generate a list of moves to solve the cube. The third step is to check if the cube is properly solved. - Elijah Gray
    /// </summary>
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
                    solver.Solver3();

                    if (test_already_happened)
                    {
                        bool sequence_equal = saved_list_of_solutions[list_shuffles_index - 1].SequenceEqual(Automate.moveList);
                        Assert.IsTrue(sequence_equal); // checks if the solution is the same as the old solution for the same scramble at the current index. -Elijah Gray
                    }
                    else
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

    /// <summary>
    /// generates a number of shuffle sequences of variable length using a seed for the random number generation.
    /// numshuffles determines the number of shuffles that should be done.
    /// shuffleLength determines how many moves each shuffle should have.
    /// the seed determines the random number generation.
    /// - Elijah Gray
    /// </summary>
    /// <param name="numShuffles"> the number of shuffles to be used in a test </param>
    /// <param name="shuffleLength"> how long each shuffle should be w/ their amount of moves </param>
    /// <param name="seed"> the number used to initialize the random generator in the shuffle generator </param>
    /// <returns></returns>
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


    /// <summary>
    /// Saves the solutions produced by the solver into a file that can be accessed later.
    /// the filename is what the file being saved should be named as.
    /// the solutions parameter's lists of strings are saved line by line after in the program to be accessed later.
    /// - Elijah Gray
    /// </summary>
    /// <param name="filename"> the filename the file should be saved as </param>
    /// <param name="solutions"> the list of list of strings to be saved into the file. These list of strings are the solution for their corresponding cube shuffle. </param>
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


    /// <summary>
    /// loads a set of solutions by a string for the filename.
    /// returns the list of list of strings used for all the shuffles in a test.
    /// used to initialize the saved_solution_list for the purpose of verifying results. 
    /// - Elijah Gray
    /// </summary>
    /// <param name="filename"> the name of the file that should be loaded </param>
    /// <returns> returns a list of list of strings w/ each string being a move in a solution used to solve a cube for comparisons to confirm the program's deterministic nature. </returns>
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

    /// <summary>
    /// goes through the data stored while the last test ran to collect data. Displays this data in the console log in the unity developer engine. 
    /// </summary>
    void display_data()
    {

        string output = "testing parameters--(Seed: " + seed + ".  Number of shuffles: " + number_of_shuffles + ". Length of shuffles: " + length_of_shuffles + ")." + "\n";
        double sum = 0;


        for (int i = 0; i < solution_counts.Count; i++)
        {
            output += solution_counts[i];
            output += ", ";
            sum += solution_counts[i];
        }
        output += "\n";

        output += "Sum of data: " + sum + "\n"; // total number of moves used in test.
        double average = Math.Ceiling((double)sum / (double)solution_counts.Count); // use double to calculate the average then round up because moves cannot be fractional.
        output += "And the average number of moves is: " + average.ToString() + "\n";
        output += "And the total number of moves is: " + solution_counts.Count + "\n";

        Debug.Log(output);
    }



}
















