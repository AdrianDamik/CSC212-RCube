// Copyright 2023
// Adrian Damik, Elijah Gray, & Aryan Pothanaboyina

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeState : MonoBehaviour
{
    //Sides
    public List<GameObject> front = new List<GameObject>();
    public List<GameObject> back = new List<GameObject>();
    public List<GameObject> up = new List<GameObject>();
    public List<GameObject> down = new List<GameObject>();
    public List<GameObject> left = new List<GameObject>();
    public List<GameObject> right = new List<GameObject>();

    public int number_of_pivots = 0;

    public static bool autoRotating = false;

    public static bool started = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // part of the original developer's program
    // this part of the program creates a "new object" to allow a face composed of 9 facelets to be moved.
    // the cubeside parameter is the side of the cube that is turned into a new object to be moved.
    // - Elijah Gray
    public void PickUp(List<GameObject> cubeSide)
    {
        Debug.Log(cubeSide.Count);
        if(cubeSide.Count != 9)
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

    // this part of the program, unmodified from the original creators github. Sets down the 9 facelets picked up by the Pickup function
    //and reattaches them to a pivot point on the rubiks cube inputted.
    // the littlecubes are the facelets of the rubiks cube and the pivot points are how we can turn the rubiks cube.
    /// -Elijah Gray
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

    // part of the original owner's work via the tutorial/github page. Given a side of the rubiks cube, return a string representation of that
    // side of the cube by adding each and every character to the string. 
    // This could be made more effecient like other parts of the program by using stringbuilder but I didn't have enough time.
    // - Elijah Gray
    string GetSideString(List<GameObject> side)
    {
        string sideString = "";
        foreach (GameObject face in side)
        {
            sideString += face.name[0].ToString();
        }
        return sideString;
    }

    // part of the original owner's work via the tutorial/github page of the person who originally followed the tutrial
    // gets the string representation of the rubiks cube for the Kociemba method's format so it may solve a rubiks cube.
    // - Elijah Gray
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

    // getter provides the up side of the rubiks cube
    // - Elijah Gray
    public string get_up()
    {
        return GetSideString(up);
    }

    // getter provides the right side of the rubiks cube
    // - Elijah Gray
    public string get_right()
    {
        return GetSideString(right);
    }

    // getter provides the front side of the rubiks cube
    // - Elijah Gry
    public string get_front()
    {
        return GetSideString(front);
    }

    // getter provides the bottom side of the rubiks cube in a string representation
    // - Elijah Gray
    public string get_down()
    {
        return GetSideString(down);
    }

    // getter provides the left side of the rubiks cube in a string representation
    // - Elijah Gray
    public string get_left()
    {
        return GetSideString(left);
    }

    // getter provides the back side of the rubiks cube in a string representation
    // - Elijah Gray
    public string get_back()
    {
        return GetSideString(back);
    }



    // Elijah Gray 3/14/2023
    // This function takes in the input of a rubiks cube side and returns a string in the order accepted by the beginner solver.
    string GetMiddleFaceletsBeginner(List<GameObject> side)
    {

        string sideString = "";

        // visualizer data:
        // 0 1 2 
        // 3 4 5 
        // 6 7 8

        //beginner solver:
        // 6 7 0
        // 4 8 1
        // 4 3 2 

        // relative solution
        // 3 4 5
        // 2 9 6 
        // 1 8 7 

        sideString += side.ElementAt(6).name[0].ToString();
        sideString += side.ElementAt(3).name[0].ToString();
        sideString += side.ElementAt(0).name[0].ToString();
        sideString += side.ElementAt(1).name[0].ToString();
        sideString += side.ElementAt(2).name[0].ToString();
        sideString += side.ElementAt(5).name[0].ToString();
        sideString += side.ElementAt(8).name[0].ToString();
        sideString += side.ElementAt(7).name[0].ToString();
        sideString += side.ElementAt(4).name[0].ToString(); // middle element


        return sideString;
    }

    // obtains a string from the visualizer that represents the facelets in the order
    // they should be placed into the beginner solver. A further step is needed to swap the asci characters
    // accepted by the beginner solver.
    // Elijah Gray 3/14/2023
    public string BeginnerFaces()
    {
        string stateString = "";

        // the red, orange, blue, and green faces are stored the same way so code can be reused. The white and yellow faces however
        // are both stored in different orders for the beginner solver and will only be called once so I put that here.


        //stateString += GetMiddleFaceletsBeginner(down); // white
        // white face
        string white = "";

        // white face
        white += down.ElementAt(5).name[0].ToString();
        white += down.ElementAt(8).name[0].ToString();
        white += down.ElementAt(7).name[0].ToString();
        white += down.ElementAt(6).name[0].ToString();

        white += down.ElementAt(3).name[0].ToString();
        white += down.ElementAt(0).name[0].ToString();
        white += down.ElementAt(1).name[0].ToString();
        white += down.ElementAt(2).name[0].ToString();

        white += down.ElementAt(4).name[0].ToString();
        stateString += white;
        // white face

        //stateString += " ";
        stateString += GetMiddleFaceletsBeginner(back); // red
        //stateString += " ";
        stateString += GetMiddleFaceletsBeginner(front); // orange
        //stateString += " ";
        stateString += GetMiddleFaceletsBeginner(right); // blue
        //stateString += " ";
        stateString += GetMiddleFaceletsBeginner(left); // green
        //stateString += " ";



        //stateString += GetMiddleFaceletsBeginner(up); // yellow
        // yellow face
        string yellow = "";

        yellow += up.ElementAt(5).name[0].ToString();
        yellow += up.ElementAt(2).name[0].ToString();
        yellow += up.ElementAt(1).name[0].ToString();
        yellow += up.ElementAt(0).name[0].ToString();

        yellow += up.ElementAt(3).name[0].ToString();
        yellow += up.ElementAt(6).name[0].ToString();
        yellow += up.ElementAt(7).name[0].ToString();
        yellow += up.ElementAt(8).name[0].ToString();


        yellow += up.ElementAt(4).name[0].ToString();
        stateString += yellow;
        // yellow face

        return stateString;
    }



    // given a side of the rubiks cube, it provides a string representing the order of the facelets expected by the beginner method for the left, right, front, and back faces
    // - Elijah Gray
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


    // given a side of the rubiks cube, it provides a string representing the order of the facelets expected by the beginner method for the up and down face.
    // - Elijah Gray
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


    // takes the rubiks cube and returns a string representing the entire rubiks cube for the beginner method.
    // the faceslets are in the order expected per face as is the order of the faces themselves for the string representation.
    // - Elijah Gray
    public string Clockwisefaces()
    {
        string stateString = "";
        stateString += GetVerticalSideClockwise(down); // white
        stateString += GetSideClockwise(right); // blue
        Debug.Log("white & blue: " + stateString);


        stateString += GetSideClockwise(back); // red
        stateString += GetSideClockwise(left); // green 
        stateString += GetSideClockwise(front); // orange
        stateString += GetVerticalSideClockwise(up); // yellow

        Debug.Log("translation size: " + stateString.Length);
        return stateString;
    }





}


