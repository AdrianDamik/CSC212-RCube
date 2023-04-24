// Created by Megalomatt (https://github.com/Megalomatt/unity-rcube) (2020)
// Revised by hamzazmah (https://github.com/hamzazmah/RubiksCubeUnity-Tutorial) (2020)
// Revised by Elijah Gray (2023)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Heavily modified. A pivot for the rotations of the cube. Manages the rotation animations. -Elijah Gray
/// </summary>
public class PivotRotation : MonoBehaviour
{

    /// <summary>
    /// The side of the cube that's currently active.
    /// </summary>
    List<GameObject> activeSide;

    /// <summary>
    ///  part of the calculation for moving the pivot.
    /// </summary>
    public Vector3 localForward;

    /// <summary>
    /// the speed of the mouse cursor.
    /// </summary>
    private Vector3 mouseRef;

    /// <summary>
    /// whether the pivot is being moved manually.
    /// </summary>
    bool dragging = false;

    /// <summary>
    /// a multiplier for the mouse movement.
    /// </summary>
    private float senstivity = 0.4f;

    /// <summary>
    /// the vector of the pivot.
    /// </summary>
    public Vector3 rotation;

    /// <summary>
    /// a boooldean to determine whether the pivot is currently moving.
    /// </summary>
    public bool autoRotating = false;

    /// <summary>
    /// The speed variable of how fast the cube should be rotating.
    /// </summary>
    private float speed = 300; //float.MaxValue;
    //private float speed = 30000000;
    //private float speed = 300;

    /// <summary>
    /// The target of the quartenion for a rotation.
    /// </summary>
    public Quaternion targetQuaternion;

    /// <summary>
    ///  Used to grab the functionality of the readcube object.
    /// </summary>
    private ReadCube readCube;

    /// <summary>
    ///  Used to grab the functionality of the cubestate object.
    /// </summary>
    private CubeState cubeState;

    /// <summary>
    /// An array of other pivot objects in the game scene.
    /// </summary>
    PivotRotation[] other_pivots;

    // creates a reference array of all OTHER pivot objects in the game scene.
    // -Elijah Gray


    /// <summary>
    ///  Used mostly for debugging. Used to initialize the list of other pivot objects in the game scene. - Elijah Gray
    /// </summary>
    void initialize_other_pivot_list()
    {
        PivotRotation[] all_pivots = GetComponentsInChildren<PivotRotation>();
        other_pivots = new PivotRotation[all_pivots.Length - 1];

        int index = 0;

        for (int i = 0; i < all_pivots.Length; i++)
        {
            if (all_pivots[i] != this)
            {
                other_pivots[index] = all_pivots[i];
                ++index;
            }

        }
    }


