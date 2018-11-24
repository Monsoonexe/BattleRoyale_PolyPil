using UnityEngine;

public class BRS_TacticalMarker : MonoBehaviour
{
	public GameObject TacticalMarker;

	private float tacticalMarkerOffset;
	private Transform FPCameraTransform;
	private float MinimapCamHeight;
    private GameObject tacticalMarkerObject;

    //what is the maximum distance away a player can set a tactical marker
    private readonly int tacticalMarkerPlaceDistanceLimit = 300;

    // Use this for initialization

    
    void Start ()
	{
		FPCameraTransform = GetComponentInChildren<Camera>().transform;
		MinimapCamHeight = GameObject.FindGameObjectWithTag ("MiniMap Camera").transform.position.y;
		tacticalMarkerOffset = MinimapCamHeight - 10.0f;
	}

	// Update is called once per frame
	void Update ()
	{
			if (Input.GetKeyDown (KeyCode.T))
			{
				PlaceMarker();
			}
	}

	private void PlaceMarker()
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

    private bool PlayerIsLookingAtItem(GameObject target)
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

    }

    public void OnTriggerStay(Collider triggerVolume)
    {
        //did we enter the volume of an item
        if (triggerVolume.CompareTag("Item"))
        {
            //get the item's itemManager
            FN_ItemManager itemManager = triggerVolume.gameObject.GetComponent<FN_ItemManager>();

            //TODO Restrict code to not fire raycasts 30x a second. This can get expensive if there are a lot of items around.
            //set the visibility to whether or not the player is looking at the model.
            itemManager.ToggleModelVisible(PlayerIsLookingAtItem(itemManager.GetItemModel()));
            

        }
    }

    public void OnTriggerExit(Collider triggerVolume)
    {
        if (triggerVolume.CompareTag("Item"))
        {
            triggerVolume.gameObject.GetComponent<FN_ItemManager>().ToggleModelVisible(false);
        }
        
    }
}
