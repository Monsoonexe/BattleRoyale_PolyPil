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

    //how high does the plane fly?
    private float planeFlightAltitude = 800.0f;

    //radius of spawn zone
    private float spawnBoundsCircleRadius;

    //was there a path successfully created
    private bool verifiedPath = false;

    //start and end points for plane to fly through
    private Vector3 planeStartPoint;
    private Vector3 planeEndPoint;

    //initial path finder
    private int spawnAttempts = 0;// used to track failures. app should pause or fail after this many fails
    private readonly int spawnAttemptsUntilFailure = 10;//default of 15 tries

    //recursion for verifying path
    private int recursionAttempts = 0;//tracks recursion attempts
    private readonly int recursionAttemptsUntilFailure = 5;//give up after this many failed attempts

	void Start ()
	{
        //error checking
        if(planeSpawnBounds == null)
        {
            Debug.LogError("ERROR: plane spanw bounds not set!");
            Debug.Break();
        }

        if(acceptableDropZones.Length < 1)
        {
            Debug.LogError("ERROR: No Acceptable Drop Zones in list!");
            Debug.Break();
        }

        //set and check altitude
        planeFlightAltitude = planeSpawnBounds.position.y > 0 ? planeSpawnBounds.position.y : 200f;//verifies that altitude is above 0

        //set radius of spawnBoundsCircleRadius
        //TODO set default minimum value to protect from less than 0 values
        spawnBoundsCircleRadius = planeSpawnBounds.localScale.x / 2;

        if (DEBUG)
        {
            //make sure it is visible
            endpointMarker.GetComponent<MeshRenderer>().enabled = true;
        }

        //set possible start and end points
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
        //if(DEBUG) Debug.Log("Setting up Flight Path. Max Attempts: " + spawnAttemptsUntilFailure);
        RaycastHit raycastHitInfo;

        //find a start point
        planeStartPoint = GetRandomPointOnCircle();
        //spawn debugger object. this object is the parent, so both will be destroyed
        if(DEBUG)Instantiate(endpointMarker, planeStartPoint, Quaternion.identity, this.transform);


        //look for an endpoint
        for (spawnAttempts = 1; spawnAttempts <= spawnAttemptsUntilFailure; ++spawnAttempts)//while you don't have a valid flight path...
        {
            if (verifiedPath)
            {
                break;//break out of for loop setting up viable flight path
            }
            //Debug.Log("Attempt No: " + spawnAttempts);
            planeEndPoint = GetRandomPointOnCircle();
            endpointMarker = Instantiate(endpointMarker, planeEndPoint, Quaternion.identity, this.transform);


            //if (DEBUG) Instantiate(debugEndpointMarker, planeEndPoint, Quaternion.identity, this.transform);

            if (Physics.Raycast(planeStartPoint, planeEndPoint - planeStartPoint, out raycastHitInfo, spawnBoundsCircleRadius))
            {
                for (int i = 0; i < acceptableDropZones.Length; ++i)
                {
                    if (raycastHitInfo.collider.gameObject == acceptableDropZones[i])//if the game object that was hit is inside this list of good zones
                    {

                        //Debug.Log("Flight Path Confirmed after: " + spawnAttempts + " attempts. Flying through: " + raycastHitInfo.collider.gameObject.name);
                        //now we know that the possible path goes through a good LZ, however, is the path on the other side clear?
                        verifiedPath = RepeatingRayCast(verifiedPath, planeStartPoint, endpointMarker);
                        //if (DEBUG) Instantiate(debugEndpointMarker, raycastHitInfo.point, Quaternion.identity, this.transform);
                        break;//break out of for loop looking through gameObjects in list
                    }//end if
                }//end for
                //if(!verifiedPath) Debug.Log("MISSED! Instead hit: " + raycastHitInfo.collider.gameObject.name);

            }//end if
            //else
            //{
            //    if(DEBUG) Debug.Log("MISS!");
            //}
        }//end for

        if (!verifiedPath)
        {
            //Debug.Log("Altitude too low. Raising Altitude and trying again.");
            //raise altitude and try again
            planeFlightAltitude += failedPathAltitudeIncrementAmount;
            //try again
            SetupFlightPath();
            //Debug.Break();
        }

    }//end func

	private void LateUpdate()
	{

        if (verifiedPath)
        {
            SpawnPlane();
        }
        else
        {
            SetupFlightPath();
        }
	}

	private void SpawnPlane()
	{
        //create this plane in the world at this position, with no rotation
        GameObject plane = Instantiate(BRS_PlaneSpawn, planeStartPoint, Quaternion.identity);
        plane.transform.LookAt (planeEndPoint);//point plane towards endpoint
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

    private bool RepeatingRayCast(bool success, Vector3 startPoint, GameObject endPoint)
    {
        if (recursionAttempts++ > recursionAttemptsUntilFailure)
        {
            recursionAttempts = 0;
            return false;
        }
        RaycastHit raycastHitInfo;
        if (Physics.Raycast(startPoint, endPoint.transform.position - startPoint, out raycastHitInfo, spawnBoundsCircleRadius))
        {
            //if (DEBUG) Instantiate(debugEndpointMarker, raycastHitInfo.point, Quaternion.identity, this.transform);
            //invalid if raycast hits terrain
            if (raycastHitInfo.collider.CompareTag("Terrain")) return false;
            //valid if raycast hits endpoint
            if (raycastHitInfo.collider.gameObject == endPoint) return true;
            //try again if raycast hits dropzone
            for (int i = 0; i < acceptableDropZones.Length; ++i)
            {
                if (raycastHitInfo.collider.gameObject == acceptableDropZones[i])//if the game object that was hit is inside this list of good zones
                {
                    return RepeatingRayCast(false, raycastHitInfo.point, endPoint);
                }//end if
            }//end for
        }

        return false;

    }

}