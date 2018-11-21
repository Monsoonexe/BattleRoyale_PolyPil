using UnityEngine;
using System.Collections;

public class BRS_PlaneDropManager : MonoBehaviour

{
    //public GameObject EndPointBall;
    [Header("Map Settings")]
    public Transform planeSpawnBounds;//where can the plane start and stop?
    public GameObject[] acceptableDropZones;//how big should the zone be that the plane flies through
    [Header("Plane Settings")]
    public GameObject BRS_PlaneSpawn;//plane object (model) to spawn
    public GameObject endpointMarker;//marks beginnning and end points for debugging purposes

    public int failedPathAltitudeIncrementAmount = 50;//if the flight path fails, raise the altitude by this much before trying again
    public bool DEBUG = true;//if true, prints debug statements

    private bool verifiedPath = false;

    //how high does the plane fly?
    private float planeFlightAltitude = 800.0f;

    //radius of spawn zone
    private float spawnBoundsCircleRadius = 100.0f;

    //start and end points for plane to fly through
    private Vector3 planeStartPoint;
    private Vector3 planeEndPoint;

    private int unsuccessfulPasses = 0;
    private readonly int flightPathChecksUntilFailure = 12;

    void Start()
    {
        //error checking
        if (planeSpawnBounds == null)
        {
            Debug.LogError("ERROR: plane spanw bounds not set!");
            Debug.Break();
        }

        if (acceptableDropZones.Length < 1)
        {
            Debug.LogError("ERROR: No Acceptable Drop Zones in list!");
            Debug.Break();
        }

        //set and check altitude
        planeFlightAltitude = planeSpawnBounds.position.y > 0 ? planeSpawnBounds.position.y : 200f;//verifies that altitude is above 0

        //set radius of spawnBoundsCircleRadius
        //leave at default value if local scale is too small
        spawnBoundsCircleRadius = planeSpawnBounds.localScale.x / 2 > spawnBoundsCircleRadius ? planeSpawnBounds.localScale.x / 2 : spawnBoundsCircleRadius;

        if (DEBUG)
        {
            //make sure endpoint prefab is visible
            endpointMarker.GetComponent<MeshRenderer>().enabled = true;
        }

        //set possible start and end points
        SetupFlightPath();
    }

    private Vector3 GetRandomPointOnCircle()
    {
        //get terminal point on unit circle, then multiply by radius
        float randomArc = Random.Range(0, 2 * Mathf.PI);
        Vector3 randomPoint = new Vector3(//create new vector3
            (Mathf.Sin(randomArc) * spawnBoundsCircleRadius) + planeSpawnBounds.position.x, //get x coordiantes on unit circle, multiply by radius, offset relative to bounds
            planeFlightAltitude, // set the height
            (Mathf.Cos(randomArc) * spawnBoundsCircleRadius) + planeSpawnBounds.position.z);//get y coordinate on unity circle, multiply by radius, offset relative to bounds
        //Debug.Log("random point: " + randomPoint);
        return randomPoint;

    }

    private void SetupFlightPath()
    {
        bool endpointHit = false;
        bool flightPathThroughLZ = false;

        //find a start point
        planeStartPoint = GetRandomPointOnCircle();
        //spawn debugger object. this object is the parent, so both will be destroyed
        if (DEBUG)
        {
            GameObject startMark = Instantiate(endpointMarker, planeStartPoint, Quaternion.identity, this.transform);
            startMark.name = "StartMarker: " + unsuccessfulPasses;

        }

        //look for an endpoint
        for (int endPointsFound = 1; endPointsFound <= flightPathChecksUntilFailure; ++endPointsFound)//while you don't have a valid flight path...
        {
            //Debug.Log("Attempt No: " + endPointsFound);
            //get end point on circle
            planeEndPoint = GetRandomPointOnCircle();
            //create a new endpoint marker at that location
            endpointMarker = Instantiate(endpointMarker, planeEndPoint, Quaternion.identity, this.transform);
            if (DEBUG) endpointMarker.name = "Endpoint Marker " + unsuccessfulPasses + "." + endPointsFound;

            ToggleDropZones(true);//turn LZ on
            //test if flight path goes through LZ
            if (TestRaycastThroughDropZone(planeStartPoint, endpointMarker.transform.position))
            {
                flightPathThroughLZ = true;
            }
            else
            {
                if (DEBUG) Debug.Log("Flight Path INVALID: Flight path not through LZ.");
            }

            //turn drop zones off so they don't interfere with raycast
            ToggleDropZones(false);//turn LZ off

            //test if flight path is clear all the way to endpoint
            if (TestRaycastHitEndPoint(planeStartPoint, endpointMarker))
            {
                endpointHit = true;
            }
            else
            {
                if (DEBUG) Debug.Log("Flight Path INVALID: Endpoint Marker Not Hit.");
            }

            //does the flight unobstructed and through a drop zone
            if(endpointHit && flightPathThroughLZ)
            {
                ToggleDropZones(true);//turn LZ on
                SpawnPlane();
                verifiedPath = true;
                break;
            }
            else
            {
                endpointHit = false;
                flightPathThroughLZ = false;
                verifiedPath = false;
            }

            
        }//end for

        if (!verifiedPath)
        {
            //this altitude is not working. keep raising
            if (++unsuccessfulPasses > flightPathChecksUntilFailure)//we've been here before
            {
                Debug.LogError("ERROR! Flight path failed after " + unsuccessfulPasses * flightPathChecksUntilFailure + " attempts. Adjust planeSpawnBounds. Skipping Plane Deployment");
                return;
            }
            //Debug.Log("Altitude too low. Raising Altitude and trying again.");
            //raise altitude and try again
            planeFlightAltitude += failedPathAltitudeIncrementAmount;
            //try again
            SetupFlightPath();
        }
        else
        {
            return;
        }
        
        
    }//end func

