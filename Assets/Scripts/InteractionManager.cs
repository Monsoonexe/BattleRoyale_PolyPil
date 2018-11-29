using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour {


    private Transform FPCameraTransform;
    private readonly int interactionRaycastLimit = 100;

    //Variables for raycasting tooltips
    private GameObject GOPlayerIsCurrentlyLookingAt = null;
    private List<GameObject> interactableObjectsWithinRange = new List<GameObject>();

    //variables for inventory manager
    private InventoryManager inventoryManager;
    private FN_ItemManager itemManagerPlayerIsLookingAt;


    // Use this for initialization
    void Start () {
        if (inventoryManager == null)
        {
            inventoryManager = GetComponent<InventoryManager>();
            if (inventoryManager == null)
            {
                Debug.LogError("ERROR! No InventoryManager on player!");
            }
        }
        FPCameraTransform = GetComponentInChildren<Camera>().transform;


    }
	
	// Update is called once per frame
	void Update ()
    {
        //HandleRaycasting to tooltips
        HandleToolTipRaycasting();

        //handle interactions
        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleInteraction();

        }

    }

    private void HandleItemPickup()
    {
        //attempt to add the item to inventory
        if (inventoryManager.AddItem(itemManagerPlayerIsLookingAt))
        {
            interactableObjectsWithinRange.Remove(itemManagerPlayerIsLookingAt.gameObject);
            Destroy(itemManagerPlayerIsLookingAt.gameObject);
        }

    }

    private void HandleInteraction()
    {
        //if player is interacting with an item
        if (itemManagerPlayerIsLookingAt) HandleItemPickup();
        else
        {
            Debug.Log("Nothing to interact with.");
        }

        //if(itemManagerPlayerIsLookingAt == vehicle) enter vehicle
        //if(itemManagerPlayerIsLookingAt == door) open door


    }

    private void HandleToolTipRaycasting()
    {
        if (interactableObjectsWithinRange.Count > 0)
        {
            bool playerIsLookingAtInteractable = false;
            FN_ItemManager itemManager;
            //Debug.Log("interactableObjectsWithinRange: " + interactableObjectsWithinRange.Count);
            GOPlayerIsCurrentlyLookingAt = WhatIsPlayerLookingAt();

            for (int i = 0; i < interactableObjectsWithinRange.Count; ++i)
            {
                itemManager = interactableObjectsWithinRange[i].GetComponent<FN_ItemManager>();
                //is the player pointing at an item that is within interaction range?
                playerIsLookingAtInteractable = itemManager.CompareModel(GOPlayerIsCurrentlyLookingAt);
                if (playerIsLookingAtInteractable)
                {
                    itemManagerPlayerIsLookingAt = itemManager;
                }
                itemManager.ToggleToolTipVisibility(playerIsLookingAtInteractable);

            }
        }
        else
        {//there is nothing nearby for the player to look at
            itemManagerPlayerIsLookingAt = null;
        }
    }
    private GameObject WhatIsPlayerLookingAt()
    //ONLY RETURNS A GO IF PLAYER IS LOOKING AT AN "ItemModel". NO OTHER FUNCTIONALITY
    {
        RaycastHit hitInfo;
        //shoot a raycast from the cameras position forward, store the info, and the ray is limited to this length
        if (Physics.Raycast(FPCameraTransform.position, FPCameraTransform.forward, out hitInfo, interactionRaycastLimit))
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
            if (!interactableObjectsWithinRange.Contains(triggerVolume.gameObject)) interactableObjectsWithinRange.Add(triggerVolume.gameObject);//add game object to list

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
