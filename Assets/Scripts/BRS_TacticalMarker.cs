using UnityEngine;

public class BRS_TacticalMarker : MonoBehaviour
{
	public GameObject TacticalMarker;

	private float markerOffset;
	private Camera FPCamera;
	private float MinimapCamHeight;
	private Ray ray;
	private RaycastHit hit;
    private GameObject markerObject;

	// Use this for initialization
	void Start ()
	{
		FPCamera = GetComponentInChildren<Camera>();
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
		ray = new Ray(FPCamera.transform.position, FPCamera.transform.forward);
		// Are we pointing at something in the world?
		if (Physics.Raycast(ray, out hit))
		{
            Vector3 markerLocation = new Vector3(hit.point.x, markerOffset, hit.point.z);
            if (markerObject != null)
            {
                Destroy(markerObject);
            }
            markerObject = Instantiate(TacticalMarker, markerLocation, Quaternion.identity, null);
            
		}
	}
}