    private void SpawnPlane()
    {
        //create this plane in the world at this position, with no rotation
        GameObject plane = Instantiate(BRS_PlaneSpawn, planeStartPoint, Quaternion.identity);
        plane.transform.LookAt(planeEndPoint);//point plane towards endpoint
        //seppuku! this object is no longer needed -- kill children as well
        if (DEBUG)
        {

            this.enabled = false;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private bool TestRaycastThroughDropZone(Vector3 startPoint, Vector3 targetObject)
    {
        //did the raycast go through a drop zone?
        bool raycastThroughDropZone = false;
        //RaycastHit will store information about anything hit by the raycast
        RaycastHit raycastHitInfo;
        //raycast
        if (Physics.Raycast(startPoint, targetObject - startPoint, out raycastHitInfo, spawnBoundsCircleRadius * 2))
        {
            if (DEBUG) Debug.Log("Object Hit: " + raycastHitInfo.collider.gameObject.name);
            for (int i = 0; i < acceptableDropZones.Length; ++i)//look through each drop zone in list
            {
                if (raycastHitInfo.collider.gameObject == acceptableDropZones[i])//if the game object that was hit is inside this list of good zones
                {
                    if(DEBUG) Debug.Log("Landing Zone: " + raycastHitInfo.collider.gameObject.name);
                    raycastThroughDropZone = true;//booyah!
                    //if (DEBUG) Instantiate(debugEndpointMarker, raycastHitInfo.point, Quaternion.identity, this.transform);
                    break;//break out of for loop looking through gameObjects in list
                }//end if
            }//end for

        }//end if
        else
        {
            Debug.LogError("ERROR! Raycast missed target LZ");
        }

        return raycastThroughDropZone;
    }

    private bool TestRaycastHitEndPoint(Vector3 startPoint, GameObject targetObject)
    {
        //did the raycast hit the endpoint unobstructed by terrain or obstacles?
        bool raycastHitEndpoint = false;
        //RaycastHit holds info about raycast
        RaycastHit raycastHitInfo;
        //if something was hit...
        if(Physics.Raycast(startPoint, targetObject.transform.position - startPoint, out raycastHitInfo, spawnBoundsCircleRadius * 2))
        {
            if (DEBUG) Debug.Log("Object Hit: " + raycastHitInfo.collider.gameObject.name);
            if (raycastHitInfo.collider.gameObject == targetObject)//were we trying to hit this thing?
            {
                raycastHitEndpoint = true;//booyah!
                if (DEBUG) Debug.Log("Endpoint Marker Hit: " + raycastHitInfo.collider.gameObject.name);
            }
        }
        else
        {
            Debug.LogError("ERROR! Raycast missed it's target: " + targetObject);
        }
        return raycastHitEndpoint;
    }

    public void ToggleDropZones(bool enabled)
    {
        //look at each dropZone in our list
        foreach(GameObject dropZone in acceptableDropZones)
        {
            //set it active or inactive
            dropZone.SetActive(enabled);
        }
        if (DEBUG) Debug.Log("All Drop Zones Active: " + enabled);
    }
}