using UnityEngine;

public class BRS_TacticalMarker : MonoBehaviour
{
	public GameObject TacticalMarker;

	private float markerOffset;
	private Transform FPCameraTransform;
	private float MinimapCamHeight;
    private GameObject markerObject;

    private readonly int tacticalMarkerPlaceDistanceLimit = 300;

	// Use this for initialization
	void Start ()
	{
		FPCameraTransform = GetComponentInChildren<Camera>().transform;
		MinimapCamHeight = GameObject.FindGameObjectWithTag ("MiniMap Camera").transform.position.y;
		markerOffset = MinimapCamHeight - 10.0f;
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
            Vector3 markerLocation = new Vector3(hitInfo.point.x, markerOffset, hitInfo.point.z);
            if (markerObject != null)
            {
                Destroy(markerObject);
            }
            markerObject = Instantiate(TacticalMarker, markerLocation, Quaternion.identity);
            
		}
	}
}
