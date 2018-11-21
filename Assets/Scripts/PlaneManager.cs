using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneManager : MonoBehaviour
{
	public float Airspeed = 100f;

	// Use this for initialization
	void Start ()
	{
        //play sound
        //randomly select plane model
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position += transform.forward * Time.deltaTime * Airspeed;
	}

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("MapBounds"))
        {
            Destroy(this.gameObject);
        }
    }
}
