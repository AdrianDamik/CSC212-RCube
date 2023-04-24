// Created by Megalomatt (https://github.com/Megalomatt/unity-rcube) (2020)
// Revised by hamzazmah (https://github.com/hamzazmah/RubiksCubeUnity-Tutorial) (2020)
// Revised by Elijah Gray (2023)

using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Slightly modified from the original creator's work. Manages the part of the program that allows the user to pan the cube around.
/// </summary>
public class SelectFace : MonoBehaviour
{

    /// <summary>
    ///  used to grab the function to get the cubestate functionality.
    /// </summary>
    CubeState cubeState;

    /// <summary>
    ///  used to grab the function to get the readcube functionality.
    /// </summary>
    ReadCube readCube;

    /// <summary>
    ///  used to mask in the physics raycasts.
    /// </summary>
    int layerMask = 1 << 8;

    /// <summary>
    ///  how many frames the program should wait.
    /// </summary>
    int frames_to_wait = 5;

    /// <summary>
    ///  called at the beginning of the program startup to initialize the variables by finding the other objects in the unity scene.
    /// </summary>
    void Start()
    {
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
    }

    /// <summary>
    /// This part of the program updates every frame to allow the user to shift the cube's angle so they can view it from different perspectives.
    /// part of the original visualizer with some modifications.
    ///  - Elijah Gray
    /// </summary>
    void Update()
    {
        //modification put it in place to ensure the user cannot move the cube during the automated cube process.
        // - Elijah
        if (CubeState.autoRotating)
        {
            frames_to_wait = 20;
            return;
        }
        else if (frames_to_wait > 0)
        {
            --frames_to_wait;
            return;
        }

        if (Input.GetMouseButtonDown(0) && Automate.CheckForMovement())
        {
            Debug.Log("cube already has a pivot busy!");
        }

        if (Input.GetMouseButtonDown(0) && !Automate.CheckForMovement() && !CubeState.autoRotating)
        {
            //Read Current State of Cube
            //readCube.ReadState();
            //CubeState.autoRotating = true;

            //RayCast from the mouse towards the cube to see if a face is hit
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
            {
                GameObject face = hit.collider.gameObject;
                //Make a List of all the Sides 

                List<List<GameObject>> cubeSides = new List<List<GameObject>>()
                {
                    cubeState.up,
                    cubeState.down,
                    cubeState.left,
                    cubeState.right,
                    cubeState.front,
                    cubeState.back
                };

                foreach (List<GameObject> cubeSide in cubeSides)
                {
                    if (cubeSide.Contains(face))
                    {
                        cubeState.PickUp(cubeSide);
                        Debug.Log("cube side is: " + cubeSide[1]);
                        // calls my new animation method -Elijah Gray
                        cubeSide[4].transform.parent.GetComponent<PivotRotation>().Start_Dragging(cubeSide);
                    }
                }


            }
        }



    }

  

}