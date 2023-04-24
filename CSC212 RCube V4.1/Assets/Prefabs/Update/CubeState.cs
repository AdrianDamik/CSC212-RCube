// Created by Megalomatt (https://github.com/Megalomatt/unity-rcube) (2020)
// Revised by hamzazmah (https://github.com/hamzazmah/RubiksCubeUnity-Tutorial) (2020)
// Revised by Elijah Gray (2023)

using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
///  Modified function created by the original developers. Manages the cube's state so it can be read and manipulated. -Elijah Gray
/// </summary>
public class CubeState : MonoBehaviour
{

    /// <summary>
    /// the front side of the cube w/ a list of all its cubies.
    /// </summary>
    public List<GameObject> front = new List<GameObject>();

    /// <summary>
    /// the back side of the cube w/ a list of all its cubies.
    /// </summary>
    public List<GameObject> back = new List<GameObject>();

    /// <summary>
    /// the up side of the cube w/ a list of all its cubies.
    /// </summary>
    public List<GameObject> up = new List<GameObject>();

    /// <summary>
    /// the rdown side of the cube w/ a list of all its cubies.
    /// </summary>
    public List<GameObject> down = new List<GameObject>();

    /// <summary>
    /// the left side of the cube w/ a list of all its cubies.
    /// </summary>
    public List<GameObject> left = new List<GameObject>();

    /// <summary>
    /// the right side of the cube w/ a list of all its cubies.
    /// </summary>
    public List<GameObject> right = new List<GameObject>();


    /// <summary>
    /// Whether the rubik's cube is autorotating currently.
    /// </summary>
    public static bool autoRotating = false;

    /// <summary>
    /// Whether the program has started, utilized by the kociemba solver.
    /// </summary>
    public static bool started = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    ///  Only slightly modified while debugging. Very important function that rabs 9 cubies (mini cubes) off the main cube object to make a temporary object to be rotated so a rotation can be performed.
    /// </summary>
    /// <param name="cubeSide"> the side of the cube that should be grabbed </param>
    public void PickUp(List<GameObject> cubeSide)
    {
        Debug.Log(cubeSide.Count);
        if (cubeSide.Count != 9)
        {
            Debug.Log("ERROR IN PICKUP");
        }
        foreach (GameObject face in cubeSide)
        {
            if (face != cubeSide[4])
            {
                face.transform.parent.transform.parent = cubeSide[4].transform.parent;
            }
        }

    }

   

    /// <summary>
    /// This part of the program, unmodified from the original creators github. Sets down the 9 facelets picked up by the Pickup function
    /// and reattaches them to a pivot point on the rubiks cube inputted.
    /// the littlecubes are the facelets of the rubiks cube and the pivot points are how we can turn the rubiks cube.
    /// -Elijah Gray
    /// </summary>
    /// <param name="littleCubes"> the mini cubes grabbed previously that should be reattached to the main cube. </param>
    /// <param name="pivot"> the pivot the rotation was performed around when the cubies were picked up. </param>
    public void PutDown(List<GameObject> littleCubes, Transform pivot)
    {
        foreach (GameObject littleCube in littleCubes)
        {
            if (littleCube != littleCubes[4])
            {
                littleCube.transform.parent.transform.parent = pivot;
            }
        }
    }


    /// <summary>
    /// Part of the original owner's work via the tutorial/github page. Given a side of the rubiks cube, return a string representation of that
    /// side of the cube by adding each and every character to the string. -Elijah Gray
    /// </summary>
    /// <param name="side"> the side of the cube that we wish to obtain data from </param>
    /// <returns> the string representation of the side used as input </returns>
    string GetSideString(List<GameObject> side)
    {
        string sideString = "";
        foreach (GameObject face in side)
        {
            sideString += face.name[0].ToString();
        }
        return sideString;
    }

    /// <summary>
    /// Part of the original owner's work via the tutorial/github page of the person who originally followed the tutrial
    /// gets the string representation of the rubiks cube for the Kociemba method's format so it may solve a rubiks cube.
    /// - Elijah Gray
    /// </summary>
    /// <returns> returns the entire cube's state in string representation for the Kociemba solver </returns>
    public string GetStateString()
    {
        string stateString = "";
        stateString += GetSideString(up);
        //stateString += " ";
        stateString += GetSideString(right);
        //stateString += " ";
        stateString += GetSideString(front);
        //stateString += " ";
        stateString += GetSideString(down);
        //stateString += " ";
        stateString += GetSideString(left);
        //stateString += " ";
        stateString += GetSideString(back);
        return stateString;
    }

