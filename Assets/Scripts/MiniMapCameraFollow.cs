using UnityEngine;

public class MiniMapCameraFollow : MonoBehaviour {
    public GameObject miniMapTarget;
    private float height;

	// Use this for initialization
	void Start () {
        height = this.transform.position.y;
		if(miniMapTarget == null)
        {
            miniMapTarget = GameObject.FindGameObjectWithTag("Player");
        }
	}
	
	// Update is called once per frame
	void LateUpdate () {
        this.transform.position = new Vector3(miniMapTarget.transform.position.x, height, miniMapTarget.transform.position.z);
        

    }
}
