// Created by Megalomatt (https://github.com/Megalomatt/unity-rcube) (2020)
// Revised by hamzazmah (https://github.com/hamzazmah/RubiksCubeUnity-Tutorial) (2020)
// Revised by Elijah Gray (2023)

using UnityEngine;

/// <summary>
///  Slightly modified class from the original creators. Used to allow the user to manually turn the cube.
/// </summary>
public class RotateCube : MonoBehaviour
{
    /// <summary>
    /// The first click of the mouse to determine a move.
    /// </summary>
    Vector2 firstPressPos;

    /// <summary>
    /// The second click of mouse1 to determine a move.
    /// </summary>
    Vector2 secondPressPos;

    /// <summary>
    /// the current vector for the mouse.
    /// </summary>
    Vector2 currentSwipe;

    /// <summary>
    /// the last vector of the mouse.
    /// </summary>
    Vector3 previousMousePosition;

    /// <summary>
    /// The speed in which the mouse is moving.
    /// </summary>
    Vector3 mouseDelta;

    /// <summary>
    /// The cube/target for movement.
    /// </summary>
    public GameObject target;

    /// <summary>
    /// The speed in which the cube should rotate.
    /// </summary>
    float speed = 200f;

    /// <summary>
    ///  How many frames the program should wait.
    /// </summary>
    int frames_to_wait = 5;

    // Start is called before the first frame update
    void Start()
    {

    }

  
    /// <summary>
    /// Mostliy unmodified by our group. Updates every frame in the program. Used to allow the user to 
    /// </summary>
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
        if (CubeState.autoRotating || Automate.CheckForMovement())
        {
            frames_to_wait = 20;
            return;
        }
        else if (frames_to_wait > 0)
        {
            --frames_to_wait;
            return;
        }


        Swipe();
        Drag();



    }


    /// <summary>
    ///  Unmodified code from the original creator, used to shift the cube around to allow the user to view it from different perspectives.
    /// </summary>
    void Drag()
    {
        if (Input.GetMouseButton(1))
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


    /// <summary>
    ///  Unmodified code from the original creator, used to shift the cube around to allow the user to view it from different perspectives.
    /// </summary>
    void Swipe()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //Get the 2D Position of the first mouse Click
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Debug.Log("first click");
        }
        if (Input.GetMouseButtonUp(1))
        {
            //Get the 2D Position of the first mouse Click
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Debug.Log("second click");

            //Create a Vector from the First and the Second Click
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
            //Normalize the 2D Vector
            currentSwipe.Normalize();
            //Swipe Depending on the Input
            if (LeftSwipe(currentSwipe))
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



    /// <summary>
    /// Umnodified function. Given a vector2 to determine how to move the cube. -Elijah Gray
    /// </summary>
    /// <param name="swipe"> the vector that should be applied for leftwards movement </param>
    /// <returns></returns>
    bool LeftSwipe(Vector2 swipe)
    {
        return currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f;
    }


    /// <summary>
    /// Umnodified function. Given a vector2 to determine how to move the cube. -Elijah Gray
    /// </summary>
    /// <param name="swipe"> the vector that should be applied for rightwards movement </param>
    /// <returns></returns>
    bool RightSwipe(Vector2 swipe)
    {
        return currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f;
    }

    /// <summary>
    /// Umnodified function. Given a vector2 to determine how to move the cube. -Elijah Gray
    /// </summary>
    /// <param name="swipe"> the vector that should be applied for upleftwards movement </param>
    /// <returns></returns>
    bool UpLeftSwipe(Vector2 swipe)
    {
        return currentSwipe.y > 0 && currentSwipe.x < 0f;
    }

    /// <summary>
    /// Umnodified function. Given a vector2 to determine how to move the cube. -Elijah Gray
    /// </summary>
    /// <param name="swipe"> the vector that should be applied for uprightwards movement </param>
    /// <returns></returns>
    bool UpRightSwipe(Vector2 swipe)
    {
        return currentSwipe.y > 0 && currentSwipe.x > 0f;
    }


    /// <summary>
    /// Umnodified function. Given a vector2 to determine how to move the cube. -Elijah Gray
    /// </summary>
    /// <param name="swipe"> the vector that should be applied for downleftwards movement </param>
    /// <returns></returns>
    bool DownLeftSwipe(Vector2 swipe)
    {
        return currentSwipe.y < 0 && currentSwipe.x < 0f;
    }


    /// <summary>
    /// Unodified function. Given a vector2 to determine how to move the cube. -Elijah Gray
    /// </summary>
    /// <param name="swipe"> the vector that should be applied for downrightwards movement </param>
    /// <returns></returns>
    bool DownRightSwipe(Vector2 swipe)
    {
        return currentSwipe.y < 0 && currentSwipe.x > 0f;
    }
}
