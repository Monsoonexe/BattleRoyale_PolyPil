using UnityEngine;

public class PlaneManager : MonoBehaviour
{
	public int airspeed = 100;
    public GameObject targetDropZone;
    public GameObject[] cargo;
    public bool hasDroppedCargo = false;



    //public UnityStandardAssets.Characters.FirstPerson.FirstPersonController fpsController;

    public void InitPlane(GameObject[] incomingCargo, GameObject incomingTargetDropZone, int incomingAirSpeed = 100)
    {
        this.cargo = incomingCargo;
        this.targetDropZone = incomingTargetDropZone;
        this.airspeed = incomingAirSpeed;
    }

    // Use this for initialization
    void Start ()
	{
        //play sound
        //randomly select plane model
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position += transform.forward * Time.deltaTime * airspeed;
	}

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("MapBounds"))
        {
            Destroy(this.gameObject);
        }
    }
}
