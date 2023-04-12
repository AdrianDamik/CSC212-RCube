// Copyright 2023
// Adrian Damik, Elijah Gray, & Aryan Pothanaboyina

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO;
using System.Text;

public class AddedSeedGenerator : MonoBehaviour
{
    public static AddedSeedGenerator instance;

    private string seedInput;
    private int stepInput;
    public InputField myInputFieldSeed;
    public InputField myInputFieldSteps;

    // retrieves input from InputFields for seed & move steps, sends to hash function to generate move list, then sends move list to automate
    public void ReadInput() { 
        seedInput = myInputFieldSeed.text;
        stepInput = int.Parse(myInputFieldSteps.text);
        Debug.Log("Seed entered: " + seedInput + " Amount of steps: " + stepInput);
        AddedCounter.instance.ResetSteps();
        Automate.moveList = hash(seedInput, stepInput);
    }

    public List<string> hash(string seed, int numMoves = 15) {
        List<string> allMoves = new List<string>() // rubik's cube move list
        {
            "U", "D", "L", "R", "F", "B",
            "U2", "D2", "L2", "R2", "F2", "B2",
            "U'", "D'", "L'", "R'", "F'", "B'"
        };

        int hash = 0;
        List<string> shuffle = new List<string>() {}; // list for overall moves after generation

        foreach (char x in seed) 
        {
            hash = hash + x; // sum of ascii numbers from seed
        }

        System.Random rnd = new System.Random(hash); // C# equivelent of srand(); will seed random numbers; same seeds generate same random numbers

        for(int i = 0; i < numMoves; i++)
        {
           int rndNum = rnd.Next(0, allMoves.Count); // random number generated [0, 18)
           string move = allMoves[rndNum]; // uses random number to retrieve a move from list
           shuffle.Add(move); // appends each generated move to a string of all moves generated
        }

        Debug.Log("Movelist length: " + numMoves);
        Debug.Log("Movelist after seed: " + System.String.Join(", ", shuffle.ToArray()));
        //Automate.moveList = shuffle; // sends generated move list to automate.cs
        return shuffle;
    }

    public Queue<System.Tuple<string, int>> ReadSeeds() {
        string listOfSeeds = Application.dataPath + "//Prefabs//Added Adrian//seeds.txt";
        Queue<System.Tuple<string, int>> my_queue = new Queue<System.Tuple<string, int>>(); // create new queue with list of seeds
        
        using (StreamReader streamReader = new StreamReader(listOfSeeds)) {
            string readContents = streamReader.ReadToEnd();
            string[] lines = readContents.Split('\n');
            foreach (string s in lines) {
                string[] lines2 = s.Split(',');
                my_queue.Enqueue(System.Tuple.Create(lines2[0], System.Int32.Parse(lines2[1])));
            }
        }
        return my_queue;
    }
}
