using UnityEngine;
using System.Collections.Generic;

public class BRS_TacticalMarker : MonoBehaviour
{
	public GameObject TacticalMarkerPrefab;

	private float tacticalMarkerOffset;
	private Transform FPCameraTransform;
	private float MinimapCamHeight;
    private GameObject tacticalMarkerObject;

    //what is the maximum distance away a player can set a tactical marker
    private readonly int tacticalMarkerPlaceDistanceLimit = 300;

    //Variables for raycasting tooltips
    private GameObject GOPlayerIsCurrentlyLookingAt = null;
    private List<GameObject> interactableObjectsWithinRange = new List<GameObject>();

    //variables for inventory manager
    private InventoryManager inventoryManager;
    private FN_ItemManager itemManagerPlayerIsLookingAt;


    void Start ()
	{
        if(inventoryManager == null)
        {
            inventoryManager = GetComponent<InventoryManager>();
            if(inventoryManager == null)
            {
                Debug.LogError("ERROR! No InventoryManager on player!");
            }
        }
		FPCameraTransform = GetComponentInChildren<Camera>().transform;
		MinimapCamHeight = GameObject.FindGameObjectWithTag ("MiniMap Camera").transform.position.y;
		tacticalMarkerOffset = MinimapCamHeight - 10.0f;//marker always shown below map
	}

	// Update is called once per frame
	void Update ()
	{
        //HandleRaycasting to tooltips
        HandleToolTipRaycasting();

        //Handle Tactical Marker
        if (Input.GetKeyDown (KeyCode.T))
		{
            PlaceTacticalMarker();
		}

        //handle interactions
        if(Input.GetKeyDown(KeyCode.E))
        {
            HandleInteraction();
            
        }

    }

    private void HandleItemPickup()
    {
        if (itemManagerPlayerIsLookingAt != null)//then player can pick up item
        {
            //attempt to add the item to inventory
            if (inventoryManager.AddItem(itemManagerPlayerIsLookingAt))
            {
                interactableObjectsWithinRange.Remove(itemManagerPlayerIsLookingAt.gameObject);
                Destroy(itemManagerPlayerIsLookingAt.gameObject);
            }
        }
        else
        {
            Debug.Log("Pickp Failed:   Player Not Looking at an item. Looking at " + itemManagerPlayerIsLookingAt.name);
        }
    }

    private void HandleInteraction()
    {
        HandleItemPickup();
        //TODO what if the player E's (interacts) with a door or a car?
        
    }

    private void HandleToolTipRaycasting()
    {
        if (interactableObjectsWithinRange.Count > 0)
        {
            bool playerIsLookingAtItem = false;
            FN_ItemManager itemManager;
            //Debug.Log("interactableObjectsWithinRange: " + interactableObjectsWithinRange.Count);
            GOPlayerIsCurrentlyLookingAt = WhatIsPlayerLookingAt();

            for (int i = 0; i < interactableObjectsWithinRange.Count; ++i)
            {
                itemManager = interactableObjectsWithinRange[i].GetComponent<FN_ItemManager>();
                //is the player pointing at an item that is within interaction range?
                playerIsLookingAtItem = itemManager.CompareModel(GOPlayerIsCurrentlyLookingAt);
                if (playerIsLookingAtItem)
                {
                    itemManagerPlayerIsLookingAt = itemManager;
                }
                itemManager.ToggleToolTipVisibility(playerIsLookingAtItem);

            }
        }
        else
        {//there is nothing nearby for the player to look at
            itemManagerPlayerIsLookingAt = null;
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
            tacticalMarkerObject = Instantiate(TacticalMarkerPrefab, markerLocation, Quaternion.identity);
            
		}
	}

    private bool PlayerIsLookingAtTarget(GameObject target)//deprecated
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
            triggerVolume.gameObject.GetComponent<FN_ItemManager>().ToggleToolTipVisibility(false);//stop showing item tooltip
            interactableObjectsWithinRange.Remove(triggerVolume.gameObject);//remove game object from list
        }
        
    }
}
