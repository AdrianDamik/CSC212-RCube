// Copyright 2023
// Adrian Damik, Elijah Gray, & Aryan Pothanaboyina

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    //Attributes
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;
    Vector3 previousMousePosition;
    Vector3 mouseDelta;
    public GameObject target;
    float speed = 200f;

    int frames_to_wait = 5;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // prevents the user from moving the cube if the cube's move point is unitialized.
        // - Elijah Gray
        if (target == null)
        {
            Debug.Log("target was null, not grabbing!");
            return;
        }

        // if the cube is moving or autorotating, the program will not allow the user to move the cube to prevent it from breaking.
        // - Elijah Gray
        if ( CubeState.autoRotating || Automate.CheckForMovement() )
        {
            frames_to_wait = 20;
            return;
        } else if (frames_to_wait > 0)
        {
            --frames_to_wait;
            return;
        }

       
         Swipe();
         Drag();
       


    }

    //unmodified code from the original creator, used to shift the cube around to allow the user to view it from different perspectives.
    // - Elijah Gray
    void Drag()
    {
        if(Input.GetMouseButton(1))
        {
            //While the Mouse is Held Down the Cube can be Moved around its central Axis
            mouseDelta = Input.mousePosition - previousMousePosition;
            mouseDelta *= 0.1f;
            transform.rotation = Quaternion.Euler(mouseDelta.y, -mouseDelta.x, 0) * transform.rotation;
        }
        else
        {

            //Debug.Log("this part of the code runs every frame 1");

            //Automatically Move to the Target position
            //transform.rotation = find
            if (transform.rotation != target.transform.rotation)
            {
                Debug.Log("this part of the code runs every frame 2");

                var step = speed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target.transform.rotation, step);
            }
        }
        previousMousePosition = Input.mousePosition;
    }

    //unmodified code from the original creator, used to shift the cube around to allow the user to view it from different perspectives.
    // - Elijah Gray
    void Swipe()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //Get the 2D Position of the first mouse Click
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Debug.Log("first click");
        }
        if(Input.GetMouseButtonUp(1))
        {
            //Get the 2D Position of the first mouse Click
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Debug.Log("second click");

            //Create a Vector from the First and the Second Click
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
            //Normalize the 2D Vector
            currentSwipe.Normalize();
            //Swipe Depending on the Input
            if(LeftSwipe(currentSwipe))
            {
                target.transform.Rotate(0, 90, 0, Space.World);
            }
            else if (RightSwipe(currentSwipe))
            {
                target.transform.Rotate(0, -90, 0, Space.World);
            }
            else if (UpRightSwipe(currentSwipe))
            {
                target.transform.Rotate(0, 0, -90, Space.World);
            }
            else if (UpLeftSwipe(currentSwipe))
            {
                target.transform.Rotate(90, 0, 0, Space.World);
            }
            else if (DownRightSwipe(currentSwipe))
            {
                target.transform.Rotate(-90, 0, 0, Space.World);
            }
            else if (DownLeftSwipe(currentSwipe))
            {
                target.transform.Rotate(0, 0, 90, Space.World);
            }

        }
    }

    // helper function given a vector2 to determine how the cube to move.
    // -Elijah Gray
    bool LeftSwipe(Vector2 swipe)
    {
        return currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f;
    }

    // helper function given a vector2 to determine how the cube to move.
    // -Elijah Gray
    bool RightSwipe(Vector2 swipe)
    {
        return currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f;
    }

    // helper function given a vector2 to determine how the cube to move.
    // -Elijah Gray
    bool UpLeftSwipe(Vector2 swipe)
    {
        return currentSwipe.y > 0 && currentSwipe.x < 0f;
    }

    // helper function given a vector2 to determine how the cube to move.
    // -Elijah Gray
    bool UpRightSwipe(Vector2 swipe)
    {
        return currentSwipe.y > 0 && currentSwipe.x > 0f;
    }

    // helper function given a vector2 to determine how the cube to move.
    // -Elijah Gray
    bool DownLeftSwipe(Vector2 swipe)
    {
        return currentSwipe.y < 0 && currentSwipe.x < 0f;
    }

    // helper function given a vector2 to determine how the cube to move.
    // -Elijah Gray
    bool DownRightSwipe(Vector2 swipe)
    {
        return currentSwipe.y < 0 && currentSwipe.x > 0f;
    }
}
