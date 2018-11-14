using UnityEngine;
using System.Collections;

public class BRS_PlaneDropManager : MonoBehaviour
     
{
    //public GameObject EndPointBall;
    [Header("Map Settings")]
	public int _MapSize = 500;//the size of the traversable map
	[Range(2,9)]
	public int DropZoneRange = 8;//how many different start/stop options should there be?
    public int AcceptableDropZoneSize = 500;//how big should the zone be that the plane flies through
	[Header("Plane Settings")]
	public GameObject BRS_PlaneSpawn;//plane object (model) to spawn
	public float BRS_PlaneAltitude; // how high does it fly?

	public GameObject PlaneStart;//start marker
	public GameObject PlaneStop;//end marker
	public bool VerifiedPath = false;

    private Vector3[] PD_L;//plane drop points left side
    private Vector3[] PD_R;//plane drop points right side

    private int spawnAttempts = 0;// used to track failures. app should pause or fail after this many fails
    private int spawnAttemptsUntilFailure = 10;//default of ten tries

	void Start ()
	{
        //set possible start and end points
        SetupStartAndEndPoints();

        //Get an acceptable flight path
        SetupFlightPath();
	}

    private void SetupStartAndEndPoints()
    {
        PD_L = new Vector3[DropZoneRange];
        PD_R = new Vector3[DropZoneRange];

        //make sure this.gameObject is positioned in the center of the map
        //start position is half the size of the map to the left of the map, 
        //end position is half the size of the map to the right.
        Vector3 setupPosition = new Vector3(this.transform.position.x - _MapSize / 2, BRS_PlaneAltitude, _MapSize);

        for (int i = 0; i < PD_L.Length; i++)
        {
            PD_L[i] = setupPosition;
            setupPosition = new Vector3(this.transform.position.x - _MapSize / 2, BRS_PlaneAltitude, (setupPosition.z - (_MapSize / DropZoneRange)));
            //Instantiate(EndPointBall, setupPosition, Quaternion.identity); // create test object here to visually track spawn points
        }

        setupPosition = new Vector3(this.transform.position.x + _MapSize / 2, BRS_PlaneAltitude, _MapSize);

        for (int i = 0; i < PD_R.Length; i++)
        {
            PD_R[i] = setupPosition;
            setupPosition = new Vector3(_MapSize, BRS_PlaneAltitude, (setupPosition.z - (_MapSize / DropZoneRange)));
            //Instantiate(EndPointBall, setupPosition, Quaternion.identity); // create test object here to visually track spawn points
        }

    }

	private void SetupFlightPath()
	{
        // Let's find a path that is certainly THROUGH the cylinder

        //Create the cylinder for flight check
        GameObject ADZ = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        ADZ.transform.position = this.gameObject.transform.position;//sets the position relative to this.gameObject.
                                                                    //make sure this.gameObject is positioned in the center of the playable map
        ADZ.transform.localScale = new Vector3(AcceptableDropZoneSize, AcceptableDropZoneSize, AcceptableDropZoneSize);//set the size of the cylinder to the size of the desired drop zone
        ADZ.name = "AcceptableDropZone";//name this new cylinder

        //Debug.Log("Start");//print test
        VerifiedPath = false;

        do
        {
			Debug.Log("Planing optimal Route");

            //Pick a Random startpoint from list
            PlaneStart.transform.position = PD_L[Random.Range(0, PD_L.Length)];
            //Pick a Random endpoint from list
            PlaneStop.transform.position = PD_R[Random.Range(0, PD_R.Length)];
            
            RaycastHit objectHit;
            if (Physics.Raycast(PlaneStart.transform.position, PlaneStart.transform.forward, out objectHit, _MapSize))
            {

                //Debug.Log("Trying " + numberOfAttempts++ + " times");
                if (objectHit.collider.gameObject.name == "AcceptableDropZone")
                {
                    VerifiedPath = true;
                    //Debug.Log("Optimal Route Calculated");
                    //destroy the cylinder when done using it
                    Destroy(objectHit.collider.gameObject);
                }
            }
            //protect from infinite loop by only testing a certain number of times before quitting
            ++spawnAttempts;
            if (spawnAttempts > spawnAttemptsUntilFailure)
            {
                Debug.LogError("ERROR:  " + spawnAttempts.ToString() + "spawnAttempts with no success. Quitting.....");
                Debug.Break();//stop the game
            }
        } while (VerifiedPath != true);

    }

	void LateUpdate()
	{

        if (VerifiedPath)
        {
            SpawnPlane();
        }
	}

	void SpawnPlane()
	{
		PlaneStart.transform.LookAt (PlaneStop.transform);//point plane towards endpoint
		Instantiate (BRS_PlaneSpawn, PlaneStart.transform.position, PlaneStart.transform.rotation);
		VerifiedPath = false;//reset
        //seppuku! this object is no longer needed
        Destroy(this.gameObject);
	}
}