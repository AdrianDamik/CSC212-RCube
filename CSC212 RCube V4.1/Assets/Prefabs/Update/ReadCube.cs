// Copyright 2023
// Adrian Damik, Elijah Gray, & Aryan Pothanaboyina

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadCube : MonoBehaviour
{
    //Rays
    public Transform tUp;
    public Transform tDown;
    public Transform tLeft;
    public Transform tRight;
    public Transform tBack;
    public Transform tFront;

    private List<GameObject> frontRays = new List<GameObject>();
    private List<GameObject> backRays = new List<GameObject>();
    private List<GameObject> upRays = new List<GameObject>();
    private List<GameObject> downRays = new List<GameObject>();
    private List<GameObject> leftRays = new List<GameObject>();
    private List<GameObject> rightRays = new List<GameObject>();

    private int layerMask = 1 << 8; //LayerMask 
    CubeState cubeState;
    CubeMap cubeMap;

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

    //slightly modified by Elijah. 
    // schedules a read once the cube has stopped moving rather than immediately on the frame its called.
    // - Elijah Gray
    public void ReadState()
    {

        if (cubeState == null)
        {
            Debug.Log("cube state is null");
        }
        StartCoroutine(WaitForAutoRotation());
    }

    // part of the original creators work, I believe this part is to setup the varous raycasts that will be used to examine the rubiks cubes's faces
    // - Elijah Gray
    void SetRayTransforms()
    {
        upRays = BuildRays(tUp, new Vector3(90, 90, 0));
        downRays = BuildRays(tDown, new Vector3(270, 90, 0));
        leftRays = BuildRays(tLeft, new Vector3(0, 180, 0));
        rightRays = BuildRays(tRight, new Vector3(0, 0, 0));
        frontRays = BuildRays(tFront, new Vector3(0, 90, 0));
        backRays = BuildRays(tBack, new Vector3(0, 270, 0));
    }

    // Part of the original creators work, I beileve this part given a raytransform and a diirection will shoot 9 raycasts in a direction
    // in order to read the 9 facelets of a rubiks cube.
    // - Elijah Gray
    List<GameObject> BuildRays(Transform rayTransform, Vector3 direction)
    {
        int rayCount = 0;
        List<GameObject> rays = new List<GameObject>();

        for(int y = 1; y > -2; y--)
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

    // Part of the original creators work, I Beileve this is how the program reads the face given the raystarts and raytransform
    // it then will take the hits and return the faces hit.
    // - Elijah Gray
    public List<GameObject> ReadFace(List<GameObject> rayStarts, Transform rayTransform)
    {
        List<GameObject> facesHit = new List<GameObject>();

        foreach(GameObject rayStart in rayStarts)
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

    // a modification by me that instead of immediately reading in the rubiks cube's data, it will wait until the rubiks cube has stopped moving.
    // Every frame the rubiks cube is moving checked by CheckForMovement(), the IEnumerator class will return a yield making it stop for a frame.
    // Once this is done the rubiks cube will be read. I did this to prevent cases where the the rubiks cube would be read mid-movement.
    // - Elijah Gray
    IEnumerator WaitForAutoRotation()
    {
        //manager.is_reading = true;

        while (Automate.CheckForMovement())
        {
            Debug.Log("Waiting for movement to stop!");
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("now executing read!");

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
