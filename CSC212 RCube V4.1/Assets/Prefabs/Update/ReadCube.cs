// Created by Megalomatt (https://github.com/Megalomatt/unity-rcube) (2020)
// Revised by hamzazmah (https://github.com/hamzazmah/RubiksCubeUnity-Tutorial) (2020)
// Revised by Elijah Gray (2023)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// lightly modified. Manages the rays that look at the cube object to read its state. -Elijah Gray
/// </summary>
public class ReadCube : MonoBehaviour
{
    //Rays
    /// <summary>
    ///  transform object for the up raycasts
    /// </summary>
    public Transform tUp;

    /// <summary>
    ///  transform object for the down raycasts
    /// </summary>
    public Transform tDown;

    /// <summary>
    ///  transform object for the left raycasts
    /// </summary>
    public Transform tLeft;

    /// <summary>
    ///  transform object for the right raycasts
    /// </summary>
    public Transform tRight;

    /// <summary>
    ///  transform object for the back raycasts
    /// </summary>
    public Transform tBack;

    /// <summary>
    ///  transform object for the front raycasts
    /// </summary>
    public Transform tFront;


    /// <summary>
    /// list of the 9 raycasts for the front of the cube in the scene.
    /// </summary>
    private List<GameObject> frontRays = new List<GameObject>();

    /// <summary>
    /// list of the 9 raycasts for the back of the cube in the scene.
    /// </summary>
    private List<GameObject> backRays = new List<GameObject>();

    /// <summary>
    /// list of the 9 raycasts for the top of the cube in the scene.
    /// </summary>
    private List<GameObject> upRays = new List<GameObject>();

    /// <summary>
    /// list of the 9 raycasts for the bottom of the cube in the scene.
    /// </summary>
    private List<GameObject> downRays = new List<GameObject>();

    /// <summary>
    /// list of the 9 raycasts for the left of the cube in the scene.
    /// </summary>
    private List<GameObject> leftRays = new List<GameObject>();

    /// <summary>
    /// list of the 9 raycasts for the right of the cube in the scene.
    /// </summary>
    private List<GameObject> rightRays = new List<GameObject>();

    /// <summary>
    /// used to identify the facelets.
    /// </summary>
    private int layerMask = 1 << 8; //LayerMask 
    
    /// <summary>
    /// variable to get the cubestate functionality.
    /// </summary>
    CubeState cubeState;

    /// <summary>
    /// map of the cubestate
    /// </summary>
    CubeMap cubeMap;

    /// <summary>
    /// used to to initiliaze the raycasts
    /// </summary>
    public GameObject emptyGO;

    // Start is called before the first frame update
    //initializes the rubiks cube's state
    //- Elijah Gray
    void Start()
    {
        SetRayTransforms();

        cubeState = FindObjectOfType<CubeState>();
        cubeMap = FindObjectOfType<CubeMap>();

        cubeState.up = ReadFace(upRays, tUp);
        cubeState.down = ReadFace(downRays, tDown);
        cubeState.left = ReadFace(leftRays, tLeft);
        cubeState.right = ReadFace(rightRays, tRight);
        cubeState.front = ReadFace(frontRays, tFront);
        cubeState.back = ReadFace(backRays, tBack);

        cubeMap.Set();

        CubeState.started = true;
    }

    // Update is called once per frame
    // function no longer updates every tick
    void Update()
    {
        //ReadState();
    }

    /// <summary>
    /// slightly modified. Reads in the state of the cube, but now only when the cube has stopped moving by calling another function. This function is a wrapper and could be simplified. -Elijah Gray
    /// </summary>
    public void ReadState()
    {

        if (cubeState == null)
        {
            Debug.Log("cube state is null");
        }
        StartCoroutine(WaitForAutoRotation());
    }
    /// <summary>
    /// unmodified used to read in the cube using all 54 raycasts.
    /// </summary>
    void SetRayTransforms()
    {
        upRays = BuildRays(tUp, new Vector3(90, 90, 0));
        downRays = BuildRays(tDown, new Vector3(270, 90, 0));
        leftRays = BuildRays(tLeft, new Vector3(0, 180, 0));
        rightRays = BuildRays(tRight, new Vector3(0, 0, 0));
        frontRays = BuildRays(tFront, new Vector3(0, 90, 0));
        backRays = BuildRays(tBack, new Vector3(0, 270, 0));
    }