    /// <summary>
    /// Gets the op side of the cube.
    /// </summary>
    /// <returns> returns the visualizer's string representation of the up side of the cube. -Elijah Gray </returns>
    public string get_up()
    {
        return GetSideString(up);
    }

    /// <summary>
    /// Gets the right side of the cube.
    /// </summary>
    /// <returns> returns the visualizer's string representation of the right side of the cube. -Elijah Gray </returns>
    public string get_right()
    {
        return GetSideString(right);
    }

    /// <summary>
    /// Gets the front side of the cube.
    /// </summary>
    /// <returns> returns the visualizer's string representation of the front side of the cube. -Elijah Gray </returns>
    public string get_front()
    {
        return GetSideString(front);
    }

    /// <summary>
    /// Gets the bottom side of the cube.
    /// </summary>
    /// <returns> returns the visualizer's string representation of the down side of the cube. -Elijah Gray </returns>
    public string get_down()
    {
        return GetSideString(down);
    }

    /// <summary>
    /// Gets the left side of the cube.
    /// </summary>
    /// <returns> returns the visualizer's string representation of the left side of the cube. -Elijah Gray </returns>
    public string get_left()
    {
        return GetSideString(left);
    }
    /// <summary>
    /// Gets the back side of the cube.
    /// </summary>
    /// <returns> returns the visualizer's string representation of the back side of the cube. -Elijah Gray </returns>
    public string get_back()
    {
        return GetSideString(back);
    }

    /// <summary>
    ///  Obtains part of the string representation of the Rubik's cube for the layer solver in the order for the "middle" faces IE the left, right, front, and back.
    /// </summary>
    /// <param name="side"> the side of the cube </param>
    /// <returns> returns the string representation of the side of the cube in the order needed by the middle faces of the beginner soolver. </returns>
    string GetSideClockwise(List<GameObject> side)
    {


        //layer solver does not include middle [4] facelet because it is implicit as the green face will always have a green center facelet and vice versa.
        string sideString = "";

        sideString += side.ElementAt(0).name[0].ToString();
        sideString += side.ElementAt(1).name[0].ToString();
        sideString += side.ElementAt(2).name[0].ToString();
        sideString += side.ElementAt(5).name[0].ToString();

        sideString += side.ElementAt(8).name[0].ToString();
        sideString += side.ElementAt(7).name[0].ToString();
        sideString += side.ElementAt(6).name[0].ToString();
        sideString += side.ElementAt(3).name[0].ToString();

        return sideString;
    }


    /// <summary>
    ///  Obtains part of the string representation of the Rubik's cube for the layer solver in the order for the "vertical" faces IE the top and bottom.
    /// </summary>
    /// <param name="side"> the side of the cube </param>
    /// <returns> returns the string representation of the side of the cube in the order needed by the vertical faces of the beginner soolver. </returns>
    string GetVerticalSideClockwise(List<GameObject> side)
    {

        //layer solver does not include middle [4] facelet because it is implicit as the green face will always have a green center facelet and vice versa.
        string sideString = "";


        sideString += side.ElementAt(8).name[0].ToString();
        sideString += side.ElementAt(7).name[0].ToString();
        sideString += side.ElementAt(6).name[0].ToString();
        sideString += side.ElementAt(3).name[0].ToString();

        sideString += side.ElementAt(0).name[0].ToString();
        sideString += side.ElementAt(1).name[0].ToString();
        sideString += side.ElementAt(2).name[0].ToString();
        sideString += side.ElementAt(5).name[0].ToString();

        return sideString;
    }

    /// <summary>
    /// Takes the rubiks cube and returns a string representing the entire rubiks cube for the beginner method.
    /// the facelets are in the order expected per face as is the order of the faces themselves for the string representation.
    /// - Elijah Gray
    /// </summary>
    /// <returns> returns the string representation of the visualizer's cube w/ the facelets of the cube in the order required by the layer solver. </returns>
    public string Clockwisefaces()
    {
        string stateString = "";

        stateString += GetVerticalSideClockwise(down); // white
        stateString += GetSideClockwise(right); // blue
        stateString += GetSideClockwise(back); // red
        stateString += GetSideClockwise(left); // green 
        stateString += GetSideClockwise(front); // orange
        stateString += GetVerticalSideClockwise(up); // yellow

        return stateString;
    }

}