    /// <summary>
    /// function to determine if any other pivots in the game scene are currently active/auto-rotating.
    /// returns true if any of the other pivots autorotating boolean is true.
    /// returns false otherwise. 
    /// </summary>
    /// <returns> returns a boolean, true if there are any other active pivots whose automate boolean is true, returns false otherwise </returns>
    bool check_other_active_pivot()
    {
        for (int i = 0; i < other_pivots.Length; i++)
        {
            if (other_pivots[i].autoRotating)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Called at the start of te program. Initializes this part of the program.
    /// </summary>

    void Start()
    {
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
        initialize_other_pivot_list();
    }

    // original program ran each and every pivotrotation every frame
    // I changed it so they should only updated when needed, greatly
    // improving performance.
    // - Elijah Gray

    /*
    // Late Update is called once per frame at the End
    void LateUpdate()
    {

       
    }
    */

    // spins part of the rubiks cube given a side of the rubiks cube as input.
    // utilized for the user's spin function. Both manual user inputs are by the original creator
    // - Elijah Gray


    /// <summary>
    /// spins part of the rubiks cube given a side of the rubiks cube as input.
    /// utilized for the user's spin function. Both manual user inputs are by the original creator
    /// - Elijah Gray
    /// </summary>
    /// <param name="side"> the side of the cube that should be rotated </param>
    private void SpinSide(List<GameObject> side)
    {
        rotation = Vector3.zero;
        Vector3 mouseOffset = (Input.mousePosition - mouseRef);

        if (side == cubeState.front)
        {
            rotation.x = (mouseOffset.x + mouseOffset.y) * senstivity * -1;
        }
        if (side == cubeState.back)
        {
            rotation.x = (mouseOffset.x + mouseOffset.y) * senstivity * 1;

        }
        if (side == cubeState.up)
        {
            rotation.y = (mouseOffset.x + mouseOffset.y) * senstivity * 1;
        }
        if (side == cubeState.down)
        {
            rotation.y = (mouseOffset.x + mouseOffset.y) * senstivity * -1;
        }
        if (side == cubeState.left)
        {
            rotation.z = (mouseOffset.x + mouseOffset.y) * senstivity * 1;
        }
        if (side == cubeState.right)
        {
            rotation.z = (mouseOffset.x + mouseOffset.y) * senstivity * -1;
        }

        transform.Rotate(rotation, Space.Self);
        mouseRef = Input.mousePosition;
    }

    // given a side of the rubiks cube, it will calculate part of the rotation needed.
    // created by original developer.
    // - Elijah Gray


    /*
        public void Rotate(List<GameObject> side)
        {
            activeSide = side;
            mouseRef = Input.mousePosition;
            dragging = true;
            //Create a Vector to rotate around
            localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;
            //Debug.Log(localForward.magnitude);

        }
    */

    // iniiates the process to rotate the rubiks cube.
    // this function will first pickup part of the rubiks cube to be the part that will be rotated.
    // the side that will be rotated is the first input. The second input angle will be the angle
    // desired by the user.
    // I modified this function to utilize by coroutine method, everything else is from the orignal creator.
    // - Elijah Gray


    /// <summary>
    /// iniiates the process to rotate the rubiks cube.
    /// this function will first pickup part of the rubiks cube to be the part that will be rotated.
    /// the side that will be rotated is the first input. The second input angle will be the angle
    /// desired by the user.
    /// I modified this function to utilize by coroutine method, everything else is from the orignal creator.
    /// - Elijah Gray
    /// </summary>
    /// <param name="side"> the side of the cube that should be rotated </param>
    /// <param name="angle"> the angle in which the cube should be rotated IE -90, 180, 90 </param>
    public void StartAutoRotate(List<GameObject> side, float angle)
    {
        Debug.Log("StartAutoRotate() called");
        cubeState.PickUp(side);
        Vector3 localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;
        targetQuaternion = Quaternion.AngleAxis(angle, localForward) * transform.localRotation;
        activeSide = side;
        autoRotating = true;
        StartCoroutine(Rotation_Animation());
        //Rotation_Animation();
    }

    // calculates a right angle rotation for the rubiks cube pivot to utilize given that the cube is at a point
    // where it has yet to complete a right angle move. Created by original developer..
    // I am not very confident in quaternions as I haven't learned them yet though but I believe this is how it works.
    // -Elijah Gray


    /// <summary>
    /// Unmodified function.  
    /// calculates a right angle rotation for the rubiks cube pivot to utilize given that the cube is at a point
    /// where it has yet to complete a right angle move. Created by original developer..
    /// I am not very confident in quaternions as I haven't learned them yet though but I believe this is how it works.
    /// -Elijah Gray
    /// </summary>
    public void RotateToRightAngle()
    {
        Vector3 vec = transform.localEulerAngles;

        vec.x = Mathf.Round(vec.x / 90) * 90;
        vec.y = Mathf.Round(vec.y / 90) * 90;
        vec.z = Mathf.Round(vec.z / 90) * 90;

        targetQuaternion.eulerAngles = vec;
        autoRotating = true;
    }

    /// <summary>
    /// Part of the new couroutine based rotation system I implemented.
    /// When called the program will return IEnumerators causing this part of the program to effectively
    /// suspend itself for a frame.
    /// this system when initiated will calculate the rotation requested, calcuate how much time is needed
    /// it will then create a float to reference how much time has passed.
    /// the program will then enter a loop and rotate the cube partially then pass a frame
    /// to smoothe out the animation over as many frames as needed.
    /// once the rotation is complete, it will let one frame pass to act as a buffer.
    /// it will then call the putdown function to place the rubiks cube cubies back onto the main cube
    /// object.
    /// it will then pass another frame to ensure the cubies have been put down first.
    /// once the cubies are down and a frame has passed, the program will read in the cube using
    /// the read state function to update the cube.
    /// once this process is done another frame will pass and then variables pertaining to the rotation being complete will be pushed.
    /// - Elijah Gray
    /// </summary>
    /// <returns> returns an IEnumerator to skip a frame while the function is running </returns>
    private IEnumerator Rotation_Animation()
    {
       
        /*
        Debug.Log("Animation played");
        if (check_other_active_pivot())
        {
            Debug.Log("Attempting to move while another pivot is active!");
            //yield break;
        }
        */

        // an anonymous individual on a forum advised me on this part as the best way to approach what I wanted to solve.
        // - Elijah Gray
        // v v v v 
        Quaternion startRotation = transform.localRotation;

        float rotationTime = Quaternion.Angle(startRotation, targetQuaternion) / speed;
        float elapsedTime = 0f;
        float t = 0f;

        while (elapsedTime < rotationTime)
        {
            elapsedTime += Time.deltaTime;
            t = Mathf.Clamp01(elapsedTime / rotationTime);
            transform.localRotation = Quaternion.Lerp(startRotation, targetQuaternion, t);
            yield return null;
        }
        // ^ ^ ^ ^ 

        yield return null;
        cubeState.PutDown(activeSide, transform.parent);
        yield return null;
        readCube.ReadState();
        yield return null;
        CubeState.autoRotating = false;
        autoRotating = false;
        dragging = false;
        this.rotation = Vector3.zero;
        yield return null;

    }

    /// <summary>
    /// part of my new system to move the rubiks cube. This initiates a process based on the original way the rubiks cube moved
    /// using user mouse input. The side variable is the side of the rubiks cube that should be dragged and rotated based on mouse input.
    /// - Elijah Gray
    /// </summary>
    /// <param name="side"> the side of the cube that should be rotated </param>
    public void Start_Dragging(List<GameObject> side)
    {

        activeSide = side;
        mouseRef = Input.mousePosition;
        dragging = true;
        autoRotating = true;
        localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;
        StartCoroutine(dragging_Cube(side));
    }

    /// <summary>
    /// this part of the program returns an IEnumerator every frame update
    /// the side inputted is the side of the cube that should be moved around using the user's mouse.
    /// this program will continue to update the cube's position every frame for as long as the user is holding down mouse1.
    /// upon releasing mouse1, the program will begin a similar animation process like playanimation() and continue to move the rubiks cube, update the frame
    /// and repeat this process until the rubiks cube has moved to a 90 degree angle.
    /// - Elijah Gray
    /// </summary>
    /// <param name="side"> the side of the cube that should be rotated </param>
    /// <returns></returns>
    IEnumerator dragging_Cube(List<GameObject> side)
    {
        Debug.Log("dragging cube!");
        Debug.Log(side);

        //cubeState.PickUp(side);

        while (Input.GetMouseButton(0))
        {

            mouseRef = Input.mousePosition;
            localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;
            //Rotate(side);
            yield return null;

            SpinSide(side);
            Debug.Log("frame skipped, holding M1");
            yield return null;
        }

        RotateToRightAngle();

        Quaternion startRotation = transform.localRotation;
        float rotationTime = Quaternion.Angle(startRotation, targetQuaternion) / speed;
        float elapsedTime = 0f;
        float t = 0f;

        while (elapsedTime < rotationTime)
        {
            elapsedTime += Time.deltaTime;
            t = Mathf.Clamp01(elapsedTime / rotationTime);
            transform.localRotation = Quaternion.Lerp(startRotation, targetQuaternion, t);
            yield return null;
        }

        yield return null;
        cubeState.PutDown(activeSide, transform.parent);
        yield return null;
        readCube.ReadState();
        yield return null;
        CubeState.autoRotating = false;
        autoRotating = false;
        dragging = false;
        this.rotation = Vector3.zero;
        yield return null;


    }




}