    /// <summary>
    ///  unmodified, part of the original creator's work.  I beileve this part given a raytransform and a diirection will shoot 9 raycasts in a direction
    /// in order to read the 9 facelets of a rubiks cube.
    /// </summary>
    /// <param name="rayTransform"> the ray transform object such as the left side's rays </param>
    /// <param name="direction"> the direction in which we build arrays toward </param>
    /// <returns></returns>
    List<GameObject> BuildRays(Transform rayTransform, Vector3 direction)
    {
        int rayCount = 0;
        List<GameObject> rays = new List<GameObject>();

        for (int y = 1; y > -2; y--)
        {
            for (int x = -1; x < 2; x++)
            {
                Vector3 startPos = new Vector3(rayTransform.localPosition.x + x, rayTransform.localPosition.y + y, rayTransform.localPosition.z);
                GameObject rayStart = Instantiate(emptyGO, startPos, Quaternion.identity, rayTransform);
                rayStart.name = rayCount.ToString();
                rays.Add(rayStart);
                rayCount++;
            }
        }
        rayTransform.localRotation = Quaternion.Euler(direction);
        return rays;
    }


    /// <summary>
    /// unmodified, part of the original creator's work. I Beileve this is how the program reads the face given the raystarts and raytransform
    /// it then will take the hits and return the faces hit.
    /// </summary>
    /// <param name="rayStarts"> a list of the origins of raycasts </param>
    /// <param name="rayTransform"> the transform of the raycasts </param>
    /// <returns></returns>
    public List<GameObject> ReadFace(List<GameObject> rayStarts, Transform rayTransform)
    {
        List<GameObject> facesHit = new List<GameObject>();

        foreach (GameObject rayStart in rayStarts)
        {
            Vector3 ray = rayStart.transform.position;
            RaycastHit hit;

            //Ray cast Intersects with Objects
            if (Physics.Raycast(ray, rayTransform.forward, out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(ray, rayTransform.forward * hit.distance, Color.yellow);
                facesHit.Add(hit.collider.gameObject);
                //Debug.Log(hit.collider.gameObject.name);
            }
            else
            {
                Debug.DrawRay(ray, rayTransform.forward * 1000, Color.green);
            }
        }

        return facesHit;

    }

    /// <summary>
    /// a modification by me that instead of immediately reading in the rubiks cube's data, it will wait until the rubiks cube has stopped moving.
    /// Every frame the rubiks cube is moving checked by CheckForMovement(), the IEnumerator class will return a yield making it stop for a frame.
    /// Once this is done the rubiks cube will be read. I did this to prevent cases where the the rubiks cube would be read mid-movement.
    /// - Elijah Gray
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForAutoRotation()
    {
        //manager.is_reading = true;

        while (Automate.CheckForMovement())
        {
            //Debug.Log("Waiting for movement to stop!");
            yield return new WaitForEndOfFrame();
        }

        //Debug.Log("now executing read!");

        cubeState = FindObjectOfType<CubeState>();
        cubeMap = FindObjectOfType<CubeMap>();

        cubeState.up = ReadFace(upRays, tUp);
        cubeState.down = ReadFace(downRays, tDown);
        cubeState.left = ReadFace(leftRays, tLeft);
        cubeState.right = ReadFace(rightRays, tRight);
        cubeState.front = ReadFace(frontRays, tFront);
        cubeState.back = ReadFace(backRays, tBack);

        cubeMap.Set();
    }





}
