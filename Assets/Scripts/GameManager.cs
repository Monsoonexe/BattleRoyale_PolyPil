using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [Header("GameSettings")]
    public bool StartInPlane;
    public GameObject[] players;

    public BRS_PlaneDropManager planeDropManager;

    public GameObject zoneWall;
    public BRS_ChangeCircle zoneWallChangeCircle;

    public int playerFlightSpeed = 200;

    private void Awake()
    {
        VerifyReferences();
    }

    private void DeployPlayersInPlane()
    {
        //TODO
        //ERROR! THIS BORKS EVERYTHING UP!!!
        //planeDropManager.LoadPlaneWithCargo(players);
        //planeDropManager.SetFlightSpeed(200);
        //planeDropManager.SetupFlightPath(DropTypeENUM.PLAYER);
        

    }

    // Use this for initialization
    void Start () {

        //populate loot
        //determine mission
        //spawn certain enemies and specific locations
        if (StartInPlane)
        {
            DeployPlayersInPlane();
        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void VerifyReferences()
    {
        if (players == null)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
        }

        if (zoneWall == null)
        {
            zoneWall = GameObject.FindGameObjectWithTag("ZoneWall");
            zoneWallChangeCircle = zoneWall.GetComponentInChildren<BRS_ChangeCircle>();
        }
        else
        {
            if (zoneWallChangeCircle == null)
            {
                zoneWallChangeCircle = zoneWall.GetComponentInChildren<BRS_ChangeCircle>();
            }
        }

    }
}
