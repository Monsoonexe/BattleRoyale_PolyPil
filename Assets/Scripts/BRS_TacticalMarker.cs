using UnityEngine;
using System.Collections.Generic;

public class BRS_TacticalMarker : MonoBehaviour
{
	public GameObject TacticalMarker;

	private float tacticalMarkerOffset;
	private Transform FPCameraTransform;
	private float MinimapCamHeight;
    private GameObject tacticalMarkerObject;

    //what is the maximum distance away a player can set a tactical marker
    private readonly int tacticalMarkerPlaceDistanceLimit = 300;

    //Variables for raycasting tooltips
    private GameObject GOPlayerIsCurrentlyLookingAt = null;
    private List<GameObject> interactableObjectsWithinRange = new List<GameObject>();

    
    void Start ()
	{
		FPCameraTransform = GetComponentInChildren<Camera>().transform;
		MinimapCamHeight = GameObject.FindGameObjectWithTag ("MiniMap Camera").transform.position.y;
		tacticalMarkerOffset = MinimapCamHeight - 10.0f;
	}

	// Update is called once per frame
	void Update ()
	{
        //Handle Tactical Marker
		if (Input.GetKeyDown (KeyCode.T))
		{
            PlaceTacticalMarker();
		}

        //HandleRaycasting to tooltips
        HandleToolTipRaycasting();

    }

    private void HandleToolTipRaycasting()
    {
        if (interactableObjectsWithinRange.Count > 0)
        {
            //Debug.Log("interactableObjectsWithinRange: " + interactableObjectsWithinRange.Count);
            GOPlayerIsCurrentlyLookingAt = WhatIsPlayerLookingAt();

            for (int i = 0; i < interactableObjectsWithinRange.Count; ++i)
            {
                FN_ItemManager itemManager = interactableObjectsWithinRange[i].GetComponent<FN_ItemManager>();
                itemManager.ToggleModelVisible(itemManager.CompareModel(GOPlayerIsCurrentlyLookingAt));

            }


            if (interactableObjectsWithinRange.Contains(GOPlayerIsCurrentlyLookingAt))
            {
                FN_ItemManager itemManager = GOPlayerIsCurrentlyLookingAt.gameObject.GetComponent<FN_ItemManager>();

            }
        }
    }

	private void PlaceTacticalMarker()
	{
        RaycastHit hitInfo;
        // Are we pointing at something in the world?
        if (Physics.Raycast(FPCameraTransform.position, FPCameraTransform.forward, out hitInfo, tacticalMarkerPlaceDistanceLimit))
		{
            Vector3 markerLocation = new Vector3(hitInfo.point.x, tacticalMarkerOffset, hitInfo.point.z);
            if (tacticalMarkerObject != null)
            {
                Destroy(tacticalMarkerObject);
            }
            tacticalMarkerObject = Instantiate(TacticalMarker, markerLocation, Quaternion.identity);
            
		}
	}

    private bool PlayerIsLookingAtTarget(GameObject target)
    {
        bool isLooking = false;
        RaycastHit hitInfo;
        //shoot a raycast from the cameras position forward, store the info, and the ray is limited to this length
        if (Physics.Raycast(FPCameraTransform.position, FPCameraTransform.forward, out hitInfo, tacticalMarkerPlaceDistanceLimit))
        {
            //is the player looking at the item model?
            if (hitInfo.collider.CompareTag("ItemModel"))
            {
                //Debug.Log("The camera is pointing at an item.");
                if(hitInfo.collider.gameObject == target)
                {
                    //Debug.Log("Raycast hit the proper item model.");
                    isLooking = true;
                }
                else
                {
                    //Debug.Log("Raycast did NOT hit the same item model.");
                }
            }
            else
            {
            //Debug.Log("Raycast did not hit an ItemModel.");
            }
        }
        else
        {
            //Debug.Log("Raycast hit nothing.");
        }

        return isLooking;

    }//deprecated

    private GameObject WhatIsPlayerLookingAt()
        //ONLY RETURNS A GO IF PLAYER IS LOOKING AT AN "ItemModel". NO OTHER FUNCTIONALITY
    {
        RaycastHit hitInfo;
        //shoot a raycast from the cameras position forward, store the info, and the ray is limited to this length
        if (Physics.Raycast(FPCameraTransform.position, FPCameraTransform.forward, out hitInfo, tacticalMarkerPlaceDistanceLimit))
        {
            //Debug.Log("Raycast hit: " + hitInfo.collider.gameObject.name);
            //is the player looking at the item model?
            if (hitInfo.collider.CompareTag("ItemModel"))
            {
                return hitInfo.collider.gameObject;
            }
            else
            {
                //Debug.Log("Raycast did not hit an ItemModel.");
            }
        }
        else
        {
            //Debug.Log("Raycast hit nothing.");
        }

        return null;

    }

    public void OnTriggerEnter(Collider triggerVolume)
    {
        if (triggerVolume.CompareTag("Item"))
        {
            //if list does not already contain, then add
            if(!interactableObjectsWithinRange.Contains(triggerVolume.gameObject)) interactableObjectsWithinRange.Add(triggerVolume.gameObject);//add game object to list

        }
    }

    public void OnTriggerStay(Collider triggerVolume)
    {
    }

    public void OnTriggerExit(Collider triggerVolume)
    {
        if (triggerVolume.CompareTag("Item"))
        {
            triggerVolume.gameObject.GetComponent<FN_ItemManager>().ToggleModelVisible(false);//stop showing item tooltip
            interactableObjectsWithinRange.Remove(triggerVolume.gameObject);//remove game object from list
        }
        
    }
}
